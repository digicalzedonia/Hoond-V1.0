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
    
    public class TipoUsuarioSucursalController : Controller
    {
        private HoondDBEntities db = new HoondDBEntities();
        private Comun DatComun = new Comun();

        //
        // GET: /TipoUsuarioSucursal/

        public ActionResult Index(string cmbCampo, string cmbPaginado, string sortField, string txtBusqueda, string msgError, int? page)
        {
            if (Session["user"] == null)
                return RedirectToAction("Login", "Login");
            UserLogin User = (UserLogin)Session["user"];
            if (User.idEmpresa == 0)
                return RedirectToAction("Index", "Config");
            int Per = DatComun.TipoPer(User.idRol, "TipoUsuarioSucursal", "Index");
            if (Per == 0)
                return RedirectToAction("Index", "Home", new { msgError = app_GlobalResources.Content.msg_AccesoDenegado });
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
            var Query = db.TipoUsuarioSucursal.Include(t => t.Usuario);
			if(!User.isAdmin)
				Query = Query.Where(q => q.activo == true);
          //  Query = Query.Where(q => q.idEmpresa == User.idEmpresa);
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
        // GET: /TipoUsuarioSucursal/Details/5

        public ActionResult Details(decimal id = 0)
        {
            if (Session["user"] == null)
                return RedirectToAction("Login", "Login");
            UserLogin User = (UserLogin)Session["user"];
            int Per = DatComun.TipoPer(User.idRol, "TipoUsuarioSucursal", "Index");
            if (Per == 0)
                return RedirectToAction("Index", new { msgError = app_GlobalResources.Content.msg_AccesoDenegado });
            ViewBag.isAdmin = User.isAdmin;
            TipoUsuarioSucursal tipousuariosucursal = db.TipoUsuarioSucursal.Find(id);
            if (tipousuariosucursal == null)
            {
                return HttpNotFound();
            }
            return View(tipousuariosucursal);
        }

        //
        // GET: /TipoUsuarioSucursal/Create

        public ActionResult Create()
        {
           if (Session["user"] == null)
                return RedirectToAction("Login", "Login");
            UserLogin User = (UserLogin)Session["user"];
            int Per = DatComun.TipoPer(User.idRol, "TipoUsuarioSucursal", "Create");
            if (Per == 0)
                return RedirectToAction("Index", new { msgError = app_GlobalResources.Content.msg_AccesoDenegado });
            else if (Per == 1)
                return RedirectToAction("Index", new { msgError = app_GlobalResources.Content.msg_SinPrivilegio });
            return View();
        }

        //
        // POST: /TipoUsuarioSucursal/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TipoUsuarioSucursal tipousuariosucursal)
        {
            if (Session["user"] == null)
                return RedirectToAction("Login", "Login");
            UserLogin User = (UserLogin)Session["user"];
            if (User.idEmpresa == 0)
                return RedirectToAction("Index", "Config");
            if (ModelState.IsValid)
            {
                tipousuariosucursal.idTipoUsuarioSucursal  = DatComun.GetId(1);
                tipousuariosucursal.idUsuario_alta  = User.idUsuario;
              //  tipousuariosucursal.idEmpresa = User.idEmpresa;
                tipousuariosucursal.fecha_alta = DateTime.Now;
                tipousuariosucursal.activo = true;
                db.TipoUsuarioSucursal.Add(tipousuariosucursal);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipousuariosucursal);
        }

        //
        // GET: /TipoUsuarioSucursal/Edit/5

        public ActionResult Edit(decimal id = 0)
        {
            if (Session["user"] == null)
                return RedirectToAction("Login", "Login");
            UserLogin User = (UserLogin)Session["user"];
            int Per = DatComun.TipoPer(User.idRol, "TipoUsuarioSucursal", "Create");
            if (Per == 0)
                return RedirectToAction("Index", new { msgError = app_GlobalResources.Content.msg_AccesoDenegado });
            else if (Per == 1)
                return RedirectToAction("Index", new { msgError = app_GlobalResources.Content.msg_SinPrivilegio });
            ViewBag.isAdmin = User.isAdmin;
            TipoUsuarioSucursal tipousuariosucursal = db.TipoUsuarioSucursal.Find(id);
            if (tipousuariosucursal == null)
            {
                return HttpNotFound();
            }
            return View(tipousuariosucursal);
        }

        //
        // POST: /TipoUsuarioSucursal/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TipoUsuarioSucursal tipousuariosucursal)
        {
            if (Session["user"] == null)
                return RedirectToAction("Login", "Login");
            UserLogin User = (UserLogin)Session["user"];
            ViewBag.isAdmin = User.isAdmin;
            if (ModelState.IsValid)
            {
                DatComun.CreateLog("TipoUsuarioSucursal", "U", tipousuariosucursal.idTipoUsuarioSucursal, User.idUsuario);
                db.Entry(tipousuariosucursal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipousuariosucursal);
        }

        //
        // GET: /TipoUsuarioSucursal/Delete/5

        public ActionResult Delete(decimal id = 0)
        {
            if (Session["user"] == null)
                return RedirectToAction("Login", "Login");
            UserLogin User = (UserLogin)Session["user"];
            int Per = DatComun.TipoPer(User.idRol, "TipoUsuarioSucursal", "Create");
            if (Per == 0)
                return RedirectToAction("Index", new { msgError = app_GlobalResources.Content.msg_AccesoDenegado });
            else if (Per == 1)
                return RedirectToAction("Index", new { msgError = app_GlobalResources.Content.msg_SinPrivilegio });
            ViewBag.isAdmin = User.isAdmin;
            TipoUsuarioSucursal tipousuariosucursal = db.TipoUsuarioSucursal.Find(id);
            if (tipousuariosucursal == null)
            {
                return HttpNotFound();
            }
            return View(tipousuariosucursal);
        }

        //
        // POST: /TipoUsuarioSucursal/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            if (Session["user"] == null)
                return RedirectToAction("Login", "Login");
            UserLogin User = (UserLogin)Session["user"];
            TipoUsuarioSucursal tipousuariosucursal = db.TipoUsuarioSucursal.Find(id);
            DatComun.CreateLog("TipoUsuarioSucursal", "D", id, User.idUsuario);
            tipousuariosucursal.activo = false;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteM(decimal[] ids)
        {
            if (Session["user"] == null)
                return RedirectToAction("Login", "Login");
            UserLogin User = (UserLogin)Session["user"];
            int Per = DatComun.TipoPer(User.idRol, "TipoUsuarioSucursal", "Create");
            if (Per == 0)
                return RedirectToAction("Index", new { msgError = app_GlobalResources.Content.msg_AccesoDenegado });
            else if (Per == 1)
                return RedirectToAction("Index", new { msgError = app_GlobalResources.Content.msg_SinPrivilegio });
            ViewBag.isAdmin = User.isAdmin;
            ViewBag.Error = string.Empty;
            if (ids != null)
            {
                var id = ids.Cast<decimal>().ToList();
                var Query = db.TipoUsuarioSucursal.Include(t => t.Usuario).Where(q => id.Contains(q.idTipoUsuarioSucursal));
                return View(Query.ToList());
            }
            else
                return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteConfirmedM(decimal[] ids)
        {
            if (Session["user"] == null)
                return RedirectToAction("Login", "Login");
            UserLogin User = (UserLogin)Session["user"];
            if (this.IsCaptchaValid(string.Empty))
            {
                TipoUsuarioSucursal Item;
                for (int i = 0; i < ids.Length; i++)
                {
                    Item = db.TipoUsuarioSucursal.Find(ids[i]);
                    DatComun.CreateLog("TipoUsuarioSucursal", "D", ids[i], User.idUsuario);
                    Item.activo = false;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            ViewBag.isAdmin = User.isAdmin;
            ViewBag.Error = app_GlobalResources.Content.msg_ValCapcha;
            var id = ids.Cast<decimal>().ToList();
            var Query = db.TipoUsuarioSucursal.Include(t => t.Usuario).Where(q => id.Contains(q.idTipoUsuarioSucursal));
            return View("DeleteM", Query.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}