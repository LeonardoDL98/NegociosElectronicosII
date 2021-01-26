using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using NegociosElectronicosII.Models;


namespace NegociosElectronicosII.Controllers
{
    public class RegistroController : BaseController
    {

        // GET: Registro
        public ActionResult Index()
        {
            ViewBag.Message = String.Empty;
            var nE_Usuario = db.NE_Usuario.Include(n => n.NE_Rol).Include(n => n.NE_Sexo);
            return View(nE_Usuario.ToList());
        }

        // GET: Registro/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NE_Usuario nE_Usuario = db.NE_Usuario.Find(id);
            if (nE_Usuario == null)
            {
                return HttpNotFound();
            }
            return View(nE_Usuario);
        }

        // GET: Registro/Create
        public ActionResult Create()
        {
            ViewBag.RolId = new SelectList(db.NE_Rol, "RolId", "Rol");
            ViewBag.SexoId = new SelectList(db.NE_Sexo, "SexoId", "Sexo");
            ViewBag.Message = String.Empty;
            return View();
        }

        // POST: Registro/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UsuarioId,Nombre,ApellidoPaterno,ApellidoMaterno,SexoId,Edad,Direccion,Telefono,CorreoElectronico,Activo,RolId,Password")] NE_Usuario nE_Usuario)
        {
            ViewBag.Message = String.Empty;
            if (ModelState.IsValid)
            {

               
                using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    NE_Autenticacion userAuth = new NE_Autenticacion();

                    try
                    {
                        if (!db.NE_Usuario.Any(x=>x.CorreoElectronico.ToUpper()==nE_Usuario.CorreoElectronico.ToUpper()))
                        {

                            db.NE_Usuario.Add(nE_Usuario);
                            db.SaveChanges();
                            userAuth = new NE_Autenticacion()
                            {
                                UsuarioId = nE_Usuario.UsuarioId,
                                Intentos = 0,
                                CuentaBloqueada = false,
                                Contrasena = Security.Security.Encrypt(nE_Usuario.password),
                                UltimoInicioSesion = DateTime.Now,
                            };

                            db.NE_Autenticacion.Add(userAuth);
                            db.SaveChanges();
                            dbTran.Commit();

                            NE_Bitacora bitacora = new NE_Bitacora() {
                                AccionId= ACCION.NUEVO_REGISTRO,
                                Descripcion= "Se ha creado al usuario: " + nE_Usuario.CorreoElectronico,
                                FechaDeRegistro= DateTime.Now,
                                UsuarioId= nE_Usuario.UsuarioId,
                            };
                            db.NE_Bitacora.Add(bitacora);
                            db.SaveChanges();
                            ViewBag.Message = "Cuenta creada";
                            return RedirectToAction("Index", "Login"); 
                        }
                        else
                        {
                            ViewBag.Message = "Ya hay un correo registrado";
                        }
                            
                    }
                    catch (Exception e)
                    {
                        ViewBag.Message = "La cuenta no se pudo crear";
                        return RedirectToAction("Create", "Registro");
                    }


                }
                
            }

            ViewBag.RolId = new SelectList(db.NE_Rol, "RolId", "Rol", nE_Usuario.RolId);
            ViewBag.SexoId = new SelectList(db.NE_Sexo, "SexoId", "Sexo", nE_Usuario.SexoId);
            return View(nE_Usuario);
        }

        // GET: Registro/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NE_Usuario nE_Usuario = db.NE_Usuario.Find(id);
            if (nE_Usuario == null)
            {
                return HttpNotFound();
            }
            ViewBag.RolId = new SelectList(db.NE_Rol, "RolId", "Rol", nE_Usuario.RolId);
            ViewBag.SexoId = new SelectList(db.NE_Sexo, "SexoId", "Sexo", nE_Usuario.SexoId);
            return View(nE_Usuario);
        }

        // POST: Registro/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UsuarioId,Nombre,ApellidoPaterno,ApellidoMaterno,SexoId,Edad,Direccion,Telefono,CorreoElectronico,Activo,RolId")] NE_Usuario nE_Usuario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nE_Usuario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RolId = new SelectList(db.NE_Rol, "RolId", "Rol", nE_Usuario.RolId);
            ViewBag.SexoId = new SelectList(db.NE_Sexo, "SexoId", "Sexo", nE_Usuario.SexoId);
            return View(nE_Usuario);
        }

        // GET: Registro/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NE_Usuario nE_Usuario = db.NE_Usuario.Find(id);
            if (nE_Usuario == null)
            {
                return HttpNotFound();
            }
            return View(nE_Usuario);
        }

        // POST: Registro/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NE_Usuario nE_Usuario = db.NE_Usuario.Find(id);
            db.NE_Usuario.Remove(nE_Usuario);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public void SendPassToAccount(Int32 ID_User, string password)
        {
           
        }

    }
}
