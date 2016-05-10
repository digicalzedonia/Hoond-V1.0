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
    
    public class MarcaController : Controller
    {
        private HoondDBEntities db = new HoondDBEntities();
        private Comun DatComun = new Comun();

        //
        // GET: /Marca/

        public ActionResult Index(string cmbCampo, string cmbPaginado, string sortField, string txtBusqueda, string msgError, int? page)
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User, "Marca", "Index", false);
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
            var Query = db.Marca.Include(m => m.Empresa).Include(m => m.Usuario).Include(m => m.Proveedor);
			if(!User.isAdmin)
				Query = Query.Where(q => q.activo == true);
            Query = Query.Where(q => q.idEmpresa == User.idEmpresa);
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
        // GET: /Marca/Details/5

        public ActionResult Details(decimal id = 0)
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User, "Marca", "Index", false);
            if (url["Action"] != string.Empty)
                return RedirectToAction(url["Action"], url["Controller"], new { msgError = url["msgError"] });
            ViewBag.isAdmin = User.isAdmin;
            Marca marca = db.Marca.Find(id);
            if (marca == null)
            {
                return HttpNotFound();
            }
            return View(marca);
        }

        //
        // GET: /Marca/Create

        public ActionResult Create()
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User, "Marca", "Create", true);
            if (url["Action"] != string.Empty)
                return RedirectToAction(url["Action"], url["Controller"], new { msgError = url["msgError"] });
            ViewBag.idProveedor = new SelectList(db.Proveedor.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa), "idProveedor", "nombreProveedor");
            return View();
        }

        //
        // POST: /Marca/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Marca marca)
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User);
            if (url["Action"] != string.Empty)
                return RedirectToAction(url["Action"], url["Controller"], new { msgError = url["msgError"] });
            if (ModelState.IsValid)
            {
                marca.idMarca  = DatComun.GetId(1);
                marca.idUsuario_alta  = User.idUsuario;
                marca.idEmpresa = User.idEmpresa;
                marca.fecha_alta = DateTime.Now;
                marca.activo = true;
                db.Marca.Add(marca);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idProveedor = new SelectList(db.Proveedor.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa), "idProveedor", "nombreProveedor", marca.idProveedor);
            return View(marca);
        }

        //
        // GET: /Marca/Edit/5

        public ActionResult Edit(decimal id = 0)
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User, "Marca", "Create", true);
            if (url["Action"] != string.Empty)
                return RedirectToAction(url["Action"], url["Controller"], new { msgError = url["msgError"] });
            ViewBag.isAdmin = User.isAdmin;
            Marca marca = db.Marca.Find(id);
            if (marca == null)
            {
                return HttpNotFound();
            }
            ViewBag.idProveedor = new SelectList(db.Proveedor.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa), "idProveedor", "nombreProveedor", marca.idProveedor);
            return View(marca);
        }

        //
        // POST: /Marca/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Marca marca)
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User);
            if (url["Action"] != string.Empty)
                return RedirectToAction(url["Action"], url["Controller"], new { msgError = url["msgError"] });
            ViewBag.isAdmin = User.isAdmin;
            if (ModelState.IsValid)
            {
                DatComun.CreateLog("Marca", "U", marca.idMarca, User.idUsuario);
                db.Entry(marca).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idProveedor = new SelectList(db.Proveedor.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa), "idProveedor", "nombreProveedor", marca.idProveedor);
            return View(marca);
        }

        //
        // GET: /Marca/Delete/5

        public ActionResult Delete(decimal id = 0)
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User, "Marca", "Create", true);
            if (url["Action"] != string.Empty)
                return RedirectToAction(url["Action"], url["Controller"], new { msgError = url["msgError"] });
            ViewBag.isAdmin = User.isAdmin;
            Marca marca = db.Marca.Find(id);
            if (marca == null)
            {
                return HttpNotFound();
            }
            return View(marca);
        }

        //
        // POST: /Marca/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User);
            if (url["Action"] != string.Empty)
                return RedirectToAction(url["Action"], url["Controller"], new { msgError = url["msgError"] });
            Marca marca = db.Marca.Find(id);
            DatComun.CreateLog("Marca", "D", id, User.idUsuario);
            marca.activo = false;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteM(decimal[] ids)
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User, "Marca", "Create", true);
            if (url["Action"] != string.Empty)
                return RedirectToAction(url["Action"], url["Controller"], new { msgError = url["msgError"] });
            ViewBag.isAdmin = User.isAdmin;
            ViewBag.Error = string.Empty;
            if (ids != null)
            {
                var id = ids.Cast<decimal>().ToList();
                var Query = db.Marca.Include(m => m.Empresa).Include(m => m.Usuario).Include(m => m.Proveedor).Where(q => id.Contains(q.idMarca));
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
                Marca Item;
                for (int i = 0; i < ids.Length; i++)
                {
                    Item = db.Marca.Find(ids[i]);
                    DatComun.CreateLog("Marca", "D", ids[i], User.idUsuario);
                    Item.activo = false;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            ViewBag.isAdmin = User.isAdmin;
            ViewBag.Error = app_GlobalResources.Content.msg_ValCapcha;
            var id = ids.Cast<decimal>().ToList();
            var Query = db.Marca.Include(m => m.Empresa).Include(m => m.Usuario).Include(m => m.Proveedor).Where(q => id.Contains(q.idMarca));
            return View("DeleteM", Query.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}