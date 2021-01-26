using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NegociosElectronicosII.Models;

namespace NegociosElectronicosII.Controllers
{
    public class DetalleProductoController : BaseController
    {
        // GET: DetalleProducto
        public ActionResult Index()
        {
            ViewBag.Message = String.Empty;
            return View();
        }

        [HttpPost]
        public PartialViewResult Detalles(int idprod, int tipoprod)
        {
            try
            {

                DetalleModel detalle = new DetalleModel();
                detalle.tipo = tipoprod;
                if (tipoprod == 1)
                {
                    detalle.vehiculo = db.NE_Vehiculo.Where(x => x.VehiculoId == idprod).First();
                    return PartialView(detalle.vehiculo);
                }
                else
                {
                    detalle.producto = db.NE_Producto.Where(x => x.ProductoId == idprod).First();
                    return PartialView(detalle.producto);
                }
            }
            catch (Exception e)
            {
                ViewBag.Message = e;
                return PartialView();

            }

        }
    }
}