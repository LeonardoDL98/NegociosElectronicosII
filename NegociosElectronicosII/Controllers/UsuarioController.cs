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
    public class UsuarioController : Controller
    {
        private DB_NE_Entitties db = new DB_NE_Entitties();

        // GET: Usuario
        public ActionResult Index()
        {
            ViewBag.Message = String.Empty;
            var nE_Usuario = db.NE_Usuario.Include(n => n.NE_Rol).Include(n => n.NE_Sexo);
            return View(nE_Usuario.ToList());
        }

        // GET: Usuario/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NE_Usuario nE_Usuario = db.NE_Usuario.Find(id);
            if (nE_Usuario == null)
            {
                return HttpNotFound();
            }
            return View(nE_Usuario);
        }

        // GET: Usuario/Create
        public ActionResult Create()
        {
            ViewBag.RolId = new SelectList(db.NE_Rol, "RolId", "Rol");
            ViewBag.SexoId = new SelectList(db.NE_Sexo, "SexoId", "Sexo");
            return View();
        }

        // POST: Usuario/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UsuarioId,Nombre,ApellidoPaterno,ApellidoMaterno,SexoId,Edad,Direccion,Telefono,CorreoElectronico,Activo,RolId,Password")] NE_Usuario nE_Usuario)
        {
            ViewBag.Message = String.Empty;
            if (ModelState.IsValid)
            {


                using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    NE_Autenticacion userAuth = new NE_Autenticacion();

                    try
                    {
                        if (!db.NE_Usuario.Any(x => x.CorreoElectronico.ToUpper() == nE_Usuario.CorreoElectronico.ToUpper()))
                        {

                            db.NE_Usuario.Add(nE_Usuario);
                            db.SaveChanges();
                            userAuth = new NE_Autenticacion()
                            {
                                UsuarioId = nE_Usuario.UsuarioId,
                                Intentos = 0,
                                CuentaBloqueada = false,
                                Contrasena = Security.Security.Encrypt(nE_Usuario.password),
                                UltimoInicioSesion = DateTime.Now,
                            };

                            db.NE_Autenticacion.Add(userAuth);
                            db.SaveChanges();
                            dbTran.Commit();

                            ViewBag.Message = "Cuenta creada";
                            return RedirectToAction("Index", "Usuario");
                        }
                        else
                        {
                            ViewBag.Message = "Ya hay un correo registrado";
                        }

                    }
                    catch (Exception e)
                    {
                        ViewBag.Message = "La cuenta no se pudo crear";
                        return RedirectToAction("Create", "Usuario");
                    }


                }

            }

            ViewBag.RolId = new SelectList(db.NE_Rol, "RolId", "Rol", nE_Usuario.RolId);
            ViewBag.SexoId = new SelectList(db.NE_Sexo, "SexoId", "Sexo", nE_Usuario.SexoId);
            return View(nE_Usuario);
          
        }

        // GET: Usuario/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NE_Usuario nE_Usuario = db.NE_Usuario.Find(id);
            if (nE_Usuario == null)
            {
                return HttpNotFound();
            }
            ViewBag.RolId = new SelectList(db.NE_Rol, "RolId", "Rol", nE_Usuario.RolId);
            ViewBag.SexoId = new SelectList(db.NE_Sexo, "SexoId", "Sexo", nE_Usuario.SexoId);
            NE_Autenticacion nE_auth = db.NE_Autenticacion.Where(x => x.UsuarioId == id).First();
            ViewBag.pass = nE_auth.Contrasena;
            return View(nE_Usuario);
        }

        // POST: Usuario/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UsuarioId,Nombre,ApellidoPaterno,ApellidoMaterno,SexoId,Edad,Direccion,Telefono,CorreoElectronico,Activo,RolId,Password")] NE_Usuario nE_Usuario)
        {
            ViewBag.Message = String.Empty;
            if (ModelState.IsValid)
            {
                using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    NE_Autenticacion userAuth = new NE_Autenticacion();
                    try
                    {
                        

                            NE_Usuario user = new NE_Usuario();
                            user = db.NE_Usuario.Where(x => x.UsuarioId == nE_Usuario.UsuarioId).First();
                            user.Nombre = nE_Usuario.Nombre;
                            user.ApellidoPaterno = nE_Usuario.ApellidoPaterno;
                            user.ApellidoMaterno = nE_Usuario.ApellidoMaterno;
                            user.SexoId = nE_Usuario.SexoId;
                            user.Edad = nE_Usuario.Edad;
                            user.Direccion = nE_Usuario.Direccion;
                            user.Telefono = nE_Usuario.Telefono;
                            user.CorreoElectronico = nE_Usuario.CorreoElectronico;
                            user.Activo = nE_Usuario.Activo;
                            user.RolId = nE_Usuario.RolId;
                            db.SaveChanges();
                            if (nE_Usuario.password != null)
                            {
                                userAuth = db.NE_Autenticacion.Where(x => x.UsuarioId == nE_Usuario.UsuarioId).First();
                                userAuth.Contrasena = Security.Security.Encrypt(nE_Usuario.password);
                                db.SaveChanges();
                            }
                            dbTran.Commit();

                            ViewBag.Message = "Cuenta editada Correctamente";
                            return RedirectToAction("Index", "Usuario");

                      
                    }
                    catch (Exception e)
                    {
                        ViewBag.Message = "La cuenta no se pudo crear";
                        return RedirectToAction("Create", "Usuario");
                    }


                }

            }

            ViewBag.RolId = new SelectList(db.NE_Rol, "RolId", "Rol", nE_Usuario.RolId);
            ViewBag.SexoId = new SelectList(db.NE_Sexo, "SexoId", "Sexo", nE_Usuario.SexoId);
            return View(nE_Usuario);
        }

        // GET: Usuario/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NE_Usuario nE_Usuario = db.NE_Usuario.Find(id);
            if (nE_Usuario == null)
            {
                return HttpNotFound();
            }
            return View(nE_Usuario);
        }

        // POST: Usuario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NE_Usuario nE_Usuario = db.NE_Usuario.Find(id);
            NE_Autenticacion nE_Autenticacion = new NE_Autenticacion();
            nE_Autenticacion = db.NE_Autenticacion.Where(x => x.UsuarioId == id).First();
            db.NE_Usuario.Remove(nE_Usuario);
            db.NE_Autenticacion.Remove(nE_Autenticacion);
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
