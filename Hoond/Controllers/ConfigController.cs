using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Threading;
using System.Globalization;
using Hoond.Models;

namespace Hoond.Controllers
{
    [Authorize]
    public class ConfigController : Controller
    {
        private HoondDBEntities db = new HoondDBEntities();
        //
        // GET: /Config/

        public ActionResult Index(string cmbEmpresa)
        {
            if (Session["user"] == null)
                return RedirectToAction("Login", "Login");
            UserLogin User = (UserLogin)Session["user"];
            var Query = from ur in db.UsuarioRol
                        join em in db.Empresa on ur.idEmpresa equals em.idEmpresa
                        where em.activo == true && ur.activo == true && ur.idUsuario == User.idUsuario
                        select new
                            {
                                em.idEmpresa,
                                em.nombreEmpresa
                            };
            if(cmbEmpresa != null)
            {
                User.isAdmin = false;
                User.idEmpresa = Convert.ToDecimal(cmbEmpresa);
                var QR = from ur in db.UsuarioRol
                         join ro in db.Rol on ur.idRol equals ro.idRol
                         where ur.activo == true && ur.idUsuario == User.idUsuario && ur.idEmpresa == User.idEmpresa
                         select new
                         {
                             ro.idRol,
                             ro.descRol
                         };
                var Result = QR.ToList()[0];
                if (Result.descRol.ToLower() == "sysAdmin" || Result.descRol.ToLower() == "sysSuperUser")
                    User.isAdmin = true;
                User.idRol = Result.idRol;
                Session["user"] = User;
            }
            ViewData["cmbEmpresa"] = new SelectList(Query, "idEmpresa", "nombreEmpresa", User.idEmpresa);
            return View();
        }

        public ActionResult Cambiar(String Idioma)
        {
            if (Session["user"] == null)
                return RedirectToAction("Login", "Login");
            UserLogin User = (UserLogin)Session["user"];
            var Query = from ur in db.UsuarioRol
                        join em in db.Empresa on ur.idEmpresa equals em.idEmpresa
                        where em.activo == true && ur.activo == true && ur.idUsuario == User.idUsuario
                        select new
                        {
                            em.idEmpresa,
                            em.nombreEmpresa
                        };
            ViewData["cmbEmpresa"] = new SelectList(Query, "idEmpresa", "nombreEmpresa", User.idEmpresa);            
            if (Idioma != null)
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Idioma);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Idioma);
            }
            HttpCookie cookie = new HttpCookie("Idioma");
            cookie.Value = Idioma;
            Response.Cookies.Add(cookie);
            return View("Index");
        }
    }
}
