using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Hoond.Models;
using Hoond.App_Start;

using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using Facebook;

namespace Hoond.Controllers
{
    public class LoginController : Controller
    {
        private HoondDBEntities db = new HoondDBEntities();
        private Comun DatComun = new Comun();

        [HttpGet]
        public ActionResult LogIn(string msgError)
        {
            db.DeleteExpiredSessions();
            if (!String.IsNullOrEmpty(msgError))
                ModelState.AddModelError(string.Empty, msgError);
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(UserLogin u)
        {
            if (ModelState.IsValid)
            {
                string pasw = DatComun.getMd5Hash(u.password);
                bool cambioc = u.password == "password" ? true : false;
                var Query = from user in db.Usuario
                            where user.activo == true && user.username == u.username && user.password == pasw
                            select new UserLogin
                            {
                                idUsuario = user.idUsuario,
                                nombreUsuario = user.nombreUsuario,
                                username = user.username
                            };
                if (Query.ToList().Count != 0)
                {
                    u = Query.ToList()[0];
                    string Result = ValidaLogin(u, cambioc);
                    if (Result == string.Empty)
                    {
                        if (cambioc)
                            return RedirectToAction("CambioPassw");
                        else
                            return RedirectToAction("Index", "Home");
                    }
                    else
                        ModelState.AddModelError(string.Empty, Result);
                }
                else
                    ModelState.AddModelError(string.Empty, app_GlobalResources.Content.msg_UsuarioInc);
            }
            return View(u);
        }

        private string ValidaLogin(UserLogin u, bool cambioc)
        {
            db.DeleteExpiredSessions();
            u.idEmpresa = 0;
            u.isAdmin = false;
            var QueryS = from ses in db.ASPStateTempSessions
                         where ses.idUsuario == u.idUsuario
                         select new
                         {
                             ses.Device,
                         };
            if (QueryS.ToList().Count == 0)
            {
                var Ur = from ur in db.UsuarioRol
                         join ro in db.Rol on ur.idRol equals ro.idRol
                         where ur.activo == true && ur.idUsuario == u.idUsuario
                         select new
                         {
                             ur.idEmpresa,
                             ro.descRol,
                             ro.idRol
                         };
                if (Ur.ToList().Count != 0)
                {
                    var Result = Ur.ToList()[0];
                    u.idEmpresa = Result.idEmpresa;
                    if (Result.descRol.ToLower() == "sysAdmin" || Result.descRol.ToLower() == "sysSuperUser")
                        u.isAdmin = true;
                    u.idRol = Result.idRol;
                }
                HttpContext.Session.Add("user", u);
                if (!cambioc)
                    FormsAuthentication.SetAuthCookie(u.nombreUsuario, false);
                return string.Empty;
            }
            else
            {
                var Result = QueryS.ToList()[0];
                return app_GlobalResources.Content.msg_SessionIni + Result.Device;
            }
        }

        public ActionResult LogOut()
        {
            HttpContext.Session.RemoveAll();
            HttpContext.Session.Clear();
            HttpContext.Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult CambioPassw()
        {
            UserLogin User = (UserLogin)Session["user"];
            Cpassw Item = new Cpassw();
            Item.idUsuario = User.idUsuario;
            Item.nombreUsuario = User.nombreUsuario;
            return View(Item);
        }

        [HttpPost]
        public ActionResult CambioPassw(Cpassw Item)
        {
            if (ModelState.IsValid)
            {
                if (Item.password == Item.passwordC)
                {
                    Usuario Usu = db.Usuario.Where(q => q.idUsuario == Item.idUsuario).First();
                    Usu.password = DatComun.getMd5Hash(Item.password);
                    db.SaveChanges();
                    FormsAuthentication.SetAuthCookie(Item.nombreUsuario, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError(string.Empty, app_GlobalResources.Content.msg_ConfigCon);
            }
            return View(Item);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            GooglePlusClient.RewriteRequest();
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }
            string id = result.ExtraData["id"];
            var Query = from usu in db.Usuario
                        join up in db.UsuarioProvider on usu.idUsuario equals up.idUsuario
                        where usu.activo == true && up.id == id
                        select new UserLogin
                        {
                            idUsuario = usu.idUsuario,
                            nombreUsuario = usu.nombreUsuario,
                            username = usu.username
                        };
            if (Query.ToList().Count != 0)
            {
                UserLogin u = Query.ToList()[0];
                string Result = ValidaLogin(u, false);
                if (Result == string.Empty)
                    return RedirectToAction("Index", "Home");
                else
                    return RedirectToAction("Login", "Login", new { msgError = Result });
            }
            else
            {
                string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
                ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
                ViewBag.ReturnUrl = returnUrl;
                RegisterExternalLoginModel Model = new RegisterExternalLoginModel();
                Model.accesstoken = result.ExtraData["accesstoken"];
                Model.id = result.ExtraData["id"];
                Model.nombreusuario = result.ExtraData["name"];
                Model.provider = result.Provider.ToUpper();
                switch (Model.provider)
                {
                    case "FACEBOOK":
                        FacebookClient Fc = new FacebookClient(Model.accesstoken);
                        dynamic me = Fc.Get("me?fields=first_name,last_name,id,email");
                        Model.email = me.email;
                        break;
                    case "GOOGLEPLUS":
                        Model.email = result.ExtraData["email"];
                        break;
                }
                return View("ExternalLoginConfirmation", Model);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var Query = from usu in db.Usuario
                           where usu.email  == model.email || usu.username == model.username
                           select new
                           {
                                     usu.nombreUsuario
                           };
                if (Query.ToList().Count == 0)
                {
                    Usuario Item = new Usuario();
                    Item.activo = true;
                    Item.idUsuario = DatComun.GetId(1);
                    Item.idTipoLogin = 1601010001010012;
                    Item.nombreUsuario = model.nombreusuario.ToUpper();
                    Item.username = model.username.ToUpper();
                    Item.password = DatComun.getMd5Hash("password");
                    Item.email = model.email.ToLower();
                    Item.idUsuario_alta = 1601010001010011;
                    Item.fecha_alta = DateTime.Now;
                    Item.activo = true;
                    db.Usuario.Add(Item);
                    db.SaveChanges();

                    UsuarioProvider Item2 = new UsuarioProvider();
                    Item2.idUsuario = Item.idUsuario;
                    Item2.id = model.id;
                    Item2.provider = model.provider;
                    Item2.tokenProvider = model.accesstoken;
                    Item2.idUsuario_alta = 1601010001010011;
                    Item2.fecha_alta = DateTime.Now;
                    Item2.activo = true;
                    db.UsuarioProvider.Add(Item2);
                    db.SaveChanges();

                    UserLogin User = new UserLogin();
                    User.idEmpresa = 0;
                    User.idUsuario = Item.idUsuario;
                    User.isAdmin = false;
                    User.nombreUsuario = model.nombreusuario;
                    User.username = model.username;
                    HttpContext.Session.Add("user", User);
                    FormsAuthentication.SetAuthCookie(User.nombreUsuario, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", app_GlobalResources.Content.msg_CorreoExist);
            }

            ViewBag.ProviderDisplayName = model.provider;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        #endregion
    }
}
