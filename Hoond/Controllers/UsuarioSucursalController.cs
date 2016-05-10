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
    
    public class UsuarioSucursalController : Controller
    {
        private HoondDBEntities db = new HoondDBEntities();
        private Comun DatComun = new Comun();

        //
        // GET: /UsuarioSucursal/

        public ActionResult Index(string cmbCampo, string cmbPaginado, string sortField, string txtBusqueda, string msgError, int? page)
        {
            if (Session["user"] == null)
                return RedirectToAction("Login", "Login");
            UserLogin User = (UserLogin)Session["user"];
            if (User.idEmpresa == 0)
                return RedirectToAction("Index", "Config");
            int Per = DatComun.TipoPer(User.idRol, "UsuarioSucursal", "Index");
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
            var Query = db.UsuarioSucursal.Include(u => u.Usuario).Include(u => u.Usuario1).Include(u => u.Sucursal).Include(u => u.TipoUsuarioSucursal);
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
        // GET: /UsuarioSucursal/Details/5

        public ActionResult Details(decimal id = 0, decimal idUsuario = 0)
        {
            if (Session["user"] == null)
                return RedirectToAction("Login", "Login");
            UserLogin User = (UserLogin)Session["user"];
            int Per = DatComun.TipoPer(User.idRol, "UsuarioSucursal", "Index");
            if (Per == 0)
                return RedirectToAction("Index", new { msgError = app_GlobalResources.Content.msg_AccesoDenegado });
            ViewBag.isAdmin = User.isAdmin;
            UsuarioSucursal usuariosucursal = db.UsuarioSucursal.Find(id, idUsuario);
            if (usuariosucursal == null)
            {
                return HttpNotFound();
            }
            return View(usuariosucursal);
        }

        //
        // GET: /UsuarioSucursal/Create

        public ActionResult Create()
        {
           if (Session["user"] == null)
                return RedirectToAction("Login", "Login");
            UserLogin User = (UserLogin)Session["user"];
            int Per = DatComun.TipoPer(User.idRol, "UsuarioSucursal", "Create");
            if (Per == 0)
                return RedirectToAction("Index", new { msgError = app_GlobalResources.Content.msg_AccesoDenegado });
            else if (Per == 1)
                return RedirectToAction("Index", new { msgError = app_GlobalResources.Content.msg_SinPrivilegio });
            ViewBag.idUsuario = new SelectList(db.Usuario.Where(e => e.activo == true), "idUsuario", "nombreUsuario");
            ViewBag.idSucursal = new SelectList(db.Sucursal.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa), "idSucursal", "nombreSucursal");
            ViewBag.idTipoUsuarioSucursal = new SelectList(db.TipoUsuarioSucursal.Where(e => e.activo == true), "idTipoUsuarioSucursal", "descTipoUsuarioSucursal");
           
            return View();
        }

        //
        // POST: /UsuarioSucursal/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UsuarioSucursal usuariosucursal)
        {
            if (Session["user"] == null)
                return RedirectToAction("Login", "Login");
            UserLogin User = (UserLogin)Session ["user"];
            if (User.idEmpresa == 0)
                return RedirectToAction("Index", "Config");
            if (ModelState.IsValid)
            {

                var infoExistencia = (from US in db.UsuarioSucursal
                                                      where US.idUsuario == usuariosucursal.idUsuario && US.idSucursal == usuariosucursal.idSucursal 
                                                 select US).ToList();

                //Valida la existecia de lo seleccionado, si existe manda un mensaje.
                if (infoExistencia.Count >0)
                {
                    ViewBag.idUsuario = new SelectList(db.Usuario.Where(e => e.activo == true), "idUsuario", "nombreUsuario", usuariosucursal.idUsuario);
                    ViewBag.idSucursal = new SelectList(db.Sucursal.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa), "idSucursal", "nombreSucursal", usuariosucursal.idSucursal);
                    ViewBag.idTipoUsuarioSucursal = new SelectList(db.TipoUsuarioSucursal.Where(e => e.activo == true), "idTipoUsuarioSucursal", "descTipoUsuarioSucursal", usuariosucursal.idTipoUsuarioSucursal);
            
                    ViewBag.error = app_GlobalResources.Content.msg_ExitenciaUsuarioSucursal;
                    return View();
                }

              //  usuariosucursal.idUsuarioSucursal  = DatComun.GetId(1);
                usuariosucursal.idUsuario_alta  = User.idUsuario;
              //  usuariosucursal.idEmpresa = User.idEmpresa;
                usuariosucursal.fecha_alta = DateTime.Now;
                usuariosucursal.activo = true;
                db.UsuarioSucursal.Add(usuariosucursal);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idUsuario = new SelectList(db.Usuario.Where(e => e.activo == true), "idUsuario", "nombreUsuario", usuariosucursal.idUsuario);
            ViewBag.idSucursal = new SelectList(db.Sucursal.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa), "idSucursal", "nombreSucursal", usuariosucursal.idSucursal);
            ViewBag.idTipoUsuarioSucursal = new SelectList(db.TipoUsuarioSucursal.Where(e => e.activo == true), "idTipoUsuarioSucursal", "descTipoUsuarioSucursal", usuariosucursal.idTipoUsuarioSucursal);
            return View(usuariosucursal);
        }

        //
        // GET: /UsuarioSucursal/Edit/5

        public ActionResult Edit(decimal id = 0, decimal idUsuario = 0)
        {
            if (Session["user"] == null)
                return RedirectToAction("Login", "Login");
            UserLogin User = (UserLogin)Session["user"];
            int Per = DatComun.TipoPer(User.idRol, "UsuarioSucursal", "Create");
            if (Per == 0)
                return RedirectToAction("Index", new { msgError = app_GlobalResources.Content.msg_AccesoDenegado });
            else if (Per == 1)
                return RedirectToAction("Index", new { msgError = app_GlobalResources.Content.msg_SinPrivilegio });
            ViewBag.isAdmin = User.isAdmin;
            UsuarioSucursal usuariosucursal = db.UsuarioSucursal.Find(id, idUsuario);
            if (usuariosucursal == null)
            {
                return HttpNotFound();
            }
            ViewBag.idUsuario = new SelectList(db.Usuario.Where(e => e.activo == true), "idUsuario", "nombreUsuario", usuariosucursal.idUsuario);
            ViewBag.idSucursal = new SelectList(db.Sucursal.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa), "idSucursal", "nombreSucursal", usuariosucursal.idSucursal);
            ViewBag.idTipoUsuarioSucursal = new SelectList(db.TipoUsuarioSucursal.Where(e => e.activo == true), "idTipoUsuarioSucursal", "descTipoUsuarioSucursal", usuariosucursal.idTipoUsuarioSucursal);
            return View(usuariosucursal);
        }

        //
        // POST: /UsuarioSucursal/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UsuarioSucursal usuariosucursal)
        {
            if (Session["user"] == null)
                return RedirectToAction("Login", "Login");
            UserLogin User = (UserLogin)Session["user"];
            ViewBag.isAdmin = User.isAdmin;
            if (ModelState.IsValid)
            {
              //  DatComun.CreateLog("UsuarioSucursal", "U", usuariosucursal.idUsuarioSucursal, User.idUsuario);
                db.Entry(usuariosucursal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idUsuario = new SelectList(db.Usuario.Where(e => e.activo == true ), "idUsuario", "nombreUsuario", usuariosucursal.idUsuario);
            ViewBag.idSucursal = new SelectList(db.Sucursal.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa), "idSucursal", "nombreSucursal", usuariosucursal.idSucursal);
            ViewBag.idTipoUsuarioSucursal = new SelectList(db.TipoUsuarioSucursal.Where(e => e.activo == true), "idTipoUsuarioSucursal", "descTipoUsuarioSucursal", usuariosucursal.idTipoUsuarioSucursal);
            return View(usuariosucursal);
        }

        //
        // GET: /UsuarioSucursal/Delete/5

        public ActionResult Delete(decimal id = 0, decimal idUsuario =0)
        {
            if (Session["user"] == null)
                return RedirectToAction("Login", "Login");
            UserLogin User = (UserLogin)Session["user"];
            int Per = DatComun.TipoPer(User.idRol, "UsuarioSucursal", "Create");
            if (Per == 0)
                return RedirectToAction("Index", new { msgError = app_GlobalResources.Content.msg_AccesoDenegado });
            else if (Per == 1)
                return RedirectToAction("Index", new { msgError = app_GlobalResources.Content.msg_SinPrivilegio });
            ViewBag.isAdmin = User.isAdmin;
            UsuarioSucursal usuariosucursal = db.UsuarioSucursal.Find(id, idUsuario);
            if (usuariosucursal == null)
            {
                return HttpNotFound();
            }
            return View(usuariosucursal);
        }

        //
        // POST: /UsuarioSucursal/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id, decimal idUsuario = 0)
        {
            if (Session["user"] == null)
                return RedirectToAction("Login", "Login");
            UserLogin User = (UserLogin)Session["user"];
            UsuarioSucursal usuariosucursal = db.UsuarioSucursal.Find(id, idUsuario);
            DatComun.CreateLog("UsuarioSucursal", "D", id, User.idUsuario);
            usuariosucursal.activo = false;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteM(decimal[] ids)
        {
            if (Session["user"] == null)
                return RedirectToAction("Login", "Login");
            UserLogin User = (UserLogin)Session["user"];
            int Per = DatComun.TipoPer(User.idRol, "UsuarioSucursal", "Create");
            if (Per == 0)
                return RedirectToAction("Index", new { msgError = app_GlobalResources.Content.msg_AccesoDenegado });
            else if (Per == 1)
                return RedirectToAction("Index", new { msgError = app_GlobalResources.Content.msg_SinPrivilegio });
            ViewBag.isAdmin = User.isAdmin;
            ViewBag.Error = string.Empty;
            if (ids != null)
            {
                var id = ids.Cast<decimal>().ToList();
                var Query = db.UsuarioSucursal.Include(u => u.Usuario).Include(u => u.Usuario1).Include(u => u.Sucursal).Include(u => u.TipoUsuarioSucursal);//.Where(q => id.Contains(q.idUsuarioSucursal));
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
                UsuarioSucursal Item;
                for (int i = 0; i < ids.Length; i++)
                {
                    Item = db.UsuarioSucursal.Find(ids[i]);
                    DatComun.CreateLog("UsuarioSucursal", "D", ids[i], User.idUsuario);
                    Item.activo = false;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            ViewBag.isAdmin = User.isAdmin;
            ViewBag.Error = app_GlobalResources.Content.msg_ValCapcha;
            var id = ids.Cast<decimal>().ToList();
            var Query = db.UsuarioSucursal.Include(u => u.Usuario).Include(u => u.Usuario1).Include(u => u.Sucursal).Include(u => u.TipoUsuarioSucursal);//.Where(q => id.Contains(q.idUsuarioSucursal));
            return View("DeleteM", Query.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}