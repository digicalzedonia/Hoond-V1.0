using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Linq.Dynamic;

using CaptchaMvc.HtmlHelpers;
using PagedList;

using Hoond.Models;

namespace Hoond.Controllers
{
    [Authorize]
    
    public class RolMenuController : Controller
    {
        private HoondDBEntities db = new HoondDBEntities();
        private Comun DatComun = new Comun();

        //
        // GET: /RolMenu/

        public ActionResult Index(string cmbCampo, string cmbPaginado, string sortField, string txtBusqueda, string msgError, int? page)
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User, "RolMenu", "Index", false);
            if (url["Action"] != string.Empty)
                return RedirectToAction(url["Action"], url["Controller"], new { msgError = url["msgError"] });
            ViewBag.msgError = msgError;
            ViewBag.isAdmin = User.isAdmin;
            ViewBag.CurrentPage = cmbPaginado;
            ViewBag.Id = string.Empty;
            ViewBag.sortField = sortField;
            if (sortField != null)
            {
                User.SortField = User.SortField == sortField ? sortField + "|desc" : sortField;
                ViewBag.Id = User.SortField == sortField ? "sortDown" : "sortUp";
            }
            string[] field = String.IsNullOrEmpty(User.SortField) ? string.Empty.Split('|') : User.SortField.Split('|');
            ViewBag.CurrentField = cmbCampo;
            ViewBag.CurrentFilter = txtBusqueda;       
            var Query = db.RolMenu.Include(r => r.Menu).Include(r => r.Rol).Include(r => r.TipoPermiso).Include(r => r.Usuario);
			if(!User.isAdmin)
				Query = Query.Where(q => q.activo == true);
            if (!String.IsNullOrEmpty(txtBusqueda) && !String.IsNullOrEmpty(cmbCampo))
            {
                if (cmbCampo != "0")
                    Query = Query.Where(cmbCampo + ".Contains(@0)", txtBusqueda);
            }
            if (!String.IsNullOrEmpty(sortField))
            {
                if (field.Length == 1)
                    Query = Query.OrderBy(field[0]);
                else
                    Query = Query.OrderBy(field[0] + " descending");
            }
            else
                Query = Query.OrderBy(q => q.fecha_alta);
            int pageNumber = (page ?? 1);
            int pageSize = DatComun.DefaultPage();
            if (cmbPaginado != null)
            {
                if (cmbPaginado != "0")
                    pageSize = Convert.ToInt32(cmbPaginado);
                else
                {
                    pageSize = Query.ToList().Count;
                    if (pageSize == 0)
                        pageSize = DatComun.DefaultPage();
                }
            }
            Session["user"] = User;
            return View(Query.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /RolMenu/Details/5

        public ActionResult Details(decimal id = 0, decimal id2 = 0)
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User, "RolMenu", "Index", false);
            if (url["Action"] != string.Empty)
                return RedirectToAction(url["Action"], url["Controller"], new { msgError = url["msgError"] });
            ViewBag.isAdmin = User.isAdmin;
            RolMenu rolmenu = db.RolMenu.Where(q => q.idRol == id && q.idMenu == id2).First();
            if (rolmenu == null)
            {
                return HttpNotFound();
            }
            return View(rolmenu);
        }

        //
        // GET: /RolMenu/Create

        public ActionResult Create()
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User, "RolMenu", "Create", true);
            if (url["Action"] != string.Empty)
                return RedirectToAction(url["Action"], url["Controller"], new { msgError = url["msgError"] });
            ViewBag.idMenu = new SelectList(db.Menu.Where(e => e.activo == true), "idMenu", "descMenu");
            ViewBag.idRol = new SelectList(db.Rol.Where(e => e.activo == true), "idRol", "descRol");
            ViewBag.idTipoPermiso = new SelectList(db.TipoPermiso.Where(e => e.activo == true), "idTipoPermiso", "descTipoPermiso");
            return View();
        }

        //
        // POST: /RolMenu/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RolMenu rolmenu)
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User);
            if (url["Action"] != string.Empty)
                return RedirectToAction(url["Action"], url["Controller"], new { msgError = url["msgError"] });
            if (ModelState.IsValid)
            {
                rolmenu.idUsuario_alta  = User.idUsuario;
                rolmenu.fecha_alta = DateTime.Now;
                rolmenu.activo = true;
                db.RolMenu.Add(rolmenu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idMenu = new SelectList(db.Menu.Where(e => e.activo == true), "idMenu", "descMenu", rolmenu.idMenu);
            ViewBag.idRol = new SelectList(db.Rol.Where(e => e.activo == true), "idRol", "descRol", rolmenu.idRol);
            ViewBag.idTipoPermiso = new SelectList(db.TipoPermiso.Where(e => e.activo == true), "idTipoPermiso", "descTipoPermiso", rolmenu.idTipoPermiso);
            return View(rolmenu);
        }

        //
        // GET: /RolMenu/Edit/5

        public ActionResult Edit(decimal id = 0, decimal id2 = 0)
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User, "RolMenu", "Create", true);
            if (url["Action"] != string.Empty)
                return RedirectToAction(url["Action"], url["Controller"], new { msgError = url["msgError"] });
            ViewBag.isAdmin = User.isAdmin;
            RolMenu rolmenu = db.RolMenu.Where(q => q.idRol == id && q.idMenu == id2).First();
            if (rolmenu == null)
            {
                return HttpNotFound();
            }
            ViewBag.idMenu = new SelectList(db.Menu.Where(e => e.activo == true), "idMenu", "descMenu", rolmenu.idMenu);
            ViewBag.idRol = new SelectList(db.Rol.Where(e => e.activo == true), "idRol", "descRol", rolmenu.idRol);
            ViewBag.idTipoPermiso = new SelectList(db.TipoPermiso.Where(e => e.activo == true), "idTipoPermiso", "descTipoPermiso", rolmenu.idTipoPermiso);
            return View(rolmenu);
        }

        //
        // POST: /RolMenu/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RolMenu rolmenu)
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User);
            if (url["Action"] != string.Empty)
                return RedirectToAction(url["Action"], url["Controller"], new { msgError = url["msgError"] });
            ViewBag.isAdmin = User.isAdmin;
            if (ModelState.IsValid)
            {
                db.Entry(rolmenu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idMenu = new SelectList(db.Menu.Where(e => e.activo == true), "idMenu", "descMenu", rolmenu.idMenu);
            ViewBag.idRol = new SelectList(db.Rol.Where(e => e.activo == true), "idRol", "descRol", rolmenu.idRol);
            ViewBag.idTipoPermiso = new SelectList(db.TipoPermiso.Where(e => e.activo == true), "idTipoPermiso", "descTipoPermiso", rolmenu.idTipoPermiso);
            return View(rolmenu);
        }

        //
        // GET: /RolMenu/Delete/5

        public ActionResult Delete(decimal id = 0, decimal id2 = 0)
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User, "RolMenu", "Create", true);
            if (url["Action"] != string.Empty)
                return RedirectToAction(url["Action"], url["Controller"], new { msgError = url["msgError"] });
            ViewBag.isAdmin = User.isAdmin;
            RolMenu rolmenu = db.RolMenu.Where(q => q.idRol == id && q.idMenu == id2).First();
            if (rolmenu == null)
            {
                return HttpNotFound();
            }
            return View(rolmenu);
        }

        //
        // POST: /RolMenu/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id, decimal id2 = 0)
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User);
            if (url["Action"] != string.Empty)
                return RedirectToAction(url["Action"], url["Controller"], new { msgError = url["msgError"] });
            RolMenu rolmenu = db.RolMenu.Where(q => q.idRol == id && q.idMenu == id2).First();
            rolmenu.activo = false;
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