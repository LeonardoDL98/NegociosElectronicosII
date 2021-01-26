using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NegociosElectronicosII.Models;

namespace NegociosElectronicosII.Controllers
{
    public class ListaDeseosController : BaseController
    {
        // GET: ListaDeseos
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AgregarDeseo(int id, int constante)
        {

            if (Settings.LoggedUser == null)
                return Json(new { Success = false, Message = "Necesitas iniciar sesion" }, JsonRequestBehavior.DenyGet);
            NE_ListaDeDeseos nE_ListaDeseos;
            if (constante == 1)
            {
                if (db.NE_ListaDeDeseos.Any(x => x.VehiculoId == id && x.UsuarioId == Settings.LoggedUser.UsuarioId))
                    return Json(new { Success = false, Message = "Este articulo ya esta en la lista de deseos" }, JsonRequestBehavior.DenyGet);

                nE_ListaDeseos = new NE_ListaDeDeseos()
                {
                    RecordDate = DateTime.Now,
                    UsuarioId = Settings.LoggedUser.UsuarioId,
                    ProductoId = null,
                    VehiculoId = id,

                };
                NE_Vehiculo nE_Vehiculo = db.NE_Vehiculo.Where(x => x.VehiculoId == id).First();
                NE_Bitacora bitacora = new NE_Bitacora()
                {
                    AccionId = ACCION.AGREGAR_LISTA_DESEOS,
                    Descripcion = "el usuario : " + Settings.LoggedUser.CorreoElectronico + " Agrego a Lista de Deseos '" + nE_Vehiculo.NE_Marca.Marca + " " + nE_Vehiculo.NombreVehiculo + " " + nE_Vehiculo.Modelo + "'",
                    FechaDeRegistro = DateTime.Now,
                    UsuarioId = Settings.LoggedUser.UsuarioId,
                };
                db.NE_Bitacora.Add(bitacora);

            }
            else
            {
                if (db.NE_ListaDeDeseos.Any(x => x.ProductoId == id && x.UsuarioId == Settings.LoggedUser.UsuarioId))
                    return Json(new { Success = false, Message = "Este articulo ya esta en la Lista de deseos" }, JsonRequestBehavior.DenyGet);

                nE_ListaDeseos = new NE_ListaDeDeseos()
                {
                    RecordDate = DateTime.Now,
                    UsuarioId = Settings.LoggedUser.UsuarioId,
                    ProductoId = id,
                    VehiculoId = null,

                };
                NE_Producto nE_Producto = db.NE_Producto.Where(x => x.ProductoId == id).First();
                NE_Bitacora bitacora = new NE_Bitacora()
                {
                    AccionId = ACCION.AGREGAR_LISTA_DESEOS,
                    Descripcion = "el usuario : " + Settings.LoggedUser.CorreoElectronico + " Agrego a Lista de Deseos '" + nE_Producto.Nombre + " de " + nE_Producto.NE_Marca.Marca + " '",
                    FechaDeRegistro = DateTime.Now,
                    UsuarioId = Settings.LoggedUser.UsuarioId,
                };
                db.NE_Bitacora.Add(bitacora);
            }
            db.NE_ListaDeDeseos.Add(nE_ListaDeseos);
            db.SaveChanges();
            return Json(new { Success = true, Message = "Se añadio a la lista de deseos" }, JsonRequestBehavior.DenyGet);
        }
        public PartialViewResult ListaDesplegable()
        {
            if (Settings.LoggedUser != null)
            {
                int numero = db.NE_ListaDeDeseos.Where(x => x.UsuarioId == Settings.LoggedUser.UsuarioId).Count();
                ViewBag.TotalDeseos = numero;
                return PartialView();
            }
            else
                return PartialView();
        }
        [HttpPost]
        public JsonResult EliminarDeseado(int id, int constante)
        {

            if (Settings.LoggedUser == null)
                return Json(new { Success = false, Message = "Necesitas iniciar sesion" }, JsonRequestBehavior.DenyGet);
            NE_ListaDeDeseos nE_ListaDeseos = new NE_ListaDeDeseos();
            if (constante == 1)
            {
                nE_ListaDeseos = db.NE_ListaDeDeseos.Where(x => x.VehiculoId == id).First();
                NE_Vehiculo nE_Vehiculo = db.NE_Vehiculo.Where(x => x.VehiculoId == id).First();
                NE_Bitacora bitacora = new NE_Bitacora()
                {
                    AccionId = ACCION.ELIMINAR_LISTA_DESEOS,
                    Descripcion = "el usuario : " + Settings.LoggedUser.CorreoElectronico + " Elimino de Lista de Deseos '"+nE_Vehiculo.NE_Marca.Marca+" " + nE_Vehiculo.NombreVehiculo +" "+nE_Vehiculo.Modelo+ "'",
                    FechaDeRegistro = DateTime.Now,
                    UsuarioId = Settings.LoggedUser.UsuarioId,
                };
                db.NE_Bitacora.Add(bitacora);
            }
            else
            {
                nE_ListaDeseos = db.NE_ListaDeDeseos.Where(x => x.ProductoId == id).First();
                NE_Producto nE_Producto = db.NE_Producto.Where(x => x.ProductoId == id).First();
                NE_Bitacora bitacora = new NE_Bitacora()
                {
                    AccionId = ACCION.ELIMINAR_LISTA_DESEOS,
                    Descripcion = "el usuario : " + Settings.LoggedUser.CorreoElectronico + " Elimino de Lista de Deseos '" + nE_Producto.Nombre + " de " + nE_Producto.NE_Marca.Marca + " '",
                    FechaDeRegistro = DateTime.Now,
                    UsuarioId = Settings.LoggedUser.UsuarioId,
                };
                db.NE_Bitacora.Add(bitacora);
            }
            db.NE_ListaDeDeseos.Remove(nE_ListaDeseos);
            db.SaveChanges();
            return Json(new { Success = true, Message = "Se elimino el producto de Deseados" }, JsonRequestBehavior.DenyGet);
        }



    }
}