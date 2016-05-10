using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hoond.Models;

namespace Hoond.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private HoondDBEntities db = new HoondDBEntities();
        public ActionResult Index(string msgError)
        {
            if (Session["user"] == null)
                return RedirectToAction("Login", "Login");
            UserLogin User = (UserLogin)Session["user"];
            if(User.sessionId == string.Empty || User.sessionId == null)
            {
                User.sessionId = HttpContext.Session.SessionID;
                var Query = db.ASPStateTempSessions.Where(c => c.SessionId.Contains(User.sessionId)).ToList();
                var Item = Query[0];
                Item.idUsuario = User.idUsuario;
                string Agent = HttpContext.Request.UserAgent.ToLower();
                string Device = "PC";
                string RemoTeIp = Request.ServerVariables.Get("REMOTE_ADDR");
                if (Agent.Contains("iphone"))
                    Device = "IPHONE";
                else if (Agent.Contains("ipad"))
                    Device = "IPAD";
                else if (Agent.Contains("ipod"))
                    Device = "IPOD";
                else if (Agent.Contains("android"))
                    Device = "ANDROID";
                Item.Device = Device;
                Item.Ip = RemoTeIp;
                db.SaveChanges();
                Session["user"] = User;
            }
            if (User.idEmpresa == 0)
                return RedirectToAction("Index", "Config");
            ViewBag.msgError = msgError;
            return View();
        }

        public ActionResult Change(string lang)
        {
            return View("Index");
        }

        public ActionResult Error()
        {
            ViewBag.Error = Session["Error"].ToString();
            return View();
        }
    }
}