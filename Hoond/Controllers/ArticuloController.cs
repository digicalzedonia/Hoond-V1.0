using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using PagedList;
using System.IO;
using Excel;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Transactions;
using CaptchaMvc.HtmlHelpers;

using Hoond.Models;

namespace Hoond.Controllers
{
    [Authorize]
    
    public class ArticuloController : Controller
    {
        private HoondDBEntities db = new HoondDBEntities();
        private Comun DatComun = new Comun();
        public ActionResult Index(string cmbCampo, string cmbPaginado, string sortField, string txtBusqueda, string msgError, int? page)
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User, "Articulo", "Index", false);
            if (url["Action"] != string.Empty)
                return RedirectToAction(url["Action"], url["Controller"], new { msgError = url["msgError"] });
            ViewBag.isAdmin = User.isAdmin;
            ViewBag.msgError = msgError;            
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
            var Query = db.Articulo.Include(a => a.Empresa).Include(a => a.Usuario).Include(a => a.Categoria).Include(a => a.Coleccion).Include(a => a.Color).Include(a => a.Marca).Include(a => a.Presentacion).Include(a => a.Proveedor).Include(a => a.SubCategoria).Include(a => a.Talla).Include(a => a.TipoArticulo);
            if (!User.isAdmin)
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
        // GET: /Articulo/Details/5

        public ActionResult Details(decimal id = 0)
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User, "Articulo", "Index", false);
            if (url["Action"] != string.Empty)
                return RedirectToAction(url["Action"], url["Controller"], new { msgError = url["msgError"] });
            ViewBag.isAdmin = User.isAdmin;
            Articulo articulo = db.Articulo.Find(id);
            if (articulo == null)
            {
                return HttpNotFound();
            }
            return View(articulo);
        }

        //
        // GET: /Articulo/Create

        public ActionResult Create(decimal codigo = 0)
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User, "Articulo", "Create", true);
            if (url["Action"] != string.Empty)
                return RedirectToAction(url["Action"], url["Controller"], new { msgError = url["msgError"] });
            List<ArticuloCom> Detalle;
            if(codigo == 0)
            {
                ViewBag.idCategoria = new SelectList(db.Categoria.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa && e.idTipoArticulo == 0), "idCategoria", "descCategoria");
                ViewBag.idColeccion = new SelectList(db.Coleccion.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa && e.idMarca == 0), "idColeccion", "descColeccion");
                ViewBag.claveColor = new SelectList(db.Color.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa && e.idTipoArticulo == 0), "claveColor", "descColor");
                ViewBag.idMarca = new SelectList(db.Marca.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa && e.idProveedor == 0), "idMarca", "nombreMarca");
                ViewBag.idPresentacion = new SelectList(db.Presentacion.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa && e.idTipoArticulo == 0), "idPresentacion", "descPresentacion");
                ViewBag.idProveedor = new SelectList(db.Proveedor.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa), "idProveedor", "nombreProveedor");
                ViewBag.idSubCategoria = new SelectList(db.SubCategoria.Where(e => e.activo == true && e.idCategoria == 0), "idSubCategoria", "descSubCategoria");
                ViewBag.claveTalla = new SelectList(db.Talla.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa && e.idTipoArticulo == 0), "claveTalla", "descTalla");
                ViewBag.idTipoArticulo = new SelectList(db.TipoArticulo.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa && e.idMarca == 0), "idTipoArticulo", "descTipoArticulo");
                Detalle = new List<ArticuloCom>();
                Session["item"] = new ArticuloItem();
                Session["detalle"] = Detalle;
                ViewBag.Detalle = Detalle;
                return View();
            }
            else
            {
                Detalle = (List<ArticuloCom>)Session["detalle"];
                ArticuloItem ArticuloNew = (ArticuloItem)Session["item"];
                Articulo articulo = new Articulo();
                articulo.idProveedor = ArticuloNew.idProveedor;
                articulo.idTipoArticulo = ArticuloNew.idTipoArticulo;
                articulo.idMarca = ArticuloNew.idMarca;
                articulo.idColeccion = ArticuloNew.idColeccion;
                articulo.temporada = ArticuloNew.temporada;
                articulo.idCategoria = ArticuloNew.idCategoria;
                articulo.idSubCategoria = ArticuloNew.idSubCategoria;
                articulo.claveArticulo = ArticuloNew.claveArticulo;
                articulo.descArticulo = ArticuloNew.descArticulo;
                articulo.nombreArticulo = ArticuloNew.nombreArticulo;
                articulo.claveColor = ArticuloNew.claveColor;
                articulo.claveTalla = ArticuloNew.claveTalla;
                articulo.idPresentacion = ArticuloNew.idPresentacion;
                articulo.cantidadPresentacion = ArticuloNew.cantidadPresentacion;
                var Query = Detalle.Where(c => c.id == codigo).ToList();
                Detalle.Remove(Query[0]);
                ViewBag.idCategoria = new SelectList(db.Categoria.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa && e.idTipoArticulo == articulo.idTipoArticulo), "idCategoria", "descCategoria", articulo.idCategoria);
                ViewBag.idColeccion = new SelectList(db.Coleccion.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa && e.idMarca == articulo.idMarca), "idColeccion", "descColeccion", articulo.idColeccion);
                ViewBag.claveColor = new SelectList(db.Color.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa && e.idTipoArticulo == articulo.idTipoArticulo), "claveColor", "descColor", articulo.claveColor);
                ViewBag.idMarca = new SelectList(db.Marca.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa && e.idProveedor == articulo.idProveedor), "idMarca", "nombreMarca", articulo.idMarca);
                ViewBag.idPresentacion = new SelectList(db.Presentacion.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa && e.idTipoArticulo == articulo.idTipoArticulo), "idPresentacion", "descPresentacion", articulo.idPresentacion);
                ViewBag.idProveedor = new SelectList(db.Proveedor.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa), "idProveedor", "nombreProveedor", articulo.idProveedor);
                ViewBag.idSubCategoria = new SelectList(db.SubCategoria.Where(e => e.activo == true && e.idCategoria == articulo.idCategoria), "idSubCategoria", "descSubCategoria", articulo.idSubCategoria);
                ViewBag.claveTalla = new SelectList(db.Talla.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa && e.idTipoArticulo == articulo.idTipoArticulo), "claveTalla", "descTalla", articulo.claveTalla);
                ViewBag.idTipoArticulo = new SelectList(db.TipoArticulo.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa && e.idMarca == articulo.idMarca), "idTipoArticulo", "descTipoArticulo", articulo.idTipoArticulo);
                Detalle = Detalle.OrderBy(c => c.color).ThenBy(c => c.talla).ToList();
                Session["item"] = ArticuloNew;
                Session["detalle"] = Detalle;
                ViewBag.Detalle = Detalle;
                return View(articulo);
            }
        }

        //
        // POST: /Articulo/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Articulo articulo)
        {
            
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User);
            if (url["Action"] != string.Empty)
                return RedirectToAction(url["Action"], url["Controller"], new { msgError = url["msgError"] });
            List<ArticuloCom> Detalle = (List<ArticuloCom>)Session["detalle"];
            if (ModelState.IsValid)
            {
                if (!String.IsNullOrEmpty(articulo.claveColor))
                {
                    var Query = Detalle.Where(q => q.claveColor == articulo.claveColor).ToList();
                    if (Query.Count == 0)
                    {
                        var desColor = db.Color.Where(c => c.claveColor == articulo.claveColor).ToList();
                        ArticuloCom Item = new ArticuloCom();
                        Item.id = DatComun.GetId(2);
                        Item.claveColor = articulo.claveColor;
                        Item.color = desColor[0].descColor;
                        Detalle.Add(Item);
                    }
                }
                if(!String.IsNullOrEmpty(articulo.claveTalla))
                {
                    var Talla = db.Talla.Where(c => c.claveTalla == articulo.claveTalla).ToList();
                    var Colores = Detalle.Select(c => c.claveColor).Distinct().ToList();
                    foreach (var item in Colores)
                    {
                        var Query2 = Detalle.Where(q => q.claveColor == item && q.claveTalla == articulo.claveTalla).ToList();
                        if (Query2.Count == 0)
                        {
                            var claveColor = Detalle.Where(c => c.claveColor == item).ToList();
                            ArticuloCom Item = new ArticuloCom();
                            Item.id = DatComun.GetId(2);
                            Item.claveColor = claveColor[0].claveColor;
                            Item.color = claveColor[0].color;
                            Item.claveTalla = articulo.claveTalla;
                            Item.talla = Talla[0].descTalla;
                            Detalle.Add(Item);
                        }
                    }
                    var DelItem = Detalle.Where(c => String.IsNullOrEmpty(c.claveTalla)).ToList();
                    foreach (var item in DelItem)
                    {
                        Detalle.Remove(item);
                    }
                }
            }
            ViewBag.idCategoria = new SelectList(db.Categoria.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa && e.idTipoArticulo == articulo.idTipoArticulo), "idCategoria", "descCategoria", articulo.idCategoria);
            ViewBag.idColeccion = new SelectList(db.Coleccion.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa && e.idMarca == articulo.idMarca), "idColeccion", "descColeccion", articulo.idColeccion);
            ViewBag.claveColor = new SelectList(db.Color.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa && e.idTipoArticulo == articulo.idTipoArticulo), "claveColor", "descColor", articulo.claveColor);
            ViewBag.idMarca = new SelectList(db.Marca.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa && e.idProveedor == articulo.idProveedor), "idMarca", "nombreMarca", articulo.idMarca);
            ViewBag.idPresentacion = new SelectList(db.Presentacion.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa && e.idTipoArticulo == articulo.idTipoArticulo), "idPresentacion", "descPresentacion", articulo.idPresentacion);
            ViewBag.idProveedor = new SelectList(db.Proveedor.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa), "idProveedor", "nombreProveedor", articulo.idProveedor);
            ViewBag.idSubCategoria = new SelectList(db.SubCategoria.Where(e => e.activo == true && e.idCategoria == articulo.idCategoria), "idSubCategoria", "descSubCategoria", articulo.idSubCategoria);
            ViewBag.claveTalla = new SelectList(db.Talla.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa && e.idTipoArticulo == articulo.idTipoArticulo), "claveTalla", "descTalla", articulo.claveTalla);
            ViewBag.idTipoArticulo = new SelectList(db.TipoArticulo.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa && e.idMarca == articulo.idMarca), "idTipoArticulo", "descTipoArticulo", articulo.idTipoArticulo);
            ArticuloItem ArticuloNew = new ArticuloItem();
            ArticuloNew.idProveedor = articulo.idProveedor;
            ArticuloNew.idTipoArticulo = articulo.idTipoArticulo;
            ArticuloNew.idMarca = articulo.idMarca;
            ArticuloNew.idColeccion = articulo.idColeccion;
            ArticuloNew.temporada = articulo.temporada;
            ArticuloNew.idCategoria = articulo.idCategoria;
            ArticuloNew.idSubCategoria = articulo.idSubCategoria;
            ArticuloNew.claveArticulo = articulo.claveArticulo;
            ArticuloNew.descArticulo = articulo.descArticulo;
            ArticuloNew.nombreArticulo = articulo.nombreArticulo;
            ArticuloNew.claveColor = articulo.claveColor;
            ArticuloNew.claveTalla = articulo.claveTalla;
            ArticuloNew.idPresentacion = articulo.idPresentacion;
            ArticuloNew.cantidadPresentacion = articulo.cantidadPresentacion;
            Detalle = Detalle.OrderBy(c => c.color).ThenBy(c => c.talla).ToList();
            Session["item"] = ArticuloNew;
            Session["detalle"] = Detalle;
            ViewBag.Detalle = Detalle;
            return View(articulo);
        }

        public ActionResult Guardar(string[] codes, string[] skus)
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User);
            if (url["Action"] != string.Empty)
                return RedirectToAction(url["Action"], url["Controller"], new { msgError = url["msgError"] });
            ViewBag.msgError = string.Empty;
            Articulo articulo = new Articulo();
            ArticuloItem ArticuloNew = (ArticuloItem)Session["item"];
            List<ArticuloCom> Detalle = (List<ArticuloCom>)Session["detalle"];
            if (codes != null)
            {
                for (int i = 0; i < codes.Length; i++)
                {
                    if (String.IsNullOrEmpty(codes[i]))
                    {
                        ViewBag.msgError = String.Format(app_GlobalResources.Content.msg_CampoReq, "codigo de barras");
                        break;
                    }
                    if (String.IsNullOrEmpty(Detalle[i].claveTalla))
                    {
                        ViewBag.msgError = String.Format(app_GlobalResources.Content.msg_CampoReq, "talla");
                        break;
                    }
                    if (String.IsNullOrEmpty(skus[i]))
                        skus[i] = ArticuloNew.claveArticulo + " " + Detalle[i].claveColor + " " + Detalle[i].claveTalla;
                }
                articulo.idProveedor = ArticuloNew.idProveedor;
                articulo.idTipoArticulo = ArticuloNew.idTipoArticulo;
                articulo.idMarca = ArticuloNew.idMarca;
                articulo.idColeccion = ArticuloNew.idColeccion;
                articulo.temporada = ArticuloNew.temporada;
                articulo.idCategoria = ArticuloNew.idCategoria;
                articulo.idSubCategoria = ArticuloNew.idSubCategoria;
                articulo.claveArticulo = ArticuloNew.claveArticulo;
                articulo.descArticulo = ArticuloNew.descArticulo;
                articulo.nombreArticulo = ArticuloNew.nombreArticulo;
                articulo.claveColor = ArticuloNew.claveColor;
                articulo.claveTalla = ArticuloNew.claveTalla;
                articulo.idPresentacion = ArticuloNew.idPresentacion;
                articulo.cantidadPresentacion = ArticuloNew.cantidadPresentacion;
                if (ViewBag.msgError == string.Empty)
                {
                    if (ModelState.IsValid)
                    {
                        using (var transaccion = new TransactionScope())
                        {
                            try
                            {
                                for (int i = 0; i < Detalle.Count; i++)
                                {
                                    Articulo articuloInsert = new Articulo();
                                    articuloInsert.idProveedor = ArticuloNew.idProveedor;
                                    articuloInsert.idTipoArticulo = ArticuloNew.idTipoArticulo;
                                    articuloInsert.idMarca = ArticuloNew.idMarca;
                                    articuloInsert.idColeccion = ArticuloNew.idColeccion;
                                    articuloInsert.temporada = ArticuloNew.temporada;
                                    articuloInsert.idCategoria = ArticuloNew.idCategoria;
                                    articuloInsert.idSubCategoria = ArticuloNew.idSubCategoria;
                                    articuloInsert.claveArticulo = ArticuloNew.claveArticulo;
                                    articuloInsert.descArticulo = ArticuloNew.descArticulo;
                                    articuloInsert.nombreArticulo = ArticuloNew.nombreArticulo;
                                    articuloInsert.claveColor = ArticuloNew.claveColor;
                                    articuloInsert.claveTalla = ArticuloNew.claveTalla;
                                    articuloInsert.idPresentacion = ArticuloNew.idPresentacion;
                                    articuloInsert.cantidadPresentacion = ArticuloNew.cantidadPresentacion;

                                    articuloInsert.barcode = Convert.ToDecimal(codes[i]);
                                    articuloInsert.sku = skus[i];
                                    articuloInsert.claveColor = Detalle[i].claveColor;
                                    articuloInsert.claveTalla = Detalle[i].claveTalla;
                                    articuloInsert.idArticulo = DatComun.GetId(1);
                                    articuloInsert.idEmpresa = User.idEmpresa;
                                    articuloInsert.idUsuario_alta = User.idUsuario;
                                    articuloInsert.fecha_alta = DateTime.Now;
                                    articuloInsert.activo = true;
                                    articuloInsert.epcSerie = 1;
                                    RFID DatRfid = new RFID();
                                    articuloInsert.epcPrefijo = DatRfid.GenerateEPCfromBarCode(codes[i], "1");
                                    db.Articulo.Add(articuloInsert);
                                    db.SaveChanges();
                                }
                                transaccion.Complete();
                                return RedirectToAction("Index");
                            }
                            catch (Exception ex)
                            {
                                ViewBag.msgError = ex.Message;
                            }
                        }
                    }
                }
            }
            ViewBag.idCategoria = new SelectList(db.Categoria.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa && e.idTipoArticulo == articulo.idTipoArticulo), "idCategoria", "descCategoria", articulo.idCategoria);
            ViewBag.idColeccion = new SelectList(db.Coleccion.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa && e.idMarca == articulo.idMarca), "idColeccion", "descColeccion", articulo.idColeccion);
            ViewBag.claveColor = new SelectList(db.Color.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa && e.idTipoArticulo == articulo.idTipoArticulo), "claveColor", "descColor", articulo.claveColor);
            ViewBag.idMarca = new SelectList(db.Marca.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa && e.idProveedor == articulo.idProveedor), "idMarca", "nombreMarca", articulo.idMarca);
            ViewBag.idPresentacion = new SelectList(db.Presentacion.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa && e.idTipoArticulo == articulo.idTipoArticulo), "idPresentacion", "descPresentacion", articulo.idPresentacion);
            ViewBag.idProveedor = new SelectList(db.Proveedor.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa), "idProveedor", "nombreProveedor", articulo.idProveedor);
            ViewBag.idSubCategoria = new SelectList(db.SubCategoria.Where(e => e.activo == true && e.idCategoria == articulo.idCategoria), "idSubCategoria", "descSubCategoria", articulo.idSubCategoria);
            ViewBag.claveTalla = new SelectList(db.Talla.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa && e.idTipoArticulo == articulo.idTipoArticulo), "claveTalla", "descTalla", articulo.claveTalla);
            ViewBag.idTipoArticulo = new SelectList(db.TipoArticulo.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa && e.idMarca == articulo.idMarca), "idTipoArticulo", "descTipoArticulo", articulo.idTipoArticulo);
            Session["item"] = ArticuloNew;
            Session["detalle"] = Detalle;
            ViewBag.Detalle = Detalle;
            if (ArticuloNew.idProveedor == 0)
                return View("Create");
            else
                return View("Create", articulo);
        }

        //
        // GET: /Articulo/Edit/5

        public ActionResult Edit(decimal id = 0)
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User, "Articulo", "Create", true);
            if (url["Action"] != string.Empty)
                return RedirectToAction(url["Action"], url["Controller"], new { msgError = url["msgError"] });
            ViewBag.isAdmin = User.isAdmin;
            Articulo articulo = db.Articulo.Find(id);
            if (articulo == null)
            {
                return HttpNotFound();
            }
            ViewBag.idCategoria = new SelectList(db.Categoria.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa && e.idTipoArticulo == articulo.idTipoArticulo), "idCategoria", "descCategoria", articulo.idCategoria);
            ViewBag.idColeccion = new SelectList(db.Coleccion.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa && e.idMarca == articulo.idMarca), "idColeccion", "descColeccion", articulo.idColeccion);
            ViewBag.claveColor = new SelectList(db.Color.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa && e.idTipoArticulo == articulo.idTipoArticulo), "claveColor", "descColor", articulo.claveColor);
            ViewBag.idMarca = new SelectList(db.Marca.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa && e.idProveedor == articulo.idProveedor), "idMarca", "nombreMarca", articulo.idMarca);
            ViewBag.idPresentacion = new SelectList(db.Presentacion.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa && e.idTipoArticulo == articulo.idTipoArticulo), "idPresentacion", "descPresentacion", articulo.idPresentacion);
            ViewBag.idProveedor = new SelectList(db.Proveedor.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa), "idProveedor", "nombreProveedor", articulo.idProveedor);
            ViewBag.idSubCategoria = new SelectList(db.SubCategoria.Where(e => e.activo == true && e.idCategoria == articulo.idCategoria), "idSubCategoria", "descSubCategoria", articulo.idSubCategoria);
            ViewBag.claveTalla = new SelectList(db.Talla.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa && e.idTipoArticulo == articulo.idTipoArticulo), "claveTalla", "descTalla", articulo.claveTalla);
            ViewBag.idTipoArticulo = new SelectList(db.TipoArticulo.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa && e.idMarca == articulo.idMarca), "idTipoArticulo", "descTipoArticulo", articulo.idTipoArticulo);
            return View(articulo);
        }

        //
        // POST: /Articulo/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Articulo articulo)
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User);
            if (url["Action"] != string.Empty)
                return RedirectToAction(url["Action"], url["Controller"], new { msgError = url["msgError"] });
            if (ModelState.IsValid)
            {
                DatComun.CreateLog("Articulo", "U", articulo.idArticulo, User.idUsuario);
                db.Entry(articulo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idCategoria = new SelectList(db.Categoria.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa && e.idTipoArticulo == articulo.idTipoArticulo), "idCategoria", "descCategoria", articulo.idCategoria);
            ViewBag.idColeccion = new SelectList(db.Coleccion.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa && e.idMarca == articulo.idMarca), "idColeccion", "descColeccion", articulo.idColeccion);
            ViewBag.claveColor = new SelectList(db.Color.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa && e.idTipoArticulo == articulo.idTipoArticulo), "claveColor", "descColor", articulo.claveColor);
            ViewBag.idMarca = new SelectList(db.Marca.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa && e.idProveedor == articulo.idProveedor), "idMarca", "nombreMarca", articulo.idMarca);
            ViewBag.idPresentacion = new SelectList(db.Presentacion.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa && e.idTipoArticulo == articulo.idTipoArticulo), "idPresentacion", "descPresentacion", articulo.idPresentacion);
            ViewBag.idProveedor = new SelectList(db.Proveedor.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa), "idProveedor", "nombreProveedor", articulo.idProveedor);
            ViewBag.idSubCategoria = new SelectList(db.SubCategoria.Where(e => e.activo == true && e.idCategoria == articulo.idCategoria), "idSubCategoria", "descSubCategoria", articulo.idSubCategoria);
            ViewBag.claveTalla = new SelectList(db.Talla.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa && e.idTipoArticulo == articulo.idTipoArticulo), "claveTalla", "descTalla", articulo.claveTalla);
            ViewBag.idTipoArticulo = new SelectList(db.TipoArticulo.Where(e => e.activo == true && e.idEmpresa == User.idEmpresa && e.idMarca == articulo.idMarca), "idTipoArticulo", "descTipoArticulo", articulo.idTipoArticulo);
            return View(articulo);
        }

        //
        // GET: /Articulo/Delete/5

        public ActionResult Delete(decimal id = 0)
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User, "Articulo", "Create", true);
            if (url["Action"] != string.Empty)
                return RedirectToAction(url["Action"], url["Controller"], new { msgError = url["msgError"] });
            ViewBag.isAdmin = User.isAdmin;
            Articulo articulo = db.Articulo.Find(id);
            if (articulo == null)
            {
                return HttpNotFound();
            }
            return View(articulo);
        }

        //
        // POST: /Default1/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User);
            if (url["Action"] != string.Empty)
                return RedirectToAction(url["Action"], url["Controller"], new { msgError = url["msgError"] });
            Articulo articulo = db.Articulo.Find(id);
            DatComun.CreateLog("Articulo", "D", id, User.idUsuario);
            articulo.activo = false;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteM(decimal[] ids)
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User, "Articulo", "Create", true);
            if (url["Action"] != string.Empty)
                return RedirectToAction(url["Action"], url["Controller"], new { msgError = url["msgError"] });
            ViewBag.isAdmin = User.isAdmin;
            ViewBag.Error = string.Empty;
            if (ids != null)
            {
                var id = ids.Cast<decimal>().ToList();
                var Query = db.Articulo.Include(a => a.Empresa).Include(a => a.Usuario).Include(a => a.Categoria).Include(a => a.Coleccion).Include(a => a.Color).Include(a => a.Marca).Include(a => a.Presentacion).Include(a => a.Proveedor).Include(a => a.SubCategoria).Include(a => a.Talla).Include(a => a.TipoArticulo).Where(q => id.Contains(q.idArticulo));
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
                Articulo Item;
                for (int i = 0; i < ids.Length; i++)
                {
                    Item = db.Articulo.Find(ids[i]);
                    DatComun.CreateLog("Articulo", "D", ids[i], User.idUsuario);
                    Item.activo = false;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            ViewBag.isAdmin = User.isAdmin;
            ViewBag.Error = app_GlobalResources.Content.msg_ValCapcha;
            var id = ids.Cast<decimal>().ToList();
            var Query = db.Articulo.Include(a => a.Empresa).Include(a => a.Usuario).Include(a => a.Categoria).Include(a => a.Coleccion).Include(a => a.Color).Include(a => a.Marca).Include(a => a.Presentacion).Include(a => a.Proveedor).Include(a => a.SubCategoria).Include(a => a.Talla).Include(a => a.TipoArticulo).Where(q => id.Contains(q.idArticulo));
            return View("DeleteM", Query.ToList());
        }

        public ActionResult CargaMasiva()
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User, "Articulo", "Create", true);
            if (url["Action"] != string.Empty)
                return RedirectToAction(url["Action"], url["Controller"], new { msgError = url["msgError"] });
            return View();
        }

        [HttpPost]
        public ActionResult CargaMasiva(HttpPostedFileBase uploadFile)
        {
            UserLogin User = (UserLogin)Session["user"];
            Dictionary<string, string> url = DatComun.ValidaSession(ref User);
            if (url["Action"] != string.Empty)
                return RedirectToAction(url["Action"], url["Controller"], new { msgError = url["msgError"] });
            string FileName = Path.GetFileName(uploadFile.FileName);
            string FileMime = uploadFile.FileName.EndsWith("xls").ToString().ToLower() == "xls" ? "xls" : "xlsx";
            string FilePath = Path.Combine(Server.MapPath("/Uploads/Articulos"), FileName);
            uploadFile.SaveAs(FilePath);
            var excelData = new ExcelData(FilePath);
            var ListaHojas = excelData.getWorksheetNames();
            foreach (var Hoja in ListaHojas)
            {
                var Datos = excelData.getData(Hoja, true);
                foreach (DataRow Dr in Datos)
                {
                    Empresa ItemE = new Empresa();
                    Proveedor ItemP = new Proveedor();
                    Marca ItemM = new Marca();
                    TipoArticulo ItemTa = new TipoArticulo();
                    Coleccion ItemCo = new Coleccion();
                    Categoria ItemCa = new Categoria();
                    SubCategoria ItemSc = new SubCategoria();
                    Presentacion ItemPre = new Presentacion();
                    Articulo ItemAr = new Articulo();
                    Caracteristica ItemCt = new Caracteristica();
                    Caracteristica ItemCc = new Caracteristica();
                    Talla ItemT = new Talla();
                    Color ItemC = new Color();
                    var QueryE = db.Empresa.Where("nombreEmpresa = @0", Dr["Empresa"].ToString().Trim().ToUpper());
                    if (QueryE.ToList().Count == 0)
                    {
                        ItemE.idEmpresa = DatComun.GetId(1);
                        ItemE.nombreEmpresa = Dr["Empresa"].ToString().Trim().ToUpper();
                        ItemE.idUsuario_alta = User.idUsuario;
                        ItemE.fecha_alta = DateTime.Now;
                        ItemE.activo = true;
                        db.Empresa.Add(ItemE);
                        db.SaveChanges();
                    }
                    else
                        ItemE = QueryE.ToList()[0];
                    var QueryP = db.Proveedor.Where("nombreProveedor = @0", Dr["Proveedor"].ToString().Trim().ToUpper());
                    if (QueryP.ToList().Count == 0)
                    {
                        ItemP.idProveedor = DatComun.GetId(1);
                        ItemP.idEmpresa = ItemE.idEmpresa;
                        ItemP.nombreProveedor = Dr["Proveedor"].ToString().Trim().ToUpper();
                        ItemP.idUsuario_alta = User.idUsuario;
                        ItemP.fecha_alta = DateTime.Now;
                        ItemP.activo = true;
                        db.Proveedor.Add(ItemP);
                        db.SaveChanges();
                    }
                    else
                        ItemP = QueryP.ToList()[0];
                    var QueryM = db.Marca.Where("nombreMarca = @0", Dr["marca"].ToString().Trim().ToUpper());
                    if (QueryM.ToList().Count == 0)
                    {
                        ItemM.idMarca = DatComun.GetId(1);
                        ItemM.idEmpresa = ItemE.idEmpresa;
                        ItemM.idProveedor = ItemP.idProveedor;
                        ItemM.nombreMarca = Dr["marca"].ToString().Trim().ToUpper();
                        ItemM.idUsuario_alta = User.idUsuario;
                        ItemM.fecha_alta = DateTime.Now;
                        ItemM.activo = true;
                        db.Marca.Add(ItemM);
                        db.SaveChanges();
                    }
                    else
                        ItemM = QueryM.ToList()[0];
                    var QueryTa = db.TipoArticulo.Where("descTipoArticulo = @0", Dr["tipoArticulo"].ToString().Trim().ToUpper());
                    if (QueryTa.ToList().Count == 0)
                    {
                        ItemTa.idTipoArticulo = DatComun.GetId(1);
                        ItemTa.idMarca = ItemM.idMarca;
                        ItemTa.idEmpresa = ItemE.idEmpresa;
                        ItemTa.descTipoArticulo = Dr["tipoArticulo"].ToString().Trim().ToUpper();
                        ItemTa.idUsuario_alta = User.idUsuario;
                        ItemTa.fecha_alta = DateTime.Now;
                        ItemTa.activo = true;
                        db.TipoArticulo.Add(ItemTa);
                        db.SaveChanges();
                    }
                    else
                        ItemTa = QueryTa.ToList()[0];
                    var QueryCo = db.Coleccion.Where("descColeccion = @0", Dr["colección"].ToString().Trim().ToUpper());
                    if (QueryCo.ToList().Count == 0)
                    {
                        ItemCo.idColeccion = DatComun.GetId(1);
                        ItemCo.idMarca = ItemM.idMarca;
                        ItemCo.idEmpresa = ItemE.idEmpresa;
                        ItemCo.descColeccion = Dr["colección"].ToString().Trim().ToUpper();
                        ItemCo.idUsuario_alta = User.idUsuario;
                        ItemCo.fecha_alta = DateTime.Now;
                        ItemCo.activo = true;
                        db.Coleccion.Add(ItemCo);
                        db.SaveChanges();
                    }
                    else
                        ItemCo = QueryCo.ToList()[0];
                    var QueryCa = db.Categoria.Where("descCategoria = @0", Dr["categoria"].ToString().Trim().ToUpper());
                    if (QueryCa.ToList().Count == 0)
                    {
                        ItemCa.idCategoria = DatComun.GetId(1);
                        ItemCa.idTipoArticulo = ItemTa.idTipoArticulo;
                        ItemCa.idEmpresa = ItemE.idEmpresa;
                        ItemCa.descCategoria = Dr["categoria"].ToString().Trim().ToUpper();
                        ItemCa.idUsuario_alta = User.idUsuario;
                        ItemCa.fecha_alta = DateTime.Now;
                        ItemCa.activo = true;
                        db.Categoria.Add(ItemCa);
                        db.SaveChanges();
                    }
                    else
                        ItemCa = QueryCa.ToList()[0];
                    var QuerySc = db.SubCategoria.Where("descSubCategoria = @0", Dr["subCategoria"].ToString().Trim().ToUpper());
                    if (QuerySc.ToList().Count == 0)
                    {
                        ItemSc.idSubCategoria = DatComun.GetId(1);
                        ItemSc.idCategoria = ItemCa.idCategoria;
                        ItemSc.descSubCategoria = Dr["subCategoria"].ToString().Trim().ToUpper();
                        ItemSc.idUsuario_alta = User.idUsuario;
                        ItemSc.fecha_alta = DateTime.Now;
                        ItemSc.activo = true;
                        db.SubCategoria.Add(ItemSc);
                        db.SaveChanges();
                    }
                    else
                        ItemSc = QuerySc.ToList()[0];
                    var QueryPre = db.Presentacion.Where("descPresentacion = @0", Dr["Presentacion"].ToString().Trim().ToUpper());
                    if (QueryPre.ToList().Count == 0)
                    {
                        ItemPre.idPresentacion = DatComun.GetId(1);
                        ItemPre.idEmpresa = ItemE.idEmpresa;
                        ItemPre.idTipoArticulo = ItemTa.idTipoArticulo;
                        ItemPre.descPresentacion = Dr["Presentacion"].ToString().Trim().ToUpper();
                        ItemPre.idUsuario_alta = User.idUsuario;
                        ItemPre.fecha_alta = DateTime.Now;
                        ItemPre.activo = true;
                        db.Presentacion.Add(ItemPre);
                        db.SaveChanges();
                    }
                    else
                        ItemPre = QueryPre.ToList()[0];
                    var QueryT = db.Talla.Where("claveTalla = @0", Dr["claveTalla"].ToString().Trim().ToUpper());
                    if (QueryT.ToList().Count == 0)
                    {
                        ItemT.claveTalla = Dr["claveTalla"].ToString().Trim().ToUpper();
                        ItemT.idEmpresa = ItemE.idEmpresa;
                        ItemT.idTipoArticulo = ItemTa.idTipoArticulo;
                        ItemT.descTalla = Dr["claveTalla"].ToString().Trim().ToUpper();
                        ItemT.posicion = 1;
                        ItemT.idUsuario_alta = User.idUsuario;
                        ItemT.fecha_alta = DateTime.Now;
                        ItemT.activo = true;
                        db.Talla.Add(ItemT);
                        db.SaveChanges();
                    }
                    else
                        ItemT = QueryT.ToList()[0];
                    var QueryC = db.Color.Where("claveColor = @0", Dr["claveColor"].ToString().Trim().ToUpper());
                    if (QueryC.ToList().Count == 0)
                    {
                        ItemC.claveColor = Dr["claveColor"].ToString().Trim().ToUpper();
                        ItemC.idEmpresa = ItemE.idEmpresa;
                        ItemC.idTipoArticulo = ItemTa.idTipoArticulo;
                        ItemC.descColor = Dr["claveColor"].ToString().Trim().ToUpper();
                        ItemC.idUsuario_alta = User.idUsuario;
                        ItemC.fecha_alta = DateTime.Now;
                        ItemC.activo = true;
                        db.Color.Add(ItemC);
                        db.SaveChanges();
                    }
                    else
                        ItemC = QueryC.ToList()[0];
                    var QueryCt = db.Caracteristica.Where("claveCaracteristica = @0 and idEmpresa = @1 ", "TALLA", ItemE.idEmpresa);
                    if (QueryCt.ToList().Count == 0)
                    {
                        ItemCt.idCaracteristica = DatComun.GetId(1);
                        ItemCt.idEmpresa = ItemE.idEmpresa;
                        ItemCt.claveCaracteristica = "TALLA";
                        ItemCt.descCaracteristica = "TALLA";
                        ItemCt.idUsuario_alta = User.idUsuario;
                        ItemCt.fecha_alta = DateTime.Now;
                        ItemCt.activo = true;
                        db.Caracteristica.Add(ItemCt);
                        db.SaveChanges();
                    }
                    else
                        ItemCt = QueryCt.ToList()[0];
                    var QueryCc = db.Caracteristica.Where("claveCaracteristica = @0 and idEmpresa = @1 ", "COLOR", ItemE.idEmpresa);
                    if (QueryCc.ToList().Count == 0)
                    {
                        ItemCc.idCaracteristica = DatComun.GetId(1);
                        ItemCc.idEmpresa = ItemE.idEmpresa;
                        ItemCc.claveCaracteristica = "COLOR";
                        ItemCc.descCaracteristica = "COLOR";
                        ItemCc.idUsuario_alta = User.idUsuario;
                        ItemCc.fecha_alta = DateTime.Now;
                        ItemCc.activo = true;
                        db.Caracteristica.Add(ItemCc);
                        db.SaveChanges();
                    }
                    else
                        ItemCc = QueryCc.ToList()[0];
                    var QueryAr = db.Articulo.Where("barcode = @0", Convert.ToDecimal(Dr["barcode"]));
                    if (QueryAr.ToList().Count == 0)
                    {
                        ItemAr.idArticulo = DatComun.GetId(1);
                        ItemAr.idEmpresa = ItemE.idEmpresa;
                        ItemAr.idProveedor = ItemP.idProveedor;
                        ItemAr.sku = Dr["sku"].ToString().Trim().ToUpper();
                        ItemAr.barcode = Convert.ToDecimal(Dr["barcode"]);
                        ItemAr.idTipoArticulo = ItemTa.idTipoArticulo;
                        ItemAr.idMarca = ItemM.idMarca;
                        ItemAr.idColeccion = ItemCo.idColeccion;
                        ItemAr.temporada = Convert.ToDecimal(Dr["temporada"]);
                        ItemAr.idCategoria = ItemCa.idCategoria;
                        ItemAr.idSubCategoria = ItemSc.idSubCategoria;
                        ItemAr.claveArticulo = Dr["claveArticulo"].ToString().Trim().ToUpper();
                        ItemAr.descArticulo = Dr["desc_articulo"].ToString().Trim().ToUpper();
                        ItemAr.nombreArticulo = Dr["nombreArticulo"].ToString().Trim().ToUpper();
                        ItemAr.claveColor = ItemC.claveColor;
                        ItemAr.claveTalla = ItemT.claveTalla;
                        ItemAr.idPresentacion = ItemPre.idPresentacion;
                        ItemAr.cantidadPresentacion = Convert.ToDecimal(Dr["cantidadPresentacion"]);
                        ItemAr.epcPrefijo = "EPC";
                        ItemAr.epcSerie = 1;
                        ItemAr.idUsuario_alta = User.idUsuario;
                        ItemAr.fecha_alta = DateTime.Now;
                        ItemAr.activo = true;
                        db.Articulo.Add(ItemAr);
                        db.SaveChanges();

                        ValorCaracteristica ItemVc = new ValorCaracteristica();
                        ItemVc.idValorCaracteristica = DatComun.GetId(1);
                        ItemVc.idCaracteristica = ItemCt.idCaracteristica;
                        ItemVc.valorCaracteristica1 = Dr["talla"].ToString().Trim().ToUpper();
                        ItemVc.idUsuario_alta = User.idUsuario;
                        ItemVc.fecha_alta = DateTime.Now;
                        ItemVc.activo = true;
                        db.ValorCaracteristica.Add(ItemVc);
                        db.SaveChanges();

                        ArticuloCaracteristica ItemAc = new ArticuloCaracteristica();
                        ItemAc.idArticulo = ItemAr.idArticulo;
                        ItemAc.idCaracteristica = ItemCt.idCaracteristica;
                        ItemAc.idValorCaracteristica = ItemVc.idValorCaracteristica;
                        ItemAc.idUsuario_alta = User.idUsuario;
                        ItemAc.fecha_alta = DateTime.Now;
                        ItemAc.activo = true;
                        db.ArticuloCaracteristica.Add(ItemAc);
                        db.SaveChanges();

                        ItemVc = new ValorCaracteristica();
                        ItemVc.idValorCaracteristica = DatComun.GetId(1);
                        ItemVc.idCaracteristica = ItemCt.idCaracteristica;
                        ItemVc.valorCaracteristica1 = Dr["color"].ToString().Trim().ToUpper();
                        ItemVc.idUsuario_alta = User.idUsuario;
                        ItemVc.fecha_alta = DateTime.Now;
                        ItemVc.activo = true;
                        db.ValorCaracteristica.Add(ItemVc);
                        db.SaveChanges();

                        ItemAc = new ArticuloCaracteristica();
                        ItemAc.idArticulo = ItemAr.idArticulo;
                        ItemAc.idCaracteristica = ItemCc.idCaracteristica;
                        ItemAc.idValorCaracteristica = ItemVc.idValorCaracteristica;
                        ItemAc.idUsuario_alta = User.idUsuario;
                        ItemAc.fecha_alta = DateTime.Now;
                        ItemAc.activo = true;
                        db.ArticuloCaracteristica.Add(ItemAc);
                        db.SaveChanges();
                    }
                }
                break;
            }
            return RedirectToAction("Index");
        }

        public ActionResult ExportToExcel()
        {
            var articulos = db.Articulo.ToList();
            var grid = new GridView();
            grid.DataSource = articulos;
            grid.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=MyExcelFile.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            grid.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            return RedirectToAction("Index"); 
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}