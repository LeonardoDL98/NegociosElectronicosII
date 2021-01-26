using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NegociosElectronicosII.Models;

namespace NegociosElectronicosII.Controllers
{
    public class ChangePassController : BaseController
    {
        // GET: ChangePass
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult Passwoord()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult ChangePas(String newPass)
        {
            try
            {
                NE_Autenticacion auth = db.NE_Autenticacion.Where(x => x.UsuarioId == Settings.LoggedUser.UsuarioId).First();
                auth.Contrasena = Security.Security.Encrypt(newPass);
                db.SaveChanges();

                return Json(new { Success = true }, JsonRequestBehavior.DenyGet);
            }
            catch
            {
                return Json(new { Success = false }, JsonRequestBehavior.DenyGet);
            }
        }
    }
}