using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NegociosElectronicosII.Models;

namespace NegociosElectronicosII.Controllers
{
    public class CarritoController : BaseController 
    {
        // GET: Carrito
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult AgregarCarro(int id, int constante, int lugar)
        {
            if (Settings.LoggedUser == null)
                return Json(new { Success = false, Message = "Necesitas iniciar sesion" }, JsonRequestBehavior.DenyGet);
            
            NE_Carrito nE_Carrito;
            if (constante == 1)
            {
                if(db.NE_Carrito.Any(x=>x.VehiculoId==id && x.UsuarioId==Settings.LoggedUser.UsuarioId))
                    return Json(new { Success = false, Message = "Este articulo ya esta en el carrito" }, JsonRequestBehavior.DenyGet);
                nE_Carrito = new NE_Carrito()
                {
                    RecordDate = DateTime.Now,
                    UsuarioId = Settings.LoggedUser.UsuarioId,
                    ProductoId = null,
                    VehiculoId = id,

                };
                NE_Vehiculo nE_Vehiculo = db.NE_Vehiculo.Where(x => x.VehiculoId == id).First();
                NE_Bitacora bitacora = new NE_Bitacora()
                {
                    AccionId = ACCION.AGREGAR_CARRO,
                    Descripcion = "el usuario : " + Settings.LoggedUser.CorreoElectronico + " Agrego al carrito '" + nE_Vehiculo.NE_Marca.Marca + " " + nE_Vehiculo.NombreVehiculo + " " + nE_Vehiculo.Modelo + "'",
                    FechaDeRegistro = DateTime.Now,
                    UsuarioId = Settings.LoggedUser.UsuarioId,
                };
                db.NE_Bitacora.Add(bitacora);
                db.SaveChanges();
                if (lugar == 2)
                {
                    NE_ListaDeDeseos nE_Lista = db.NE_ListaDeDeseos.Where(x => x.VehiculoId == id).First();
                    db.NE_ListaDeDeseos.Remove(nE_Lista);
                }
            }
            else
            {
                if (db.NE_Carrito.Any(x => x.ProductoId == id && x.UsuarioId==Settings.LoggedUser.UsuarioId))
                    return Json(new { Success = false, Message = "Este articulo ya esta en el carrito" }, JsonRequestBehavior.DenyGet);
                nE_Carrito = new NE_Carrito()
                {
                    RecordDate = DateTime.Now,
                    UsuarioId = Settings.LoggedUser.UsuarioId,
                    ProductoId = id,
                    VehiculoId = null,

                };
                NE_Producto nE_Producto = db.NE_Producto.Where(x => x.ProductoId == id).First();
                NE_Bitacora bitacora = new NE_Bitacora()
                {
                    AccionId = ACCION.AGREGAR_CARRO,
                    Descripcion = "el usuario : " + Settings.LoggedUser.CorreoElectronico + " Agrego al carrito '" + nE_Producto.Nombre + " de "+nE_Producto.NE_Marca.Marca+" '",
                    FechaDeRegistro = DateTime.Now,
                    UsuarioId = Settings.LoggedUser.UsuarioId,
                };
                db.NE_Bitacora.Add(bitacora);
                db.SaveChanges();
                if (lugar == 2)
                {
                    NE_ListaDeDeseos nE_Lista = db.NE_ListaDeDeseos.Where(x => x.ProductoId == id).First();
                    db.NE_ListaDeDeseos.Remove(nE_Lista);
                }
            }
            db.NE_Carrito.Add(nE_Carrito);
            db.SaveChanges();
            return Json(new { Success = true, Message = "Añadido al Carrito" }, JsonRequestBehavior.DenyGet);
        }

        public PartialViewResult CarritoDesplegable()
        {
            if (Settings.LoggedUser != null)
            {
                int numero = db.NE_Carrito.Where(x => x.UsuarioId == Settings.LoggedUser.UsuarioId).Count();
                ViewBag.TotalCarro = numero;
                return PartialView();
            }
            else
                return PartialView();
        }

        [HttpPost]
        public JsonResult EliminarCarro(int id, int constante)
        {

            if (Settings.LoggedUser == null)
                return Json(new { Success = false, Message = "Necesitas iniciar sesion" }, JsonRequestBehavior.DenyGet);
            NE_Carrito nE_Carrito= new NE_Carrito();
            if (constante == 1)
            {
                nE_Carrito = db.NE_Carrito.Where(x=>x.VehiculoId==id).First();
                NE_Vehiculo nE_Vehiculo = db.NE_Vehiculo.Where(x => x.VehiculoId == id).First();
                NE_Bitacora bitacora = new NE_Bitacora()
                {
                    AccionId = ACCION.ELIMINAR_CARRO,
                    Descripcion = "el usuario : " + Settings.LoggedUser.CorreoElectronico + " Elimino del carrito '" + nE_Vehiculo.NE_Marca.Marca + " " + nE_Vehiculo.NombreVehiculo + " " + nE_Vehiculo.Modelo + "'",
                    FechaDeRegistro = DateTime.Now,
                    UsuarioId = Settings.LoggedUser.UsuarioId,
                };
                db.NE_Bitacora.Add(bitacora);

            }
            else
            {
                nE_Carrito = db.NE_Carrito.Where(x => x.ProductoId == id).First();
                NE_Producto nE_Producto = db.NE_Producto.Where(x => x.ProductoId == id).First();
                NE_Bitacora bitacora = new NE_Bitacora()
                {
                    AccionId = ACCION.ELIMINAR_CARRO,
                    Descripcion = "el usuario : " + Settings.LoggedUser.CorreoElectronico + " Elimino del Carrito '" + nE_Producto.Nombre + " de " + nE_Producto.NE_Marca.Marca + " '",
                    FechaDeRegistro = DateTime.Now,
                    UsuarioId = Settings.LoggedUser.UsuarioId,
                };
                db.NE_Bitacora.Add(bitacora);
            }
            db.NE_Carrito.Remove(nE_Carrito);
            db.SaveChanges();
            return Json(new { Success = true, Message = "Se elimino el producto" }, JsonRequestBehavior.DenyGet);
        }




    }
}