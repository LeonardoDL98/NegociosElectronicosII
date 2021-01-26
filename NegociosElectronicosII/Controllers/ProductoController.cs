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
    [NegociosII_Auth(Roles = "1")]
    public class ProductoController : Controller
    {
        private DB_NE_Entitties db = new DB_NE_Entitties();
        

        // GET: Producto
        public ActionResult Index()
        {
            var nE_Producto = db.NE_Producto.Include(n => n.NE_Color).Include(n => n.NE_Marca);
            return View(nE_Producto.ToList());
        }

        // GET: Producto/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NE_Producto nE_Producto = db.NE_Producto.Find(id);
            if (nE_Producto == null)
            {
                return HttpNotFound();
            }
            return View(nE_Producto);
        }

        // GET: Producto/Create
        public ActionResult Create()
        {
            ViewBag.ColorId = new SelectList(db.NE_Color, "ColorId", "Color");
            ViewBag.MarcaId = new SelectList(db.NE_Marca, "MarcaId", "Marca");
            return View();
        }

        // POST: Producto/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductoId,Nombre,Descripcion,MarcaId,ColorId,Stock,PrecioVenta,PrecioCompra,Activo,MarcarComoOferta,PrecioOFerta")] NE_Producto nE_Producto)
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
                            db.NE_Producto.Add(nE_Producto);
                            db.SaveChanges();

                            var name = String.Format("Product_{0}",nE_Producto.ProductoId);
                            //var filePath = Server.MapPath("~/Imagenes/Productos/" + postedFile.FileName + extension);
                            var filePath = Server.MapPath("~/Imagenes/Productos/" + name + extension);

                            NE_ProductoImagen imagen = new NE_ProductoImagen()
                            {
                                Extension = extension,
                                Nombre = name,
                                ProductoId = nE_Producto.ProductoId,
                                Ruta = filePath,
                            };
                            db.NE_ProductoImagen.Add(imagen);
                            db.SaveChanges();

                            postedFile.SaveAs(filePath);

                            return RedirectToAction("Index");
                        }
                    }

                }

                ViewBag.ColorId = new SelectList(db.NE_Color, "ColorId", "Color", nE_Producto.ColorId);
                ViewBag.MarcaId = new SelectList(db.NE_Marca, "MarcaId", "Marca", nE_Producto.MarcaId);




                return View(nE_Producto);
            }
            catch (Exception ex){
                return View(nE_Producto);
            }
        }

        // GET: Producto/Edit/5
        public ActionResult Edit(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NE_Producto nE_Producto = db.NE_Producto.Find(id);
            if (nE_Producto == null)
            {
                return HttpNotFound();
            }
            ViewBag.ColorId = new SelectList(db.NE_Color, "ColorId", "Color", nE_Producto.ColorId);
            ViewBag.MarcaId = new SelectList(db.NE_Marca, "MarcaId", "Marca", nE_Producto.MarcaId);
            return View(nE_Producto);
        }

        // POST: Producto/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductoId,Nombre,Descripcion,MarcaId,ColorId,Stock,PrecioVenta,PrecioCompra,Activo,MarcarComoOferta,PrecioOFerta")] NE_Producto nE_Producto)
        {
            try
            {
                string mensaje;
                HttpPostedFileBase postedFile = Request.Files[0];
               

                if (ModelState.IsValid)
                {
                    NE_Producto nE_Prod = new NE_Producto();
                    nE_Prod = db.NE_Producto.Where(x => x.ProductoId == nE_Producto.ProductoId).First();
                    nE_Prod.Nombre = nE_Producto.Nombre;
                    nE_Prod.Descripcion = nE_Producto.Descripcion;
                    nE_Prod.MarcaId = nE_Producto.MarcaId;
                    nE_Prod.ColorId = nE_Producto.ColorId;
                    nE_Prod.Stock = nE_Producto.Stock;
                    nE_Prod.PrecioVenta = nE_Producto.PrecioVenta;
                    nE_Prod.PrecioCompra = nE_Producto.PrecioCompra;
                    nE_Prod.Activo = nE_Producto.Activo;
                    nE_Prod.MarcarComoOferta = nE_Producto.MarcarComoOferta;
                    nE_Prod.PrecioOFerta = nE_Producto.PrecioOFerta;
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
                           

                            NE_ProductoImagen nE_ProdImagen = new NE_ProductoImagen();
                            nE_ProdImagen = db.NE_ProductoImagen.Where(x => x.ProductoId == nE_Producto.ProductoId).First();

                            var name = String.Format("Product_{0}", nE_Producto.ProductoId); 
                            var filePath = Server.MapPath("~/Imagenes/Productos/" + name + extension);

                            nE_ProdImagen.ProductoId = nE_Producto.ProductoId;
                            nE_ProdImagen.Nombre = name;
                            nE_ProdImagen.Ruta = filePath;
                            nE_ProdImagen.Extension = ext;
                            db.SaveChanges();
                            postedFile.SaveAs(filePath);
                            return RedirectToAction("Index");
                        }
                    }

                }

                ViewBag.ColorId = new SelectList(db.NE_Color, "ColorId", "Color", nE_Producto.ColorId);
                ViewBag.MarcaId = new SelectList(db.NE_Marca, "MarcaId", "Marca", nE_Producto.MarcaId);




                return View(nE_Producto);
            }
            catch (Exception ex)
            {
                return View(nE_Producto);
            }

            //if (ModelState.IsValid)
            //{
            //    db.Entry(nE_Producto).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //ViewBag.ColorId = new SelectList(db.NE_Color, "ColorId", "Color", nE_Producto.ColorId);
            //ViewBag.MarcaId = new SelectList(db.NE_Marca, "MarcaId", "Marca", nE_Producto.MarcaId);
          //  return View(nE_Producto);
        }

        // GET: Producto/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NE_Producto nE_Producto = db.NE_Producto.Find(id);
            if (nE_Producto == null)
            {
                return HttpNotFound();
            }
            return View(nE_Producto);
        }

        // POST: Producto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NE_ProductoImagen nE_ProdImagen = new NE_ProductoImagen();
            NE_Producto nE_Producto = db.NE_Producto.Find(id);
            db.NE_Producto.Remove(nE_Producto);
            nE_ProdImagen = db.NE_ProductoImagen.Where(x => x.ProductoId == id).First();
            db.NE_ProductoImagen.Remove(nE_ProdImagen);
            db.SaveChanges();
            
            //System.IO.File.Delete(nE_ProdImagen.Ruta);
            return RedirectToAction("Index");
        }

        public ActionResult ImagenCreate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NE_Producto nE_Producto = db.NE_Producto.Find(id);
            if (nE_Producto == null)
            {
                return HttpNotFound();
            }
            return View(nE_Producto);
        }

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
                        var filePath = Server.MapPath("~/Imagenes/Productos/" + name + extension);

                        NE_ProductoImagen imagen = new NE_ProductoImagen()
                        {
                            Extension = extension,
                            Nombre = name,
                            ProductoId = id,
                            Ruta = filePath,
                        };
                        db.NE_ProductoImagen.Add(imagen);
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

        public ActionResult DeleteImages(Int32 id) {

            return View(db.NE_ProductoImagen.Where(x=>x.ProductoId==id).ToList());
        }

        public ActionResult DeleteImagesPost(Int32 id)
        {
            NE_ProductoImagen prodImagen = db.NE_ProductoImagen.Find(id);
            Int32 ID_Base = prodImagen.ProductoId;

            if (System.IO.File.Exists(Server.MapPath("~/Imagenes/Producto/" + prodImagen.Nombre + prodImagen.Extension))) 
                System.IO.File.Delete(Server.MapPath("~/Imagenes/Producto/" + prodImagen.Nombre + prodImagen.Extension));


            db.NE_ProductoImagen.Remove(prodImagen);
            db.SaveChanges();

            return RedirectToAction("DeleteImages", new { id = ID_Base });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
