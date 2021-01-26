using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NegociosElectronicosII.GlobalCode;
using NegociosElectronicosII.Models;

namespace NegociosElectronicosII.Controllers
{
    [NegociosII_Auth(Roles ="1")]
    public class VehiculoController : Controller
    {
        private DB_NE_Entitties db = new DB_NE_Entitties();

        // GET: Vehiculo
        public ActionResult Index()
        {
            var nE_Vehiculo = db.NE_Vehiculo.Include(n => n.NE_Categoria).Include(n => n.NE_Color).Include(n => n.NE_Marca).Include(n => n.NE_Transmision);
            return View(nE_Vehiculo.ToList());
        }

        // GET: Vehiculo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NE_Vehiculo nE_Vehiculo = db.NE_Vehiculo.Find(id);
            if (nE_Vehiculo == null)
            {
                return HttpNotFound();
            }
            return View(nE_Vehiculo);
        }

        // GET: Vehiculo/Create
        public ActionResult Create()
        {
            ViewBag.CategoriaId = new SelectList(db.NE_Categoria, "CategoriaId", "Categoria");
            ViewBag.ColorId = new SelectList(db.NE_Color, "ColorId", "Color");
            ViewBag.MarcaId = new SelectList(db.NE_Marca, "MarcaId", "Marca");
            ViewBag.TransmisionId = new SelectList(db.NE_Transmision, "TransmisionId", "Transmision");
            return View();
        }

        // POST: Vehiculo/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VehiculoId,CategoriaId,MarcaId,Modelo,TransmisionId,ColorId,PrecioVenta,PrecioCompra,Descripcion,Activo,NombreVehiculo,MarcarComoOferta,PrecioOFerta")] NE_Vehiculo nE_Vehiculo)
        {
            try
            {
                string mensaje;
                HttpPostedFileBase postedFile = Request.Files[0];

                if (ModelState.IsValid)
                {
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {
                        IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (!AllowedFileExtensions.Contains(extension))
                        {
                            mensaje = "Porfavor actualiza la imagen a estension de tipo .jpg,.gif,.png.";
                        }
                        else
                        {
                            db.NE_Vehiculo.Add(nE_Vehiculo);
                            db.SaveChanges();

                            var name = String.Format("Vehiculo_{0}", nE_Vehiculo.VehiculoId);
                            //var filePath = Server.MapPath("~/Imagenes/Productos/" + postedFile.FileName + extension);
                            var filePath = Server.MapPath("~/Imagenes/Vehiculo/" + name + extension);

                            NE_VehiculoImagen imagen = new NE_VehiculoImagen()
                            {
                                Extension = extension,
                                Nombre = name,
                                VehiculoId = nE_Vehiculo.VehiculoId,
                                Ruta = filePath,
                            };
                            db.NE_VehiculoImagen.Add(imagen);
                            db.SaveChanges();

                            postedFile.SaveAs(filePath);

                            return RedirectToAction("Index");
                        }
                    }

                }
                ViewBag.CategoriaId = new SelectList(db.NE_Categoria, "CategoriaId", "Categoria", nE_Vehiculo.CategoriaId);
                ViewBag.ColorId = new SelectList(db.NE_Color, "ColorId", "Color", nE_Vehiculo.ColorId);
                ViewBag.MarcaId = new SelectList(db.NE_Marca, "MarcaId", "Marca", nE_Vehiculo.MarcaId);
                ViewBag.TransmisionId = new SelectList(db.NE_Transmision, "TransmisionId", "Transmision", nE_Vehiculo.TransmisionId);
                return View(nE_Vehiculo);
            }
            catch (Exception ex)
            {
                return View(nE_Vehiculo);
            }
           
        }

        // GET: Vehiculo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NE_Vehiculo nE_Vehiculo = db.NE_Vehiculo.Find(id);
            if (nE_Vehiculo == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoriaId = new SelectList(db.NE_Categoria, "CategoriaId", "Categoria", nE_Vehiculo.CategoriaId);
            ViewBag.ColorId = new SelectList(db.NE_Color, "ColorId", "Color", nE_Vehiculo.ColorId);
            ViewBag.MarcaId = new SelectList(db.NE_Marca, "MarcaId", "Marca", nE_Vehiculo.MarcaId);
            ViewBag.TransmisionId = new SelectList(db.NE_Transmision, "TransmisionId", "Transmision", nE_Vehiculo.TransmisionId);
            return View(nE_Vehiculo);
        }

        // POST: Vehiculo/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VehiculoId,CategoriaId,MarcaId,Modelo,TransmisionId,ColorId,PrecioVenta,PrecioCompra,Descripcion,Activo,NombreVehiculo,MarcarComoOferta,PrecioOFerta")] NE_Vehiculo nE_Vehiculo)
        {
            try
            {
                string mensaje;
                HttpPostedFileBase postedFile = Request.Files[0];


                if (ModelState.IsValid)
                {
                    NE_Vehiculo nE_Vehic = new NE_Vehiculo();
                    nE_Vehic = db.NE_Vehiculo.Where(x => x.VehiculoId == nE_Vehiculo.VehiculoId).First();
                    nE_Vehic.NombreVehiculo = nE_Vehiculo.NombreVehiculo;
                    nE_Vehic.Modelo = nE_Vehiculo.Modelo;
                    nE_Vehic.TransmisionId = nE_Vehiculo.TransmisionId;
                    nE_Vehic.Descripcion = nE_Vehiculo.Descripcion;
                    nE_Vehic.MarcaId = nE_Vehiculo.MarcaId;
                    nE_Vehic.ColorId = nE_Vehiculo.ColorId;
                    nE_Vehic.CategoriaId = nE_Vehiculo.CategoriaId;
                    nE_Vehic.PrecioVenta = nE_Vehiculo.PrecioVenta;
                    nE_Vehic.PrecioCompra = nE_Vehiculo.PrecioCompra;
                    nE_Vehic.Activo = nE_Vehiculo.Activo;
                    nE_Vehic.MarcarComoOferta = nE_Vehiculo.MarcarComoOferta;
                    nE_Vehic.PrecioOFerta = nE_Vehiculo.PrecioOFerta;
                    db.SaveChanges();

                    if (postedFile != null && postedFile.ContentLength > 0)
                    {
                        IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (!AllowedFileExtensions.Contains(extension))
                        {
                            mensaje = "Porfavor actualiza la imagen a estension de tipo .jpg,.gif,.png.";
                        }
                        else
                        {


                            NE_VehiculoImagen nE_VehiculoImagen = new NE_VehiculoImagen();
                            nE_VehiculoImagen = db.NE_VehiculoImagen.Where(x => x.VehiculoId == nE_Vehiculo.VehiculoId).First();

                            var name = String.Format("Vehiculo_{0}", nE_Vehiculo.VehiculoId);
                            var filePath = Server.MapPath("~/Imagenes/Vehiculo/" + name + extension);

                            nE_VehiculoImagen.VehiculoId = nE_Vehiculo.VehiculoId;
                            nE_VehiculoImagen.Nombre = name;
                            nE_VehiculoImagen.Ruta = filePath;
                            nE_VehiculoImagen.Extension = ext;
                            db.SaveChanges();
                            postedFile.SaveAs(filePath);
                            return RedirectToAction("Index");
                        }
                    }

                }

                ViewBag.CategoriaId = new SelectList(db.NE_Categoria, "CategoriaId", "Categoria", nE_Vehiculo.CategoriaId);
                ViewBag.ColorId = new SelectList(db.NE_Color, "ColorId", "Color", nE_Vehiculo.ColorId);
                ViewBag.MarcaId = new SelectList(db.NE_Marca, "MarcaId", "Marca", nE_Vehiculo.MarcaId);
                ViewBag.TransmisionId = new SelectList(db.NE_Transmision, "TransmisionId", "Transmision", nE_Vehiculo.TransmisionId);
                return View(nE_Vehiculo);
            }
            catch (Exception ex)
            {
                return View(nE_Vehiculo);
            }

        }

        // GET: Vehiculo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NE_Vehiculo nE_Vehiculo = db.NE_Vehiculo.Find(id);
            if (nE_Vehiculo == null)
            {
                return HttpNotFound();
            }
            return View(nE_Vehiculo);
        }

        // POST: Vehiculo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NE_VehiculoImagen nE_VehiculoImagen = new NE_VehiculoImagen();
            NE_Vehiculo nE_Vehiculo = db.NE_Vehiculo.Find(id);
            db.NE_Vehiculo.Remove(nE_Vehiculo);
            nE_VehiculoImagen = db.NE_VehiculoImagen.Where(x => x.VehiculoId == id).First();
            db.NE_VehiculoImagen.Remove(nE_VehiculoImagen);
            db.SaveChanges();
            System.IO.File.Delete(nE_VehiculoImagen.Ruta);
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

        public ActionResult ImagenCreate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NE_Vehiculo nE_Vehiculo = db.NE_Vehiculo.Find(id);
            if (nE_Vehiculo == null)
            {
                return HttpNotFound();
            }
            return View(nE_Vehiculo);
        }

        // POST: Vehiculo/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ImagenCreate(int id)
        {
            try
            {
                string mensaje;
                HttpPostedFileBase postedFile = Request.Files[0];

                if (postedFile != null && postedFile.ContentLength > 0)
                {
                    IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                    var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                    var extension = ext.ToLower();
                    if (!AllowedFileExtensions.Contains(extension))
                    {
                        mensaje = "Porfavor actualiza la imagen a estension de tipo .jpg,.gif,.png.";
                    }
                    else
                    {
                        var name = String.Format("Vehiculo_{0:ddMMyyyyHHmmss}", DateTime.Now);
                        //var filePath = Server.MapPath("~/Imagenes/Productos/" + postedFile.FileName + extension);
                        var filePath = Server.MapPath("~/Imagenes/Vehiculo/" + name + extension);

                        NE_VehiculoImagen imagen = new NE_VehiculoImagen()
                        {
                            Extension = extension,
                            Nombre = name,
                            VehiculoId = id,
                            Ruta = filePath,
                        };
                        db.NE_VehiculoImagen.Add(imagen);
                        db.SaveChanges();
                                
                                
                        postedFile.SaveAs(filePath);

                        return RedirectToAction("Index");
                    }
                }

                return View();
            }
            catch (Exception ex)
            {
                return View();
            }

        }

        public ActionResult DeleteImages(Int32 id)
        {

            return View(db.NE_VehiculoImagen.Where(x => x.VehiculoId == id).ToList());
        }

        public ActionResult DeleteImagesPost(Int32 id)
        {
            NE_VehiculoImagen prodImagen = db.NE_VehiculoImagen.Find(id);
            Int32 ID_Base = prodImagen.VehiculoId;

            if (System.IO.File.Exists(Server.MapPath("~/Imagenes/Producto/" + prodImagen.Nombre + prodImagen.Extension)))
                System.IO.File.Delete(Server.MapPath("~/Imagenes/Producto/" + prodImagen.Nombre + prodImagen.Extension));


            db.NE_VehiculoImagen.Remove(prodImagen);
            db.SaveChanges();

            return RedirectToAction("DeleteImages", new { id = ID_Base });
        }


    }
}
