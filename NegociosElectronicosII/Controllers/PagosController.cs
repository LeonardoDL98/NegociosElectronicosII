using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NegociosElectronicosII.Models;
using Newtonsoft.Json;

namespace NegociosElectronicosII.Controllers
{
    public class PagosController : BaseController
    {
       
        // GET: Pagos
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult FormasPago()
        {
            return PartialView();
        }
        

        [HttpPost]
        public JsonResult AgregarVenta(int tipopago, int precio, string valores)
        {
            Int32 TipoDeMensage = 2;
            if (Settings.LoggedUser == null)
                return Json(new { Success = false, Message = "Primero de tiene que iniciar sesion" , TipoDeMensage = TipoDeMensage }, JsonRequestBehavior.DenyGet);

            using (DbContextTransaction dbTran = db.Database.BeginTransaction())
            {
                try
                {
                    List<VentasDetalle> bsObj = JsonConvert.DeserializeObject<List<VentasDetalle>>(valores);

                    NE_Venta nE_Venta = new NE_Venta()
                    {
                        UsuarioId = Settings.LoggedUser.UsuarioId,
                        Fecha = DateTime.Now,
                        TotalVenta = precio,
                        TipoPagoId = tipopago
                    };
                    db.NE_Venta.Add(nE_Venta);
                    db.SaveChanges();
                    NE_Carrito nE_Carrito = new NE_Carrito();
                    foreach (var i in bsObj)
                    {
                        NE_VentaDetalle nE_VentaDetalle = new NE_VentaDetalle()
                        {
                            VentaId = nE_Venta.VentaId,
                            ProductoId = i.productoid,
                            VehiculoId = i.vehiculoid,
                            Precio = Convert.ToInt32(i.total),
                            Cantidad = Convert.ToInt32(i.cantidad)
                        };

                        if (i.vehiculoid != null) {
                            NE_Vehiculo nE_vehiculo = new NE_Vehiculo();
                            nE_vehiculo = db.NE_Vehiculo.Where(x => x.VehiculoId == i.vehiculoid).First();
                            if (nE_vehiculo.Activo == true)
                            {
                                nE_vehiculo.Activo = false;
                                nE_Carrito = db.NE_Carrito.Where(x => x.VehiculoId == i.vehiculoid).First();
                                db.NE_Carrito.Remove(nE_Carrito);
                            }
                        }
                        else
                        {
                            NE_Producto nE_producto = new NE_Producto();
                            nE_producto = db.NE_Producto.Where(x => x.ProductoId == i.productoid).First();
                            if (nE_producto.Activo == true)
                            {
                                if (nE_producto.Stock == i.cantidad)
                                {
                                    nE_producto.Stock = nE_producto.Stock - (int)i.cantidad;
                                    nE_producto.Activo = false;
                                }
                                else
                                {
                                    nE_producto.Stock = nE_producto.Stock - (int)i.cantidad;
                                }
                                nE_Carrito = db.NE_Carrito.Where(x => x.ProductoId == i.productoid).First();
                                db.NE_Carrito.Remove(nE_Carrito);
                            }
                                
                        }

                        db.NE_VentaDetalle.Add(nE_VentaDetalle);
                        db.SaveChanges();
                    }

                    NE_Bitacora bitacora = new NE_Bitacora()
                    {
                        AccionId = ACCION.COMPRA,
                        Descripcion = "el usuario : " + Settings.LoggedUser.CorreoElectronico + " ha realizado una nueva compra.",
                        FechaDeRegistro = DateTime.Now,
                        UsuarioId = Settings.LoggedUser.UsuarioId,
                    };
                    db.NE_Bitacora.Add(bitacora);
                    db.SaveChanges();


                    dbTran.Commit();
                    return Json(new { Success = true, Message = valores, TipoDeMensage=1 }, JsonRequestBehavior.DenyGet);
                }
                catch (Exception e)
                {
                    return Json(new { Success = false, Message = "Primero de tiene que iniciar sesion", TipoDeMensage= TipoDeMensage }, JsonRequestBehavior.DenyGet);
                }


            }

           
        }

    
    }
}