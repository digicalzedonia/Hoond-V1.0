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
    
    public class RolController : Controller
    {
        private HoondDBEntities db = new HoondDBEntities();
        private Comun DatComun = new Comun();

        //
        // GET: /Rol/

        public ActionResult Index(string cmbCampo, string cmbPaginado, string sortField, string txtBusqueda, string msgError, int? page)
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User, "Rol", "Index", false);
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
            var Query = db.Rol.Include(r => r.Usuario);
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
        // GET: /Rol/Details/5

        public ActionResult Details(decimal id = 0)
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User, "Rol", "Index", false);
            if (url["Action"] != string.Empty)
                return RedirectToAction(url["Action"], url["Controller"], new { msgError = url["msgError"] });
            ViewBag.isAdmin = User.isAdmin;
            Rol rol = db.Rol.Find(id);
            if (rol == null)
            {
                return HttpNotFound();
            }
            return View(rol);
        }

        //
        // GET: /Rol/Create

        public ActionResult Create()
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User, "Rol", "Create", true);
            if (url["Action"] != string.Empty)
                return RedirectToAction(url["Action"], url["Controller"], new { msgError = url["msgError"] });
            return View();
        }

        //
        // POST: /Rol/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Rol rol)
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User);
            if (url["Action"] != string.Empty)
                return RedirectToAction(url["Action"], url["Controller"], new { msgError = url["msgError"] });
            if (ModelState.IsValid)
            {
                rol.idRol  = DatComun.GetId(1);
                rol.idUsuario_alta  = User.idUsuario;
                rol.fecha_alta = DateTime.Now;
                rol.activo = true;
                db.Rol.Add(rol);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rol);
        }

        //
        // GET: /Rol/Edit/5

        public ActionResult Edit(decimal id = 0)
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User, "Rol", "Create", true);
            if (url["Action"] != string.Empty)
                return RedirectToAction(url["Action"], url["Controller"], new { msgError = url["msgError"] });
            ViewBag.isAdmin = User.isAdmin;
            Rol rol = db.Rol.Find(id);
            if (rol == null)
            {
                return HttpNotFound();
            }
            return View(rol);
        }

        //
        // POST: /Rol/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Rol rol)
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User);
            if (url["Action"] != string.Empty)
                return RedirectToAction(url["Action"], url["Controller"], new { msgError = url["msgError"] });
            ViewBag.isAdmin = User.isAdmin;
            if (ModelState.IsValid)
            {
                DatComun.CreateLog("Rol", "U", rol.idRol, User.idUsuario);
                db.Entry(rol).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rol);
        }

        //
        // GET: /Rol/Delete/5

        public ActionResult Delete(decimal id = 0)
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User, "Rol", "Create", true);
            if (url["Action"] != string.Empty)
                return RedirectToAction(url["Action"], url["Controller"], new { msgError = url["msgError"] });
            ViewBag.isAdmin = User.isAdmin;
            Rol rol = db.Rol.Find(id);
            if (rol == null)
            {
                return HttpNotFound();
            }
            return View(rol);
        }

        //
        // POST: /Rol/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User);
            if (url["Action"] != string.Empty)
                return RedirectToAction(url["Action"], url["Controller"], new { msgError = url["msgError"] });
            Rol rol = db.Rol.Find(id);
            DatComun.CreateLog("Rol", "D", id, User.idUsuario);
            rol.activo = false;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteM(decimal[] ids)
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User, "Rol", "Create", true);
            if (url["Action"] != string.Empty)
                return RedirectToAction(url["Action"], url["Controller"], new { msgError = url["msgError"] });
            ViewBag.isAdmin = User.isAdmin;
            ViewBag.Error = string.Empty;
            if (ids != null)
            {
                var id = ids.Cast<decimal>().ToList();
                var Query = db.Rol.Include(r => r.Usuario).Where(q => id.Contains(q.idRol));
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
                Rol Item;
                for (int i = 0; i < ids.Length; i++)
                {
                    Item = db.Rol.Find(ids[i]);
                    DatComun.CreateLog("Rol", "D", ids[i], User.idUsuario);
                    Item.activo = false;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            ViewBag.isAdmin = User.isAdmin;
            ViewBag.Error = app_GlobalResources.Content.msg_ValCapcha;
            var id = ids.Cast<decimal>().ToList();
            var Query = db.Rol.Include(r => r.Usuario).Where(q => id.Contains(q.idRol));
            return View("DeleteM", Query.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}