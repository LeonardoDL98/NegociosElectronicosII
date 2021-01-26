using NegociosElectronicosII.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NegociosElectronicosII.Controllers
{
    public class BaseController : Controller
    {
        public DB_NE_Entitties db = new DB_NE_Entitties();


        public PartialViewResult ComboDeCatergoriasParcial() {
            return PartialView(db.NE_Categoria.ToList());
        }

        public PartialViewResult BusquedaParcial() {
            return PartialView();
        }

        public List<T> DesordenarLista<T>(List<T> input)
        {
            List<T> arr = input;
            List<T> arrDes = new List<T>();

            Random randNum = new Random();
            while (arr.Count > 0)
            {
                int val = randNum.Next(0, arr.Count - 1);
                arrDes.Add(arr[val]);
                arr.RemoveAt(val);
            }

            return arrDes;
        }

        [HttpPost]
        public JsonResult RegistroBitacora(String texto, Int32 AccionId)
        {
            
            if (Settings.LoggedUser != null)
            {
                NE_Bitacora bitacora = new NE_Bitacora()
                {
                    AccionId = AccionId,
                    Descripcion = texto,
                    FechaDeRegistro = DateTime.Now,
                    UsuarioId = Settings.LoggedUser.UsuarioId,
                };

                db.NE_Bitacora.Add(bitacora);
                db.SaveChanges();
            }
            else
                return Json(new { Message="" }, JsonRequestBehavior.DenyGet);

            return Json(new { },JsonRequestBehavior.DenyGet);
        }
    }
}