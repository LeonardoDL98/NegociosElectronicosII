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
    public class CarrusellController : Controller
    {
        private DB_NE_Entitties db = new DB_NE_Entitties();

        // GET: Carrusell
        public ActionResult Index()
        {
            return View(db.NE_Carrusel.ToList());
        }

        // GET: Carrusell/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NE_Carrusel nE_Carrusel = db.NE_Carrusel.Find(id);
            if (nE_Carrusel == null)
            {
                return HttpNotFound();
            }
            return View(nE_Carrusel);
        }

        // GET: Carrusell/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Carrusell/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CarruselId,Posicion,Texto,Descripcion,Ruta,Extension,NombreArchivo,Activo")] NE_Carrusel nE_Carrusel)
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
                            var name = String.Format("banner_{0}", nE_Carrusel.CarruselId);
                            var filePath = Server.MapPath("~/Imagenes/Carrusel/" + name + extension);

                            NE_Carrusel imagen = new NE_Carrusel()
                            {
                                NombreArchivo = name,
                                Extension = ext,
                                Ruta=filePath,
                                Posicion=nE_Carrusel.Posicion,
                                Texto= nE_Carrusel.Texto,
                                Descripcion= nE_Carrusel.Descripcion,
                                Activo= nE_Carrusel.Activo,
                            };
                            db.NE_Carrusel.Add(imagen);
                            db.SaveChanges();

                            NE_Carrusel updateModel = db.NE_Carrusel.Find(imagen.CarruselId);
                            updateModel.NombreArchivo= String.Format("banner_{0}", imagen.CarruselId);
                            db.SaveChanges();
                            filePath = Server.MapPath("~/Imagenes/Carrusel/" + updateModel.NombreArchivo+updateModel.Extension);
                            postedFile.SaveAs(filePath);

                            return RedirectToAction("Index");
                        }
                    }

                }
                return View(nE_Carrusel);
            }
            catch (Exception ex)
            {
                return View(nE_Carrusel);
            }
          
        }

        // GET: Carrusell/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NE_Carrusel nE_Carrusel = db.NE_Carrusel.Find(id);
            if (nE_Carrusel == null)
            {
                return HttpNotFound();
            }
            return View(nE_Carrusel);
        }

        // POST: Carrusell/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CarruselId,Posicion,Texto,Descripcion,Ruta,Extension,NombreArchivo,Activo")] NE_Carrusel nE_Carrusel)
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


                            var name = String.Format("banner_{0}", nE_Carrusel.CarruselId);
                            var filePath = Server.MapPath("~/Imagenes/Carrusel/" + name + extension);

                            NE_Carrusel nE_Carru = new NE_Carrusel();
                            nE_Carru = db.NE_Carrusel.Where(x => x.CarruselId == nE_Carrusel.CarruselId).First();
                            nE_Carru.NombreArchivo = name;
                            nE_Carru.Extension = ext;
                            nE_Carru.Ruta = filePath;
                            nE_Carru.Posicion = nE_Carrusel.Posicion;
                            nE_Carru.Texto = nE_Carrusel.Texto;
                            nE_Carru.Descripcion = nE_Carrusel.Descripcion;
                            nE_Carru.Activo = nE_Carrusel.Activo;
                            db.SaveChanges();

                            postedFile.SaveAs(filePath);

                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        NE_Carrusel nE_Carru = new NE_Carrusel();
                        nE_Carru.Posicion = nE_Carrusel.Posicion;
                        nE_Carru.Texto = nE_Carrusel.Texto;
                        nE_Carru.Descripcion = nE_Carrusel.Descripcion;
                        nE_Carru.Activo = nE_Carrusel.Activo;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }

                }

                return View(nE_Carrusel);
            }
            catch (Exception ex)
            {
                return View(nE_Carrusel);
            }
        }

        // GET: Carrusell/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NE_Carrusel nE_Carrusel = db.NE_Carrusel.Find(id);
            if (nE_Carrusel == null)
            {
                return HttpNotFound();
            }
            return View(nE_Carrusel);
        }

        // POST: Carrusell/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NE_Carrusel nE_Carrusel = db.NE_Carrusel.Find(id);
            db.NE_Carrusel.Remove(nE_Carrusel);
            System.IO.File.Delete(nE_Carrusel.Ruta);
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
    }
}
