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
    public class FAQSController : Controller
    {
        private DB_NE_Entitties db = new DB_NE_Entitties();

        // GET: FAQS
        public ActionResult Index()
        {
            return View(db.NE_FAQS.ToList());
        }

        // GET: FAQS/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NE_FAQS nE_FAQS = db.NE_FAQS.Find(id);
            if (nE_FAQS == null)
            {
                return HttpNotFound();
            }
            return View(nE_FAQS);
        }

        // GET: FAQS/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FAQS/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FAQId,Pregunta,Respuesta,Orden,Activo")] NE_FAQS nE_FAQS)
        {
            if (ModelState.IsValid)
            {
                db.NE_FAQS.Add(nE_FAQS);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nE_FAQS);
        }

        // GET: FAQS/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NE_FAQS nE_FAQS = db.NE_FAQS.Find(id);
            if (nE_FAQS == null)
            {
                return HttpNotFound();
            }
            return View(nE_FAQS);
        }

        // POST: FAQS/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FAQId,Pregunta,Respuesta,Orden,Activo")] NE_FAQS nE_FAQS)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nE_FAQS).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nE_FAQS);
        }

        // GET: FAQS/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NE_FAQS nE_FAQS = db.NE_FAQS.Find(id);
            if (nE_FAQS == null)
            {
                return HttpNotFound();
            }
            return View(nE_FAQS);
        }

        // POST: FAQS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NE_FAQS nE_FAQS = db.NE_FAQS.Find(id);
            db.NE_FAQS.Remove(nE_FAQS);
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
