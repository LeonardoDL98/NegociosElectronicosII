using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NegociosElectronicosII.Models;
using NegociosElectronicosII.GlobalCode;

namespace NegociosElectronicosII.Controllers
{
    [NegociosII_Auth(Roles = "1")]
    public class BitacoraController : Controller
    {
        private DB_NE_Entitties db = new DB_NE_Entitties();

        // GET: Bitacora
        public ActionResult Index()
        {
            var nE_Bitacora = db.NE_Bitacora.Include(n => n.NE_Accion).Include(n => n.NE_Usuario);
            return View(nE_Bitacora.ToList());
        }

        // GET: Bitacora/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NE_Bitacora nE_Bitacora = db.NE_Bitacora.Find(id);
            if (nE_Bitacora == null)
            {
                return HttpNotFound();
            }
            return View(nE_Bitacora);
        }

        // GET: Bitacora/Create
        public ActionResult Create()
        {
            ViewBag.AccionId = new SelectList(db.NE_Accion, "AccionId", "Accion");
            ViewBag.UsuarioId = new SelectList(db.NE_Usuario, "UsuarioId", "Nombre");
            return View();
        }

        // POST: Bitacora/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BitacoraId,AccionId,Descripcion,FechaDeRegistro,UsuarioId")] NE_Bitacora nE_Bitacora)
        {
            if (ModelState.IsValid)
            {
                db.NE_Bitacora.Add(nE_Bitacora);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AccionId = new SelectList(db.NE_Accion, "AccionId", "Accion", nE_Bitacora.AccionId);
            ViewBag.UsuarioId = new SelectList(db.NE_Usuario, "UsuarioId", "Nombre", nE_Bitacora.UsuarioId);
            return View(nE_Bitacora);
        }

        // GET: Bitacora/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NE_Bitacora nE_Bitacora = db.NE_Bitacora.Find(id);
            if (nE_Bitacora == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccionId = new SelectList(db.NE_Accion, "AccionId", "Accion", nE_Bitacora.AccionId);
            ViewBag.UsuarioId = new SelectList(db.NE_Usuario, "UsuarioId", "Nombre", nE_Bitacora.UsuarioId);
            return View(nE_Bitacora);
        }

        // POST: Bitacora/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BitacoraId,AccionId,Descripcion,FechaDeRegistro,UsuarioId")] NE_Bitacora nE_Bitacora)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nE_Bitacora).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccionId = new SelectList(db.NE_Accion, "AccionId", "Accion", nE_Bitacora.AccionId);
            ViewBag.UsuarioId = new SelectList(db.NE_Usuario, "UsuarioId", "Nombre", nE_Bitacora.UsuarioId);
            return View(nE_Bitacora);
        }

        // GET: Bitacora/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NE_Bitacora nE_Bitacora = db.NE_Bitacora.Find(id);
            if (nE_Bitacora == null)
            {
                return HttpNotFound();
            }
            return View(nE_Bitacora);
        }

        // POST: Bitacora/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NE_Bitacora nE_Bitacora = db.NE_Bitacora.Find(id);
            db.NE_Bitacora.Remove(nE_Bitacora);
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
