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
    
    public class UsuarioRolController : Controller
    {
        private HoondDBEntities db = new HoondDBEntities();
        private Comun DatComun = new Comun();

        //
        // GET: /UsuarioRol/

        public ActionResult Index(string cmbCampo, string cmbPaginado, string sortField, string txtBusqueda, string msgError, int? page)
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User, "UsuarioRol", "Index", false);
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
            var Query = db.UsuarioRol.Include(u => u.Empresa).Include(u => u.Rol).Include(u => u.Usuario).Include(u => u.Usuario1);
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
        // GET: /UsuarioRol/Details/5

        public ActionResult Details(decimal id = 0, decimal id2 = 0)
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User, "UsuarioRol", "Index", false);
            if (url["Action"] != string.Empty)
                return RedirectToAction(url["Action"], url["Controller"], new { msgError = url["msgError"] });
            ViewBag.isAdmin = User.isAdmin;
            UsuarioRol usuariorol = db.UsuarioRol.Where(q => q.idUsuario == id && q.idRol == id2).First();
            if (usuariorol == null)
            {
                return HttpNotFound();
            }
            return View(usuariorol);
        }

        //
        // GET: /UsuarioRol/Create

        public ActionResult Create()
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User, "UsuarioRol", "Create", true);
            if (url["Action"] != string.Empty)
                return RedirectToAction(url["Action"], url["Controller"], new { msgError = url["msgError"] });
            ViewBag.idRol = new SelectList(db.Rol.Where(e => e.activo == true), "idRol", "descRol");
            ViewBag.idUsuario = new SelectList(db.Usuario.Where(e => e.activo == true), "idUsuario", "nombreUsuario");
            ViewBag.idEmpresa = new SelectList(db.Empresa.Where(e => e.activo == true), "idEmpresa", "nombreEmpresa");
            return View();
        }

        //
        // POST: /UsuarioRol/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UsuarioRol usuariorol)
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User);
            if (url["Action"] != string.Empty)
                return RedirectToAction(url["Action"], url["Controller"], new { msgError = url["msgError"] });
            if (ModelState.IsValid)
            {
                usuariorol.idUsuario_alta  = User.idUsuario;
                usuariorol.fecha_alta = DateTime.Now;
                usuariorol.activo = true;
                db.UsuarioRol.Add(usuariorol);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idRol = new SelectList(db.Rol.Where(e => e.activo == true), "idRol", "descRol", usuariorol.idRol);
            ViewBag.idUsuario = new SelectList(db.Usuario.Where(e => e.activo == true), "idUsuario", "nombreUsuario", usuariorol.idUsuario);
            ViewBag.idEmpresa = new SelectList(db.Empresa.Where(e => e.activo == true), "idEmpresa", "nombreEmpresa", usuariorol.idEmpresa);
            return View(usuariorol);
        }

        //
        // GET: /UsuarioRol/Edit/5

        public ActionResult Edit(decimal id = 0, decimal id2 = 0)
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User, "UsuarioRol", "Create", true);
            if (url["Action"] != string.Empty)
                return RedirectToAction(url["Action"], url["Controller"], new { msgError = url["msgError"] });
            ViewBag.isAdmin = User.isAdmin;
            UsuarioRol usuariorol = db.UsuarioRol.Where(q => q.idUsuario == id && q.idRol == id2).First();
            if (usuariorol == null)
            {
                return HttpNotFound();
            }
            ViewBag.idRol = new SelectList(db.Rol.Where(e => e.activo == true), "idRol", "descRol", usuariorol.idRol);
            ViewBag.idUsuario = new SelectList(db.Usuario.Where(e => e.activo == true), "idUsuario", "nombreUsuario", usuariorol.idUsuario);
            ViewBag.idEmpresa = new SelectList(db.Empresa.Where(e => e.activo == true), "idEmpresa", "nombreEmpresa", usuariorol.idEmpresa);
            return View(usuariorol);
        }

        //
        // POST: /UsuarioRol/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UsuarioRol usuariorol)
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User);
            if (url["Action"] != string.Empty)
                return RedirectToAction(url["Action"], url["Controller"], new { msgError = url["msgError"] });
            ViewBag.isAdmin = User.isAdmin;
            if (ModelState.IsValid)
            {
                db.Entry(usuariorol).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idRol = new SelectList(db.Rol.Where(e => e.activo == true), "idRol", "descRol", usuariorol.idRol);
            ViewBag.idUsuario = new SelectList(db.Usuario.Where(e => e.activo == true), "idUsuario", "nombreUsuario", usuariorol.idUsuario);
            ViewBag.idEmpresa = new SelectList(db.Empresa.Where(e => e.activo == true), "idEmpresa", "nombreEmpresa", usuariorol.idEmpresa);
            return View(usuariorol);
        }

        //
        // GET: /UsuarioRol/Delete/5

        public ActionResult Delete(decimal id = 0, decimal id2 = 0)
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User, "UsuarioRol", "Create", true);
            if (url["Action"] != string.Empty)
                return RedirectToAction(url["Action"], url["Controller"], new { msgError = url["msgError"] });
            ViewBag.isAdmin = User.isAdmin;
            UsuarioRol usuariorol = db.UsuarioRol.Where(q => q.idUsuario == id && q.idRol == id2).First();
            if (usuariorol == null)
            {
                return HttpNotFound();
            }
            return View(usuariorol);
        }

        //
        // POST: /UsuarioRol/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id, decimal id2 = 0)
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User);
            if (url["Action"] != string.Empty)
                return RedirectToAction(url["Action"], url["Controller"], new { msgError = url["msgError"] });
            UsuarioRol usuariorol = db.UsuarioRol.Where(q => q.idUsuario == id && q.idRol == id2).First();
            DatComun.CreateLog("UsuarioRol", "D", id, User.idUsuario);
            usuariorol.activo = false;
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