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
    
    public class UsuarioProviderController : Controller
    {
        private HoondDBEntities db = new HoondDBEntities();
        private Comun DatComun = new Comun();

        //
        // GET: /UsuarioProvider/

        public ActionResult Index(string cmbCampo, string cmbPaginado, string sortField, string txtBusqueda, string msgError, int? page)
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User, "UsuarioProvider", "Index", false);
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
            var Query = db.UsuarioProvider.Include(u => u.Usuario).Include(u => u.Usuario1).Include(u => u.Provider1);
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
        // GET: /UsuarioProvider/Details/5

        public ActionResult Details(decimal id = 0)
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User, "UsuarioProvider", "Index", false);
            if (url["Action"] != string.Empty)
                return RedirectToAction(url["Action"], url["Controller"], new { msgError = url["msgError"] });
            ViewBag.isAdmin = User.isAdmin;
            UsuarioProvider usuarioprovider = db.UsuarioProvider.Where(q => q.idUsuario == id).First();
            if (usuarioprovider == null)
            {
                return HttpNotFound();
            }
            return View(usuarioprovider);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}