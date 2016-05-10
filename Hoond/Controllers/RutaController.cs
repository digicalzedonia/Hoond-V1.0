using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hoond.Models;


namespace Hoond.Controllers
{
    public class RutaController : Controller
    {
        private HoondDBEntities db = new HoondDBEntities();
        private Comun DatComun = new Comun();

        //
        // GET: /Ruta/

        public ActionResult Index()
        {
            var ruta = db.Ruta.Include(r => r.Usuario).Where(e=>e.activo==true);
            return View(ruta.ToList());
        }

        //
        // GET: /Ruta/Details/5

        public ActionResult Details(decimal id = 0)
        {
            Ruta ruta = db.Ruta.Find(id);
            if (ruta == null)
            {
                return HttpNotFound();
            }
            return View(ruta);
        }

        //
        // GET: /Ruta/Create

        public ActionResult Create()
        {
            ViewBag.idUsuario_alta = new SelectList(db.Usuario, "idUsuario", "nombreUsuario");
            return View();
        }

        //
        // POST: /Ruta/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Ruta ruta)
        {
            UserLogin User = (UserLogin)Session["user"];

            if (ModelState.IsValid)
            {
                //Valida si existe la ruta

                if (System.IO.Directory.Exists(ruta.direccion) == false)
                {
                    ViewBag.Error = app_GlobalResources.Content.errorRutaNoExistente ;
                    return View("Create");
                }


                newID idRuta = new newID();

                ruta.idRuta = idRuta.CalculaId(1);
                ruta.idUsuario_alta = User.idUsuario;
                ruta.activo = true;
                ruta.fecha_alta = DateTime.Now;
                db.Ruta.Add(ruta);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idUsuario_alta = new SelectList(db.Usuario, "idUsuario", "nombreUsuario", ruta.idUsuario_alta);
            return View(ruta);
        }

        //
        // GET: /Ruta/Edit/5

        public ActionResult Edit(decimal id = 0)
        {
            Ruta ruta = db.Ruta.Find(id);
            if (ruta == null)
            {
                return HttpNotFound();
            }
            ViewBag.idUsuario_alta = new SelectList(db.Usuario, "idUsuario", "nombreUsuario", ruta.idUsuario_alta);
            return View(ruta);
        }

        //
        // POST: /Ruta/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Ruta ruta)
        {
            if (ModelState.IsValid)
            {
                if (System.IO.Directory.Exists(ruta.direccion) == false)
                {
                    ViewBag.Error = app_GlobalResources.Content.errorRutaNoExistente;
                    return View("Edit");
                }

                db.Entry(ruta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idUsuario_alta = new SelectList(db.Usuario, "idUsuario", "nombreUsuario", ruta.idUsuario_alta);
            return View(ruta);
        }

        //
        // GET: /Ruta/Delete/5

        public ActionResult Delete(decimal id = 0)
        {
            Ruta ruta = db.Ruta.Find(id);
            if (ruta == null)
            {
                return HttpNotFound();
            }
            return View(ruta);
        }

        //
        // POST: /Ruta/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            //Ruta ruta = db.Ruta.Find(id);
            //db.Ruta.Remove(ruta);
            Ruta Ruta = db.Ruta.Find(id);
            Ruta.activo = false;
            //db.path.Remove(path);
            db.Entry(Ruta).State = EntityState.Modified;

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}