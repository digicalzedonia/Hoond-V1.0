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
    
    public class MenuController : Controller
    {
        private HoondDBEntities db = new HoondDBEntities();
        private Comun DatComun = new Comun();

        //
        // GET: /Menu/

        public ActionResult Index(string cmbCampo, string cmbPaginado, string sortField, string txtBusqueda, string msgError, int? page)
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User, "Menu", "Index", false);
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
            var Query = db.Menu.Include(m => m.Icono).Include(m => m.Menu2).Include(m => m.Usuario).Include(m => m.Vista);
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
        // GET: /Menu/Details/5

        public ActionResult Details(decimal id = 0)
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User, "Menu", "Index", false);
            if (url["Action"] != string.Empty)
                return RedirectToAction(url["Action"], url["Controller"], new { msgError = url["msgError"] });
            ViewBag.isAdmin = User.isAdmin;
            Menu menu = db.Menu.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        //
        // GET: /Menu/Create

        public ActionResult Create()
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User, "Menu", "Create", true);
            if (url["Action"] != string.Empty)
                return RedirectToAction(url["Action"], url["Controller"], new { msgError = url["msgError"] });
            ViewBag.idIcono = new SelectList(db.Icono.Where(e => e.activo == true), "idIcono", "descIcono");
            ViewBag.idMenu_padre = new SelectList(db.Menu.Where(e => e.activo == true), "idMenu", "descMenu");
            ViewBag.idVista = new SelectList(db.Vista.Where(e => e.activo == true), "idVista", "descVista");
            return View();
        }

        //
        // POST: /Menu/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Menu menu)
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User);
            if (url["Action"] != string.Empty)
                return RedirectToAction(url["Action"], url["Controller"], new { msgError = url["msgError"] });
            if (ModelState.IsValid)
            {
                menu.idMenu  = DatComun.GetId(1);
                menu.idUsuario_alta  = User.idUsuario;
                menu.fecha_alta = DateTime.Now;
                menu.activo = true;
                db.Menu.Add(menu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idIcono = new SelectList(db.Icono.Where(e => e.activo == true), "idIcono", "descIcono", menu.idIcono);
            ViewBag.idMenu_padre = new SelectList(db.Menu.Where(e => e.activo == true), "idMenu", "descMenu", menu.idMenu_padre);
            ViewBag.idVista = new SelectList(db.Vista.Where(e => e.activo == true), "idVista", "descVista", menu.idVista);
            return View(menu);
        }

        //
        // GET: /Menu/Edit/5

        public ActionResult Edit(decimal id = 0)
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User, "Menu", "Create", true);
            if (url["Action"] != string.Empty)
                return RedirectToAction(url["Action"], url["Controller"], new { msgError = url["msgError"] });
            ViewBag.isAdmin = User.isAdmin;
            Menu menu = db.Menu.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            ViewBag.idIcono = new SelectList(db.Icono.Where(e => e.activo == true), "idIcono", "descIcono", menu.idIcono);
            ViewBag.idMenu_padre = new SelectList(db.Menu.Where(e => e.activo == true), "idMenu", "descMenu", menu.idMenu_padre);
            ViewBag.idVista = new SelectList(db.Vista.Where(e => e.activo == true), "idVista", "descVista", menu.idVista);
            return View(menu);
        }

        //
        // POST: /Menu/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Menu menu)
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User);
            if (url["Action"] != string.Empty)
                return RedirectToAction(url["Action"], url["Controller"], new { msgError = url["msgError"] });
            ViewBag.isAdmin = User.isAdmin;
            if (ModelState.IsValid)
            {
                DatComun.CreateLog("Menu", "U", menu.idMenu, User.idUsuario);
                db.Entry(menu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idIcono = new SelectList(db.Icono.Where(e => e.activo == true), "idIcono", "descIcono", menu.idIcono);
            ViewBag.idMenu_padre = new SelectList(db.Menu.Where(e => e.activo == true), "idMenu", "descMenu", menu.idMenu_padre);
            ViewBag.idVista = new SelectList(db.Vista.Where(e => e.activo == true), "idVista", "descVista", menu.idVista);
            return View(menu);
        }

        //
        // GET: /Menu/Delete/5

        public ActionResult Delete(decimal id = 0)
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User, "Menu", "Create", true);
            if (url["Action"] != string.Empty)
                return RedirectToAction(url["Action"], url["Controller"], new { msgError = url["msgError"] });
            ViewBag.isAdmin = User.isAdmin;
            Menu menu = db.Menu.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        //
        // POST: /Menu/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User);
            if (url["Action"] != string.Empty)
                return RedirectToAction(url["Action"], url["Controller"], new { msgError = url["msgError"] });
            Menu menu = db.Menu.Find(id);
            DatComun.CreateLog("Menu", "D", id, User.idUsuario);
            menu.activo = false;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteM(decimal[] ids)
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User, "Menu", "Create", true);
            if (url["Action"] != string.Empty)
                return RedirectToAction(url["Action"], url["Controller"], new { msgError = url["msgError"] });
            ViewBag.isAdmin = User.isAdmin;
            ViewBag.Error = string.Empty;
            if (ids != null)
            {
                var id = ids.Cast<decimal>().ToList();
                var Query = db.Menu.Include(m => m.Icono).Include(m => m.Menu2).Include(m => m.Usuario).Include(m => m.Vista).Where(q => id.Contains(q.idMenu));
                return View(Query.ToList());
            }
            else
                return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteConfirmedM(decimal[] ids)
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User);
            if (url["Action"] != string.Empty)
                return RedirectToAction(url["Action"], url["Controller"], new { msgError = url["msgError"] });
            if (this.IsCaptchaValid(string.Empty))
            {
                Menu Item;
                for (int i = 0; i < ids.Length; i++)
                {
                    Item = db.Menu.Find(ids[i]);
                    DatComun.CreateLog("Menu", "D", ids[i], User.idUsuario);
                    Item.activo = false;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            ViewBag.isAdmin = User.isAdmin;
            ViewBag.Error = app_GlobalResources.Content.msg_ValCapcha;
            var id = ids.Cast<decimal>().ToList();
            var Query = db.Menu.Include(m => m.Icono).Include(m => m.Menu2).Include(m => m.Usuario).Include(m => m.Vista).Where(q => id.Contains(q.idMenu));
            return View("DeleteM", Query.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}