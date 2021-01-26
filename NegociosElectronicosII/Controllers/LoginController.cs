using NegociosElectronicosII.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NegociosElectronicosII.Email;
using NegociosElectronicosII.GlobalCode;


namespace NegociosElectronicosII.Controllers
{
    public class LoginController : BaseController
    {
        
        public ActionResult Index()
        {
            ViewBag.Message = String.Empty;
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginModel model)
        {
            NE_Autenticacion userAuth = new NE_Autenticacion();
            NE_Usuario user = new NE_Usuario();
            string pass;
            String Message = String.Empty;

            if (db.NE_Usuario.Any(x => x.CorreoElectronico == model.Email && x.Activo))
            {
                user = db.NE_Usuario.Where(x => x.CorreoElectronico == model.Email).First();
                pass = Security.Security.Encrypt(model.Password);
                if (!db.NE_Autenticacion.Any(x => x.UsuarioId == user.UsuarioId && x.Contrasena == pass))
                {
                    userAuth = db.NE_Autenticacion.Where(x => x.UsuarioId == user.UsuarioId).First();
                    userAuth.Intentos = userAuth.Intentos + 1;
                    if (userAuth.Intentos >= 5)
                    {
                        userAuth.CuentaBloqueada = true;
                        db.SaveChanges();
                        ViewBag.Message = Recursos.CuentBloq;
                        return View(model);

                    }
                    db.SaveChanges();
                    ViewBag.Message = Recursos.IncorrectPass;
                    return View(model);
                }
                else
                {
                    userAuth = db.NE_Autenticacion.Where(x => x.UsuarioId == user.UsuarioId).First();
                    if (userAuth.CuentaBloqueada)
                    {
                        ViewBag.Message = Recursos.CuentBloq;
                        return RedirectToAction("Index","Home");
                    }
                    else
                    {
                        user = db.NE_Usuario.Where(x => x.CorreoElectronico == model.Email).First();
                        userAuth = db.NE_Autenticacion.Where(x => x.UsuarioId == user.UsuarioId).First();
                        userAuth.Intentos = 0;
                        userAuth.UltimoInicioSesion = DateTime.Now;
                        Settings.LoggedUser = user;
                        db.SaveChanges();
                        ViewBag.Message = "Bienvenido"+Settings.LoggedUser.Nombre;

                        NE_Bitacora bitacora = new NE_Bitacora()
                        {
                            AccionId = ACCION.INICIO_DE_SESION,
                            Descripcion = "el usuario : " + user.CorreoElectronico + " ha iniciado sesion",
                            FechaDeRegistro = DateTime.Now,
                            UsuarioId = user.UsuarioId,
                        };
                        db.NE_Bitacora.Add(bitacora);
                        db.SaveChanges();

                        if (user.RolId==4)
                            return RedirectToAction("Index", "Principal");
                        if(user.RolId==1)
                            return RedirectToAction("Index", "Vehiculo");

                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            else
            {
                ViewBag.Message = Recursos.ErrorEmail;
                return View(model);
            }
        }

        public ActionResult RecoveryPass()
        {
            return View();
        }

        [HttpPost]
        public JsonResult IsEmailValid(String email)
        {
            try
            {
                if (db.NE_Usuario.Any(x => x.CorreoElectronico.ToUpper() == email.ToUpper()))
                {
                    //get user
                    NE_Usuario user = db.NE_Usuario.Where(x => x.CorreoElectronico.ToUpper() == email.ToUpper()).First();
                    //add confirmed request
                    NE_RecoveryPassword recovery = new NE_RecoveryPassword()
                    {
                        UsuarioId = user.UsuarioId,
                        RecordDate = DateTime.Now,
                        ExpiredDate = DateTime.Now.AddDays(1),
                        IsConfirmed = false,
                    };
                    db.NE_RecoveryPassword.Add(recovery);
                    db.SaveChanges();

                    //fill template
                    String template = db.NE_EmailTemplate.Where(x => x.Name == "RecoveryPass").First().EmailTemplate;
                    template = String.Format(template, user.Nombre + " " + user.ApellidoPaterno+" "+user.ApellidoMaterno,Settings.URL_TOConfirmEmail + recovery.RecoveryPasswordId.ToString(), "carsold22141024@gmail.com");
                    //create Instance
                   
                    Mail mail = new Mail()
                    {
                        AccountServer = Settings.ACCOUNT_SERVER,
                        Subject = Recursos.RestablecerPass,
                        From = Settings.FROM,
                        Host = Settings.HOST_SERVER,
                        PasswordServer = Settings.PASSWORD_SERVER,
                        Body = template,
                        To = new List<string>() { user.CorreoElectronico },
                        Port = Settings.PORT_SERVER
                    };
                    mail.Send();

                    return Json(new { Success = true, Message = Recursos.ConfEmail }, JsonRequestBehavior.DenyGet);
                }
                else
                    return Json(new { Success = false, Message = Recursos.ErrorEmail }, JsonRequestBehavior.DenyGet);

            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = Recursos.MessError }, JsonRequestBehavior.DenyGet);
            }
        }

        public ActionResult Confirm(Int32 ID)
        {
            NE_RecoveryPassword model = db.NE_RecoveryPassword.Find(ID);
            ViewBag.IsConfirmValid = model.ExpiredDate > DateTime.Now;
            ViewBag.IsConfirmed = model.IsConfirmed;

            String Message = String.Empty;
            ViewBag.ID = ID;
            //TODO: redirect to change pass
            return View();
        }

        [HttpPost]
        public JsonResult ChangePass(String newPass, Int32 ID)
        {
            try
            {
                NE_RecoveryPassword model = db.NE_RecoveryPassword.Find(ID);
                NE_Usuario user = db.NE_Usuario.Where(x => x.UsuarioId == model.UsuarioId).First();
                NE_Autenticacion auth = db.NE_Autenticacion.Where(x => x.UsuarioId == user.UsuarioId).First();
                model.IsConfirmed = true;
                auth.Contrasena= Security.Security.Encrypt(newPass);
                db.SaveChanges();

                return Json(new { Success = true }, JsonRequestBehavior.DenyGet);
            }
            catch
            {
                return Json(new { Success = false }, JsonRequestBehavior.DenyGet);
            }
        }

        public ActionResult LogOut()
        {
            Settings.LoggedUser = null;
            return RedirectToAction("Index", "Principal");
        }

        public ActionResult LoginConGoogle() {
            //your client id  
            string clientid = "61788243292-fo8st62hkof639inq7gcs1rsjofrg2jq.apps.googleusercontent.com";
            //your client secret  
            string clientsecret = "bYVnZkif0jkmpKkFWTeqaKac";
            //your redirection url  
            string redirection_url = Request.Url.AbsoluteUri.Replace("LoginConGoogle", "PuntoFinalGoogle");
            string url = "https://accounts.google.com/o/oauth2/v2/auth?scope=profile&include_granted_scopes=true&redirect_uri=" + redirection_url + "&response_type=code&client_id=" + clientid + "";
            return Redirect(url);
        }

        public ActionResult PuntoFinalGoogle() {

            return View();
        }

        public ActionResult LoginWithCode() {
            return View();
        }

        [HttpPost]
        public JsonResult SolicitarCodigo(String email) {

            if (db.NE_Usuario.Any(x => x.CorreoElectronico.ToUpper() == email.ToUpper()))
            {
                NE_Usuario usuario= db.NE_Usuario.Where(x => x.CorreoElectronico.ToUpper() == email.ToUpper()).First();
                Random aleatorio = new Random();
                Int32 Code= aleatorio.Next(100000,999999);
                DateTime ahora = DateTime.Now;

                NE_AutenticacionConEmail autenticacion = new NE_AutenticacionConEmail()
                {
                    CodigoConfirmado = false,
                    CodigoDeVerificacion = Code.ToString(),
                    Email = email,
                    FechaDeSolicitud = ahora,
                    FechaDeVencimiento = ahora.AddMinutes(5)
                };
                db.NE_AutenticacionConEmail.Add(autenticacion);
                db.SaveChanges();

                //fill template
                String template = db.NE_EmailTemplate.Where(x => x.Name == "Auth").First().EmailTemplate;
                template = String.Format(template, usuario.CorreoElectronico, Code.ToString() , "carsold22141024@gmail.com");
                
                //create Instance
                Mail mail = new Mail()
                {
                    AccountServer = Settings.ACCOUNT_SERVER,
                    Subject = "Codigo de recuperacion",
                    From = Settings.FROM,
                    Host = Settings.HOST_SERVER,
                    PasswordServer = Settings.PASSWORD_SERVER,
                    Body = template,
                    To = new List<string>() { email},
                    Port = Settings.PORT_SERVER
                };
                mail.Send();

                return Json(new { Mensaje = "Se ha enviado un codigo a tu correo, en caso de no ser asi, da clic en reenviar codigo.", TipoMensaje = 1 }, JsonRequestBehavior.DenyGet);
            }
            else
            {
                return Json(new { Mensaje= "No existe un usuario con esa cuenta, verifique el email", TipoMensaje=2 }, JsonRequestBehavior.DenyGet);
            }
        }

        [HttpPost]
        public JsonResult ConfirmarCodigoLogin(String email,String codigo) {

            NE_Autenticacion userAuth = new NE_Autenticacion();
            NE_Usuario user = new NE_Usuario();
            string pass;
            String Message = String.Empty;

            if (db.NE_Usuario.Any(x => x.CorreoElectronico == email && x.Activo))
            {
                user = db.NE_Usuario.Where(x => x.CorreoElectronico == email).First();
                if (db.NE_AutenticacionConEmail.Any(x=>!x.CodigoConfirmado && x.Email.ToUpper()== email.ToUpper()))
                {
                    NE_AutenticacionConEmail model = db.NE_AutenticacionConEmail.Where(x => !x.CodigoConfirmado && x.Email.ToUpper() == email.ToUpper()).OrderByDescending(x=>x.FechaDeSolicitud).First();
                    if (DateTime.Now > model.FechaDeVencimiento)
                        return Json(new { TipoMensaje=2, Mensaje= "El codigo caduco , por favor de clic en reenviar para recibir otro.", UrlAredireccionar="" }, JsonRequestBehavior.DenyGet);

                    if (model.CodigoDeVerificacion != codigo)
                        return Json(new { TipoMensaje = 2, Mensaje = "El cofigo seleccionado no es valido", UrlAredireccionar = "" }, JsonRequestBehavior.DenyGet);

                    userAuth = db.NE_Autenticacion.Where(x => x.UsuarioId == user.UsuarioId).First();
                    userAuth.Intentos = 0;
                    userAuth.UltimoInicioSesion = DateTime.Now;
                    Settings.LoggedUser = user;
                    db.SaveChanges();

                    model.CodigoConfirmado = true;
                    db.SaveChanges();

                    NE_Bitacora bitacora = new NE_Bitacora()
                    {
                        AccionId = ACCION.INICIO_DE_SESION,
                        Descripcion = "el usuario : " + user.CorreoElectronico + " ha iniciado sesion",
                        FechaDeRegistro = DateTime.Now,
                        UsuarioId = user.UsuarioId,
                    };
                    db.NE_Bitacora.Add(bitacora);
                    db.SaveChanges();

                    String url = user.RolId == 4 ? Url.Action("Index", "Principal") : Url.Action("Index", "Vehiculo");
                    return Json(new { TipoMensaje = 1, Mensaje = String.Empty, UrlAredireccionar = url }, JsonRequestBehavior.DenyGet);
                }
                else
                    return Json(new { TipoMensaje = 2, Mensaje = "El codigo seleccionado no es correcto", UrlAredireccionar = "" }, JsonRequestBehavior.DenyGet);
            }
            else
                return Json(new { TipoMensaje = 2, Mensaje = "La cuenta seleccionada no esta activa", UrlAredireccionar = "" }, JsonRequestBehavior.DenyGet);
        }

    }

}
