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
    public class MenuItemsController : Controller
    {
        private HoondDBEntities db = new HoondDBEntities();

        [ChildActionOnly]
        public PartialViewResult _MenuLateral()
        {
            decimal idRol = 0;
            if (Session["user"] != null)
            {
                UserLogin User = (UserLogin)Session["user"];
                idRol = User.idRol;
            }
            var Mr = from rm in db.RolMenu
                     where rm.activo == true && rm.idRol == idRol
                     select new { rm.idMenu };
            List<decimal> Id = new List<decimal>();
            foreach (var item in Mr.ToList())
                Id.Add(item.idMenu);
            var Query = db.Menu.Include(m => m.Icono).Include(m => m.Menu2).Include(m => m.Usuario).Include(m => m.Vista).Where(q => q.activo == true && Id.Contains(q.idMenu));
            return PartialView("~/Views/Shared/partials/_MenuLateral.cshtml", Query.ToList());
        }
    }
}