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
    public class ComunController : Controller
    {
        private HoondDBEntities db = new HoondDBEntities();
        private Comun DatComun = new Comun();
        public PartialViewResult Mensaje(string msgError)
        {
            ViewBag.msgError = msgError;
            return PartialView();
        }
        public PartialViewResult Buscar(string cmbCampo, string txtBusqueda, string cmbPaginado, string Campos, string Descripcion)
        {
            ViewData["cmbPaginado"] = DatComun.GetPages();
            ViewData["cmbCampo"] = DatComun.GetCampos(Campos, Descripcion);
            return PartialView();
        }
        public PartialViewResult InsertProveedor()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult InsertProveedor(string Value)
        {
            if (!String.IsNullOrEmpty(Value))
            {
                UserLogin User = (UserLogin)Session["user"];
                Proveedor Item = new Proveedor();
                Item.idProveedor = DatComun.GetId(1);
                Item.idEmpresa = User.idEmpresa;
                Item.activo = true;
                Item.nombreProveedor = Value.ToUpper();
                Item.fecha_alta = DateTime.Now;
                Item.idUsuario_alta = User.idUsuario;
                db.Proveedor.Add(Item);
                db.SaveChanges();
                return Content(Item.idProveedor + "|" + Item.nombreProveedor);
            }
            else
                return PartialView();
        }

        public string GetProveedor()
        {
            UserLogin User = (UserLogin)Session["user"];
            var Query = from c in db.Proveedor
                        where c.activo == true && c.idEmpresa == User.idEmpresa 
                        orderby c.nombreProveedor
                        select new
                        {
                            id = c.idProveedor,
                            valor = c.nombreProveedor
                        };
            return Newtonsoft.Json.JsonConvert.SerializeObject(Query);
        }

        public PartialViewResult InsertMarca()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult InsertMarca(string Value, string idPadre)
        {
            if (!String.IsNullOrEmpty(Value))
            {
                UserLogin User = (UserLogin)Session["user"];
                Marca Item = new Marca();
                Item.idMarca = DatComun.GetId(1);
                Item.idEmpresa = User.idEmpresa;
                Item.idProveedor = Convert.ToDecimal(idPadre);
                Item.activo = true;
                Item.nombreMarca = Value.ToUpper();
                Item.fecha_alta = DateTime.Now;
                Item.idUsuario_alta = User.idUsuario;
                db.Marca.Add(Item);
                db.SaveChanges();
                return Content(Item.idMarca + "|" + Item.nombreMarca);
            }
            else
                return PartialView();
        }

        public string GetMarca(string idPadre)
        {
            decimal id = Convert.ToDecimal(idPadre);
            UserLogin User = (UserLogin)Session["user"];
            var Query = from c in db.Marca
                        where c.activo == true && c.idEmpresa == User.idEmpresa && c.idProveedor == id
                        orderby c.nombreMarca
                        select new
                        {
                            id = c.idMarca,
                            valor = c.nombreMarca
                        };
            return Newtonsoft.Json.JsonConvert.SerializeObject(Query);
        }

        public PartialViewResult InsertTipoArticulo()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult InsertTipoArticulo(string Value, string idPadre)
        {
            if (!String.IsNullOrEmpty(Value))
            {
                UserLogin User = (UserLogin)Session["user"];
                TipoArticulo Item = new TipoArticulo();
                Item.idTipoArticulo = DatComun.GetId(1);
                Item.idEmpresa = User.idEmpresa;
                Item.idMarca = Convert.ToDecimal(idPadre);
                Item.activo = true;
                Item.descTipoArticulo = Value.ToUpper();
                Item.fecha_alta = DateTime.Now;
                Item.idUsuario_alta = User.idUsuario;
                db.TipoArticulo.Add(Item);
                db.SaveChanges();
                return Content(Item.idTipoArticulo + "|" + Item.descTipoArticulo);
            }
            else
                return PartialView();
        }

        public string GetTipoArticulo(string idPadre)
        {
            decimal id = Convert.ToDecimal(idPadre);
            UserLogin User = (UserLogin)Session["user"];
            var Query = from c in db.TipoArticulo
                        where c.activo == true && c.idEmpresa == User.idEmpresa && c.idMarca == id
                        orderby c.descTipoArticulo
                        select new
                        {
                            id = c.idTipoArticulo,
                            valor = c.descTipoArticulo
                        };
            return Newtonsoft.Json.JsonConvert.SerializeObject(Query);
        }

        public PartialViewResult InsertColeccion()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult InsertColeccion(string Value, string idPadre)
        {
            if (!String.IsNullOrEmpty(Value))
            {
                UserLogin User = (UserLogin)Session["user"];
                Coleccion Item = new Coleccion();
                Item.idColeccion = DatComun.GetId(1);
                Item.idEmpresa = User.idEmpresa;
                Item.idMarca = Convert.ToDecimal(idPadre);
                Item.activo = true;
                Item.descColeccion = Value.ToUpper();
                Item.fecha_alta = DateTime.Now;
                Item.idUsuario_alta = User.idUsuario;
                db.Coleccion.Add(Item);
                db.SaveChanges();
                return Content(Item.idColeccion + "|" + Item.descColeccion);
            }
            else
                return PartialView();
        }

        public string GetColeccion(string idPadre)
        {
            decimal id = Convert.ToDecimal(idPadre);
            UserLogin User = (UserLogin)Session["user"];
            var Query = from c in db.Coleccion
                        where c.activo == true && c.idEmpresa == User.idEmpresa && c.idMarca == id
                        orderby c.descColeccion
                        select new
                        {
                            id = c.idColeccion,
                            valor = c.descColeccion
                        };
            return Newtonsoft.Json.JsonConvert.SerializeObject(Query);
        }

        public PartialViewResult InsertCategoria()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult InsertCategoria(string Value, string idPadre)
        {
            if (!String.IsNullOrEmpty(Value))
            {
                UserLogin User = (UserLogin)Session["user"];
                Categoria Item = new Categoria();
                Item.idCategoria = DatComun.GetId(1);
                Item.idEmpresa = User.idEmpresa;
                Item.idTipoArticulo = Convert.ToDecimal(idPadre);
                Item.activo = true;
                Item.descCategoria = Value.ToUpper();
                Item.fecha_alta = DateTime.Now;
                Item.idUsuario_alta = User.idUsuario;
                db.Categoria.Add(Item);
                db.SaveChanges();
                return Content(Item.idCategoria + "|" + Item.descCategoria);
            }
            else
                return PartialView();
        }

        public string GetCategoria(string idPadre)
        {
            decimal id = Convert.ToDecimal(idPadre);
            UserLogin User = (UserLogin)Session["user"];
            var Query = from c in db.Categoria
                        where c.activo == true && c.idEmpresa == User.idEmpresa && c.idTipoArticulo == id
                        orderby c.descCategoria
                        select new
                        {
                            id = c.idCategoria,
                            valor = c.descCategoria
                        };
            return Newtonsoft.Json.JsonConvert.SerializeObject(Query);
        }

        public PartialViewResult InsertSubCategoria()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult InsertSubCategoria(string Value, string idPadre)
        {
            if (!String.IsNullOrEmpty(Value))
            {
                UserLogin User = (UserLogin)Session["user"];
                SubCategoria Item = new SubCategoria();
                Item.idSubCategoria = DatComun.GetId(1);
                Item.idCategoria = Convert.ToDecimal(idPadre);
                Item.activo = true;
                Item.descSubCategoria = Value.ToUpper();
                Item.fecha_alta = DateTime.Now;
                Item.idUsuario_alta = User.idUsuario;
                db.SubCategoria.Add(Item);
                db.SaveChanges();
                return Content(Item.idSubCategoria + "|" + Item.descSubCategoria);
            }
            else
                return PartialView();
        }

        public string GetSubCategoria(string idPadre)
        {
            decimal id = Convert.ToDecimal(idPadre);
            var Query = from c in db.SubCategoria
                        where c.activo == true && c.idCategoria == id
                        orderby c.descSubCategoria
                        select new
                        {
                            id = c.idSubCategoria,
                            valor = c.descSubCategoria
                        };
            return Newtonsoft.Json.JsonConvert.SerializeObject(Query);
        }

        public PartialViewResult InsertColor()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult InsertColor(string Value, string idPadre)
        {
            if (!String.IsNullOrEmpty(Value))
            {
                UserLogin User = (UserLogin)Session["user"];
                Color Item = new Color();
                string[] vals = Value.Split(',');
                Item.idEmpresa = User.idEmpresa;
                Item.idTipoArticulo = Convert.ToDecimal(idPadre);
                Item.activo = true;
                Item.claveColor = vals[0].ToUpper();
                Item.descColor = vals[1].ToUpper();
                Item.fecha_alta = DateTime.Now;
                Item.idUsuario_alta = User.idUsuario;
                db.Color.Add(Item);
                db.SaveChanges();
                return Content(Item.claveColor + "|" + Item.descColor);
            }
            else
                return PartialView();
        }

        public string GetColor(string idPadre)
        {
            decimal id = Convert.ToDecimal(idPadre);
            UserLogin User = (UserLogin)Session["user"];
            var Query = from c in db.Color
                        where c.activo == true && c.idEmpresa == User.idEmpresa && c.idTipoArticulo == id
                        orderby c.descColor
                        select new
                        {
                            id = c.claveColor,
                            valor = c.descColor
                        };
            return Newtonsoft.Json.JsonConvert.SerializeObject(Query);
        }

        public PartialViewResult InsertTalla()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult InsertTalla(string Value, string idPadre)
        {
            if (!String.IsNullOrEmpty(Value))
            {
                UserLogin User = (UserLogin)Session["user"];
                Talla Item = new Talla();
                string[] vals = Value.Split(',');
                Item.idEmpresa = User.idEmpresa;
                Item.idTipoArticulo = Convert.ToDecimal(idPadre);
                Item.activo = true;
                Item.claveTalla = vals[0].ToUpper();
                Item.descTalla = vals[1].ToUpper();
                Item.fecha_alta = DateTime.Now;
                Item.idUsuario_alta = User.idUsuario;
                db.Talla.Add(Item);
                db.SaveChanges();
                return Content(Item.claveTalla + "|" + Item.descTalla);
            }
            else
                return PartialView();
        }

        public string GetTalla(string idPadre)
        {
            decimal id = Convert.ToDecimal(idPadre);
            UserLogin User = (UserLogin)Session["user"];
            var Query = from c in db.Talla
                        where c.activo == true && c.idEmpresa == User.idEmpresa && c.idTipoArticulo == id
                        orderby c.descTalla
                        select new
                        {
                            id = c.claveTalla,
                            valor = c.descTalla
                        };
            return Newtonsoft.Json.JsonConvert.SerializeObject(Query);
        }

        public PartialViewResult InsertPresentacion()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult InsertPresentacion(string Value, string idPadre)
        {
            if (!String.IsNullOrEmpty(Value))
            {
                UserLogin User = (UserLogin)Session["user"];
                Presentacion Item = new Presentacion();
                Item.idPresentacion = DatComun.GetId(1);
                Item.idEmpresa = User.idEmpresa;
                Item.idTipoArticulo = Convert.ToDecimal(idPadre);
                Item.activo = true;
                Item.descPresentacion = Value.ToUpper();
                Item.fecha_alta = DateTime.Now;
                Item.idUsuario_alta = User.idUsuario;
                db.Presentacion.Add(Item);
                db.SaveChanges();
                return Content(Item.idPresentacion + "|" + Item.descPresentacion);
            }
            else
                return PartialView();
        }

        public string GetPresentacion(string idPadre)
        {
            decimal id = Convert.ToDecimal(idPadre);
            UserLogin User = (UserLogin)Session["user"];
            var Query = from c in db.Presentacion
                        where c.activo == true && c.idEmpresa == User.idEmpresa && c.idTipoArticulo == id
                        orderby c.descPresentacion
                        select new
                        {
                            id = c.idPresentacion,
                            valor = c.descPresentacion
                        };
            return Newtonsoft.Json.JsonConvert.SerializeObject(Query);
        }

        public PartialViewResult InsertTipoLogin()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult InsertTipoLogin(string Value)
        {
            if (!String.IsNullOrEmpty(Value))
            {
                UserLogin User = (UserLogin)Session["user"];
                TipoLogin Item = new TipoLogin();
                Item.idTipoLogin = DatComun.GetId(1);
                Item.activo = true;
                Item.descTipoLogin = Value.ToUpper();
                Item.fecha_alta = DateTime.Now;
                Item.idUsuario_alta = User.idUsuario;
                db.TipoLogin.Add(Item);
                db.SaveChanges();
                return Content(Item.idTipoLogin + "|" + Item.descTipoLogin);
            }
            else
                return PartialView();
        }

        public string GetTipoLogin()
        {
            UserLogin User = (UserLogin)Session["user"];
            var Query = from c in db.TipoLogin
                        where c.activo == true 
                        orderby c.descTipoLogin
                        select new
                        {
                            id = c.idTipoLogin,
                            valor = c.descTipoLogin
                        };
            return Newtonsoft.Json.JsonConvert.SerializeObject(Query);
        }

        public PartialViewResult InsertRol()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult InsertRol(string Value)
        {
            if (!String.IsNullOrEmpty(Value))
            {
                UserLogin User = (UserLogin)Session["user"];
                Rol Item = new Rol();
                Item.idRol = DatComun.GetId(1);
                Item.activo = true;
                Item.descRol = Value.ToUpper();
                Item.fecha_alta = DateTime.Now;
                Item.idUsuario_alta = User.idUsuario;
                db.Rol.Add(Item);
                db.SaveChanges();
                return Content(Item.idRol + "|" + Item.descRol);
            }
            else
                return PartialView();
        }

        public string GetRol()
        {
            UserLogin User = (UserLogin)Session["user"];
            var Query = from c in db.Rol
                        where c.activo == true
                        orderby c.descRol
                        select new
                        {
                            id = c.idRol,
                            valor = c.descRol
                        };
            return Newtonsoft.Json.JsonConvert.SerializeObject(Query);
        }

        public PartialViewResult InsertTipoSucursal()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult InsertTipoSucursal(string Value)
        {
            if (!String.IsNullOrEmpty(Value))
            {
                UserLogin User = (UserLogin)Session["user"];
                TipoSucursal Item = new TipoSucursal();
                Item.idTipoSucursal = DatComun.GetId(1);
                Item.activo = true;
                Item.idEmpresa = User.idEmpresa;
                Item.descTipoSucursal = Value.ToUpper();
                Item.fecha_alta = DateTime.Now;
                Item.idUsuario_alta = User.idUsuario;
                db.TipoSucursal.Add(Item);
                db.SaveChanges();
                return Content(Item.idTipoSucursal + "|" + Item.descTipoSucursal);
            }
            else
                return PartialView();
        }

        public string GetTipoSucursal()
        {
            UserLogin User = (UserLogin)Session["user"];
            var Query = from c in db.TipoSucursal
                        where c.activo == true && c.idEmpresa == User.idEmpresa
                        orderby c.descTipoSucursal
                        select new
                        {
                            id = c.idTipoSucursal,
                            valor = c.descTipoSucursal
                        };
            return Newtonsoft.Json.JsonConvert.SerializeObject(Query);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}
