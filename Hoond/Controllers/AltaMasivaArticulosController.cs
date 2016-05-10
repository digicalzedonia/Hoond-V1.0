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
using Excel;
using System.Net;
using CaptchaMvc.HtmlHelpers;
using Archivos;
using System.Xml;
using System.Xml.Xsl;
using Hoond.Models;
using System.IO;



namespace Hoond.Controllers
{
    [Authorize]

    public class AltaMasivaArticulosController : Controller
    {
        private HoondDBEntities db = new HoondDBEntities();
        private Comun DatComun = new Comun();

        //
        // GET: /AltaMasivaArticulos/

        #region "Metodos"
        void ObtieneInfo()
        {
            //Tipo de Archivo.
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem
            {
                Text = "----",
                Value = "0",
                Selected = true
            });

            items.Add(new SelectListItem { Text = "txt", Value = "1" });

            items.Add(new SelectListItem { Text = "xls", Value = "2" });

            ViewBag.TipoArchivo = items;
            ViewBag.TipoCatDescarga = items;

          

        }

        private int? descarga(string TipoArchivo, int? accion)
        {


            if (TipoArchivo == "0")
            {
                //valida que se seleccione el tipo de archivo.
                ViewBag.error = app_GlobalResources.Content.errorSeleccionTipoArchivo;
                return 1;
            }
            else
            {
                //Procesa la informacion.
                //string ruta = "C:\\ArchivosTemporales\\Pruebas";

                Ruta R = db.Ruta.Find(1604061724504365);
                string ruta = Server.MapPath(R.direccion);

                if (!System.IO.Directory.Exists(ruta))
                {
                    System.IO.Directory.CreateDirectory(ruta);
                }

                var info_proveedor = (from art in db.Articulo
                                      select new
                                      {
                                         idProveedor = art.idProveedor,
                                         Proveedor = art.Proveedor.nombreProveedor,
                                         sku = art.sku,
                                         barcode = art.barcode,
                                         idTipoArticulo = art.idTipoArticulo,
                                         TipoArticulo = art.TipoArticulo.descTipoArticulo,
                                         idMarca = art.idMarca,
                                         Marca = art.Marca.nombreMarca,
                                         idColeccion = art.idColeccion,
                                         Coleccion = art.Coleccion.descColeccion,
                                         temporada = art.temporada,
                                         idCategoria = art.idCategoria,
                                         Categoria = art.Categoria.descCategoria,
                                         idSubCategoria = art.idSubCategoria,
                                         SubCategoria = art.SubCategoria.descSubCategoria,
                                         claveArticulo = art.claveArticulo,
                                         descArticulo = art.descArticulo,
                                         nombreArticulo	= art.nombreArticulo,
                                         claveColor = art.claveColor,
                                         claveTalla = art.claveTalla,
                                         idPresentacion = art.idPresentacion,
                                         Presentacion = art.Presentacion.descPresentacion,
                                         cantidadPresentacion = art.cantidadPresentacion

                                      });
                ArchivoData a = new ArchivoData();
                DataTable dt_proveedor = a.LINQResultToDataTable(info_proveedor);
                DataSet dts = new DataSet();
                dt_proveedor.TableName = "Articulo";
                dts.Tables.Add(dt_proveedor);
                TempData["dtsInfo"] = dts;

                if (accion == 1)
                {

                    if (TipoArchivo == "1")
                    {
                        //txt
                       
                       

                        //se crean los archivos txt
                        string rutaCompleta = Path.Combine(ruta,  dts.Tables[0].TableName + ".txt");
                        a.CreateTextDelimiterFile(rutaCompleta, dts.Tables[0], "|", true, false);

                      
                        //Se descarga
                        Response.Clear();
                        Response.ContentType = "application/octet-stream";
                        Response.Charset = "";
                        Response.AddHeader("Content-Disposition", "attachment; filename=FormatoCargaArticulos.txt");
                        Response.WriteFile(rutaCompleta );

                        Response.Flush();
                        Response.End();
                        GC.Collect();

                        System.IO.File.Delete(rutaCompleta + ".txt");
                       
                    }
                    else
                    {
                        string nomb = DateTime.Now.ToString("dMyyyy HHmmss ff");
                        string rutaCompleta = Path.Combine(ruta, "FormatoCargaArticulos" + nomb + ".xls");
                        ExcelData e = new ExcelData(rutaCompleta);

                        string xx = e.fncExcelExport(dts, rutaCompleta, "", 0, false);

                        Response.Clear();
                        Response.ContentType = "application/octet-stream";
                        Response.Charset = "";
                        Response.AddHeader("Content-Disposition", "attachment; filename=FormatoCargaArticulos.xls");
                        Response.WriteFile(rutaCompleta);

                        Response.Flush();
                        Response.End();
                        GC.Collect();

                        System.IO.File.Delete(rutaCompleta);
                    }
                }

            }
            return 0;
        }

        private DataTable ProcesaInfo(DataTable infomacionDeHoja,  Int16 accion)
        {
            UserLogin User = (UserLogin)Session["user"];

            ViewBag.errorVal = "0";

            DataSet infomacionGen = TempData["dtsInfo"] as DataSet;

            if (infomacionGen != null)
            {
                for (int c = 0; c <= infomacionGen.Tables[0].Columns.Count - 1; c++)
                {
                    if (!infomacionDeHoja.Columns.Contains(infomacionGen.Tables[0].Columns[c].ColumnName))
                    {
                        ViewBag.error = app_GlobalResources.Content.errorColumnasErroneas;
                        ViewBag.errorVal = "1";
                        return null;
                    }
                }
            }



            if (infomacionDeHoja.Columns.Contains("Accion") == false)
            {
                infomacionDeHoja.Columns.Add("Accion");
                infomacionDeHoja.Columns.Add("ide");
            }

         
                    //si accion =0, valida la informacion recibida como la existencia de los id
                    for (Int32 r = 0; r <= infomacionDeHoja.Rows.Count - 1; r++)
                    {
                        if (accion == 0) //valida informacion
                        {
                            int? val = 0;
                            //se valida si tiene toda la informacion necesaria
                            if (infomacionDeHoja.Rows[r]["idColeccion"].ToString() == "" && val == 0)
                            {
                                infomacionDeHoja.Rows[r]["Accion"] = string.Format(app_GlobalResources.Content.msgIDNulo, "idColeccion"); 
                                infomacionDeHoja.Rows[r]["ide"] = 0;
                                infomacionDeHoja.AcceptChanges();
                                val = 1;
                            }
                            if (infomacionDeHoja.Rows[r]["Temporada"].ToString() == "" && val == 0)
                            {
                                infomacionDeHoja.Rows[r]["Accion"] = string.Format(app_GlobalResources.Content.msgIDNulo, "Temporada"); 
                                infomacionDeHoja.Rows[r]["ide"] = 0;
                                infomacionDeHoja.AcceptChanges();
                                val = 1;
                            }
                            if (infomacionDeHoja.Rows[r]["idCategoria"].ToString() == "" && val == 0)
                            {
                                infomacionDeHoja.Rows[r]["Accion"] = string.Format(app_GlobalResources.Content.msgIDNulo, "idCategoria"); 
                                infomacionDeHoja.Rows[r]["ide"] = 0;
                                infomacionDeHoja.AcceptChanges();
                                val = 1;
                            }
                            if (infomacionDeHoja.Rows[r]["idSubCategoria"].ToString() == "" && val == 0)
                            {
                                infomacionDeHoja.Rows[r]["Accion"] = string.Format(app_GlobalResources.Content.msgIDNulo, "idSubCategoria"); 
                                infomacionDeHoja.Rows[r]["ide"] = 0;
                                infomacionDeHoja.AcceptChanges();
                                val = 1;
                            }
                            if (infomacionDeHoja.Rows[r]["claveArticulo"].ToString() == "" && val == 0)
                            {
                                infomacionDeHoja.Rows[r]["Accion"] = string.Format(app_GlobalResources.Content.msgIDNulo, "claveArticulo"); 
                                infomacionDeHoja.Rows[r]["ide"] = 0;
                                infomacionDeHoja.AcceptChanges();
                                val = 1;
                            }
                            if (infomacionDeHoja.Rows[r]["descArticulo"].ToString() == "" && val == 0)
                            {
                                infomacionDeHoja.Rows[r]["Accion"] = string.Format(app_GlobalResources.Content.msgIDNulo, "descArticulo"); 
                                infomacionDeHoja.Rows[r]["ide"] = 0;
                                infomacionDeHoja.AcceptChanges();
                                val = 1;
                            }
                            if (infomacionDeHoja.Rows[r]["nombreArticulo"].ToString() == "" && val == 0)
                            {
                                infomacionDeHoja.Rows[r]["Accion"] = string.Format(app_GlobalResources.Content.msgIDNulo, "nombreArticulo"); 
                                infomacionDeHoja.Rows[r]["ide"] = 0;
                                infomacionDeHoja.AcceptChanges();
                                val = 1;
                            }
                            if (infomacionDeHoja.Rows[r]["claveColor"].ToString() == "" && val == 0)
                            {
                                infomacionDeHoja.Rows[r]["Accion"] = string.Format(app_GlobalResources.Content.msgIDNulo, "claveColor"); 
                                infomacionDeHoja.Rows[r]["ide"] = 0;
                                infomacionDeHoja.AcceptChanges();
                                val = 1;
                            }
                            if (infomacionDeHoja.Rows[r]["claveTalla"].ToString() == "" && val == 0)
                            {
                                infomacionDeHoja.Rows[r]["Accion"] = string.Format(app_GlobalResources.Content.msgIDNulo, "claveTalla"); 
                                infomacionDeHoja.Rows[r]["ide"] = 0;
                                infomacionDeHoja.AcceptChanges();
                                val = 1;
                            }
                            if (infomacionDeHoja.Rows[r]["idPresentacion"].ToString() == "" && val == 0)
                            {
                                infomacionDeHoja.Rows[r]["Accion"] = string.Format(app_GlobalResources.Content.msgIDNulo, "idPresentacion"); 
                                infomacionDeHoja.Rows[r]["ide"] = 0;
                                infomacionDeHoja.AcceptChanges();
                                val = 1;
                            }
                            if (infomacionDeHoja.Rows[r]["cantidadPresentacion"].ToString() == "" && val == 0)
                            {
                                infomacionDeHoja.Rows[r]["Accion"] = string.Format(app_GlobalResources.Content.msgIDNulo, "cantidadPresentacion"); 
                                infomacionDeHoja.Rows[r]["ide"] = 0;
                                infomacionDeHoja.AcceptChanges();
                                val = 1;
                            }

                            if (infomacionDeHoja.Rows[r]["barcode"].ToString() == "" && val == 0)
                            {
                                infomacionDeHoja.Rows[r]["Accion"] = string.Format(app_GlobalResources.Content.msgIDNulo, "barcode");
                                infomacionDeHoja.Rows[r]["ide"] = 0;
                                infomacionDeHoja.AcceptChanges();
                                val = 1;
                            }

                            if (infomacionDeHoja.Rows[r]["sku"].ToString() == "" && val == 0)
                            {
                                infomacionDeHoja.Rows[r]["Accion"] = string.Format(app_GlobalResources.Content.msgIDNulo, "sku");
                                infomacionDeHoja.Rows[r]["ide"] = 0;
                                infomacionDeHoja.AcceptChanges();
                                val = 1;
                            }

                            if (infomacionDeHoja.Rows[r]["idProveedor"].ToString() == "" && val == 0)
                            {
                                infomacionDeHoja.Rows[r]["Accion"] = string.Format(app_GlobalResources.Content.msgIDNulo, "idProveedor");
                                infomacionDeHoja.Rows[r]["ide"] = 0;
                                infomacionDeHoja.AcceptChanges();
                                val = 1;
                            }

                            if (val == 0)
                            {
                                // se valida la existencia dentro de las tablas
                                
                               
                                Proveedor prov = db.Proveedor.Find(Convert.ToDecimal(infomacionDeHoja.Rows[r]["idProveedor"].ToString()));
                                if (prov == null && val == 0)
                                {
                                    infomacionDeHoja.Rows[r]["Accion"] = string.Format(app_GlobalResources.Content.msgIDNoExiste, "idProveedor");
                                    infomacionDeHoja.Rows[r]["ide"] = 0;
                                    infomacionDeHoja.AcceptChanges();
                                    val = 1;
                                }

                                TipoArticulo TArt = db.TipoArticulo.Find(Convert.ToDecimal(infomacionDeHoja.Rows[r]["idTipoArticulo"].ToString()));
                                if (TArt == null && val == 0)
                                {
                                    infomacionDeHoja.Rows[r]["Accion"] = string.Format(app_GlobalResources.Content.msgIDNoExiste, "idTipoArticulo"); 
                                    infomacionDeHoja.Rows[r]["ide"] = 0;
                                    infomacionDeHoja.AcceptChanges();
                                    val = 1;
                                }

                                //idMarca
                                Marca AMArca = db.Marca.Find(Convert.ToDecimal(infomacionDeHoja.Rows[r]["idMarca"].ToString()));
                                if (AMArca == null && val == 0)
                                {
                                    infomacionDeHoja.Rows[r]["Accion"] = string.Format(app_GlobalResources.Content.msgIDNoExiste, "idMarca"); 
                                    infomacionDeHoja.Rows[r]["ide"] = 0;
                                    infomacionDeHoja.AcceptChanges();
                                    val = 1;
                                }

                                //idColeccion	
                                Coleccion AColeccion = db.Coleccion.Find(Convert.ToDecimal(infomacionDeHoja.Rows[r]["idColeccion"].ToString()));
                                if (AColeccion == null && val == 0)
                                {
                                    infomacionDeHoja.Rows[r]["Accion"] = string.Format(app_GlobalResources.Content.msgIDNoExiste, "idColeccion"); 
                                    infomacionDeHoja.Rows[r]["ide"] = 0;
                                    infomacionDeHoja.AcceptChanges();
                                    val = 1;
                                }
                                //idCategoria
                                Categoria ACategoria = db.Categoria.Find(Convert.ToDecimal(infomacionDeHoja.Rows[r]["idCategoria"].ToString()));
                                if (ACategoria == null && val == 0)
                                {
                                    infomacionDeHoja.Rows[r]["Accion"] = string.Format(app_GlobalResources.Content.msgIDNoExiste, "idCategoria");
                                    infomacionDeHoja.Rows[r]["ide"] = 0;
                                    infomacionDeHoja.AcceptChanges();
                                    val = 1;
                                }

                                //idSubCategoria	
                                SubCategoria AidSubCategoria = db.SubCategoria.Find(Convert.ToDecimal(infomacionDeHoja.Rows[r]["idSubCategoria"].ToString()));
                                if (AidSubCategoria == null && val == 0)
                                {
                                    infomacionDeHoja.Rows[r]["Accion"] = string.Format(app_GlobalResources.Content.msgIDNoExiste, "idSubCategoria"); 
                                    infomacionDeHoja.Rows[r]["ide"] = 0;
                                    infomacionDeHoja.AcceptChanges();
                                    val = 1;
                                }

                                //idPresentacion
                                Presentacion AidPresentacion = db.Presentacion.Find(Convert.ToDecimal(infomacionDeHoja.Rows[r]["idPresentacion"].ToString()));
                                if (AidPresentacion == null && val == 0)
                                {
                                    infomacionDeHoja.Rows[r]["Accion"] = string.Format(app_GlobalResources.Content.msgIDNoExiste, "idPresentacion");
                                    infomacionDeHoja.Rows[r]["ide"] = 0;
                                    infomacionDeHoja.AcceptChanges();
                                    val = 1;
                                }

                               

                                

                            }

                            if (val == 0)
                            { 
                                //se validan la relacion entre tablas
                                decimal aidmarca = decimal.Parse(infomacionDeHoja.Rows[r]["idMarca"].ToString());
                                decimal atipoarticulo = decimal.Parse(infomacionDeHoja.Rows[r]["idTipoArticulo"].ToString());
                                
                                //se valida que el idmarca corresponda
                                var infoTipoA = (from TipoArticuloa in db.TipoArticulo
                                                 where TipoArticuloa.idMarca == aidmarca && TipoArticuloa.idTipoArticulo == atipoarticulo
                                                 select TipoArticuloa).ToList();

                                if (infoTipoA.Count == 0 )
                                {
                                    infomacionDeHoja.Rows[r]["Accion"] = string.Format(app_GlobalResources.Content.msgErrorRelacion, "TipoArticulo", "Marca");
                                    infomacionDeHoja.Rows[r]["ide"] = 0;
                                    infomacionDeHoja.AcceptChanges();
                                    val = 1;
                                }

                                decimal aidCategoria = decimal.Parse(infomacionDeHoja.Rows[r]["idCategoria"].ToString());
                                decimal aSubidCategoria = decimal.Parse(infomacionDeHoja.Rows[r]["idSubCategoria"].ToString());

                                var infoSubCat =  (from TipoCatg in db.SubCategoria
                                                   where TipoCatg.idCategoria == aidCategoria && TipoCatg.idSubCategoria == aSubidCategoria
                                                 select TipoCatg).ToList();

                                if (infoSubCat.Count == 0)
                                {
                                    infomacionDeHoja.Rows[r]["Accion"] = string.Format(app_GlobalResources.Content.msgErrorRelacion, "Categoria", "SubCategoria");
                                    infomacionDeHoja.Rows[r]["ide"] = 0;
                                    infomacionDeHoja.AcceptChanges();
                                    val = 1;
                                }


                            }

                            if (val == 0)
                            {
                                //se valida si se inserta o se actualiza.
                                //si exiet el proveedor, el barcode y empresa, entonces es modificacion si no es alta.
                                decimal aidproveedor = decimal.Parse(infomacionDeHoja.Rows[r]["idProveedor"].ToString());
                                decimal abarcode = decimal.Parse(infomacionDeHoja.Rows[r]["barcode"].ToString());


                                //se valida que el idmarca corresponda
                                var infoExistencia = (from TArticulo in db.Articulo
                                                      where TArticulo.idProveedor == aidproveedor && TArticulo.barcode == abarcode &&  TArticulo.idEmpresa == User.idEmpresa
                                                 select TArticulo).ToList();

                                if (infoExistencia.Count == 0)
                                {
                                    infomacionDeHoja.Rows[r]["Accion"] = app_GlobalResources.Content.msgAlta;
                                    infomacionDeHoja.Rows[r]["ide"] = 1;
                                    infomacionDeHoja.AcceptChanges();
                                }
                                else 
                                {
                                    infomacionDeHoja.Rows[r]["Accion"] = app_GlobalResources.Content.msgModificacion;
                                    infomacionDeHoja.Rows[r]["ide"] = 2;
                                    infomacionDeHoja.AcceptChanges();
                                }
                            }

                        }
                        else //guarda en la bd
                        {
                            if (infomacionDeHoja.Rows[r]["ide"].ToString() != "0")
                            {
                                if (infomacionDeHoja.Rows[r]["ide"].ToString() == "2")
                                {
                                    //modificacion

                                   // Articulo art = db.Articulo.Find(decimal.Parse(infomacionDeHoja.Rows[r]["idArticulo"].ToString()));
                                    decimal aidproveedor = decimal.Parse(infomacionDeHoja.Rows[r]["idProveedor"].ToString());
                                    decimal abarcode = decimal.Parse(infomacionDeHoja.Rows[r]["barcode"].ToString());
                                    Articulo art = (from TArticulo in db.Articulo
                                                          where TArticulo.idProveedor == aidproveedor && TArticulo.barcode == abarcode && TArticulo.idEmpresa == User.idEmpresa
                                                          select TArticulo).First();

                                  
                                    art.idProveedor = Convert.ToDecimal(infomacionDeHoja.Rows[r]["idProveedor"].ToString());
                                    art.sku = infomacionDeHoja.Rows[r]["sku"].ToString();
                                    art.barcode = Convert.ToDecimal(infomacionDeHoja.Rows[r]["barcode"].ToString());
                                    art.idTipoArticulo = Convert.ToDecimal(infomacionDeHoja.Rows[r]["idTipoArticulo"].ToString());

                                    art.idMarca = Convert.ToDecimal(infomacionDeHoja.Rows[r]["idMarca"].ToString());
                                    art.idColeccion = Convert.ToDecimal(infomacionDeHoja.Rows[r]["idColeccion"].ToString());
                                    art.temporada = Convert.ToDecimal(infomacionDeHoja.Rows[r]["temporada"].ToString());
                                    art.idCategoria = Convert.ToDecimal(infomacionDeHoja.Rows[r]["idCategoria"].ToString());
                                    art.idSubCategoria = Convert.ToDecimal(infomacionDeHoja.Rows[r]["idSubCategoria"].ToString());

                                    art.claveArticulo = infomacionDeHoja.Rows[r]["claveArticulo"].ToString();
                                    art.descArticulo = infomacionDeHoja.Rows[r]["descArticulo"].ToString();
                                    art.nombreArticulo = infomacionDeHoja.Rows[r]["nombreArticulo"].ToString();

                                    RFID EPC = new RFID();

                                    art.epcPrefijo = EPC.GenerateEPCfromBarCode(infomacionDeHoja.Rows[r]["barcode"].ToString(), "1");
                                    art.epcSerie = 1;

                                    art.claveColor = infomacionDeHoja.Rows[r]["claveColor"].ToString();
                                    art.claveTalla = infomacionDeHoja.Rows[r]["claveTalla"].ToString();
                                    art.idPresentacion = Convert.ToDecimal(infomacionDeHoja.Rows[r]["idPresentacion"].ToString());
                                    art.cantidadPresentacion = Convert.ToDecimal(infomacionDeHoja.Rows[r]["cantidadPresentacion"].ToString());
                                  
                                    db.Entry(art).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                                else
                                {
                                    //alta
                                    newID idarticulo = new newID();
                                    Articulo art = new Articulo();
                                    art.idArticulo = idarticulo.CalculaId(1);
                                    art.idProveedor = Convert.ToDecimal(infomacionDeHoja.Rows[r]["idProveedor"].ToString());
                                    art.sku = infomacionDeHoja.Rows[r]["sku"].ToString();
                                    art.barcode = Convert.ToDecimal(infomacionDeHoja.Rows[r]["barcode"].ToString());
                                    art.idTipoArticulo = Convert.ToDecimal(infomacionDeHoja.Rows[r]["idTipoArticulo"].ToString());

                                    art.idMarca = Convert.ToDecimal(infomacionDeHoja.Rows[r]["idMarca"].ToString());
                                    art.idColeccion = Convert.ToDecimal(infomacionDeHoja.Rows[r]["idColeccion"].ToString());
                                    art.temporada = Convert.ToDecimal(infomacionDeHoja.Rows[r]["temporada"].ToString());
                                    art.idCategoria = Convert.ToDecimal(infomacionDeHoja.Rows[r]["idCategoria"].ToString());
                                    art.idSubCategoria = Convert.ToDecimal(infomacionDeHoja.Rows[r]["idSubCategoria"].ToString());

                                    art.claveArticulo = infomacionDeHoja.Rows[r]["claveArticulo"].ToString();
                                    art.descArticulo = infomacionDeHoja.Rows[r]["descArticulo"].ToString();
                                    art.nombreArticulo = infomacionDeHoja.Rows[r]["nombreArticulo"].ToString();

                                    RFID EPC = new RFID();

                                    art.epcPrefijo = EPC.GenerateEPCfromBarCode(infomacionDeHoja.Rows[r]["barcode"].ToString(), "1");
                                    art.epcSerie = 1;

                                    art.claveColor = infomacionDeHoja.Rows[r]["claveColor"].ToString();
                                    art.claveTalla = infomacionDeHoja.Rows[r]["claveTalla"].ToString();
                                    art.idPresentacion = Convert.ToDecimal(infomacionDeHoja.Rows[r]["idPresentacion"].ToString());
                                    art.cantidadPresentacion = Convert.ToDecimal(infomacionDeHoja.Rows[r]["cantidadPresentacion"].ToString());

                                    art.idUsuario_alta = User.idUsuario;
                                    art.idEmpresa = User.idEmpresa;
                                    art.activo = true;
                                    art.fecha_alta = DateTime.Now;
                                    db.Articulo.Add(art);
                                    db.SaveChanges();
                                }


                            }

                        }

                    }

            return infomacionDeHoja;
        }

        #endregion




        public ActionResult Index(string cmbCampo, string cmbPaginado, string sortField, string txtBusqueda, int? page)
        {
            ObtieneInfo();
            return View();
        }

        //
        // GET: /AltaMasivaArticulos/Details/5



        public ActionResult Procesa()
        {
            var model = TempData["ModelName"] as DataTable; ;
            TempData["ModelName"] = model;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string cmdsubmit, string TipoArchivo, string TipoCatDescarga, HttpPostedFileBase files,  string listahojas, string ruta)
        {
            ViewBag.errorVal = 0;
            
            ObtieneInfo();

            if (!string.IsNullOrWhiteSpace(cmdsubmit))
            {
                //Descarga informacion
                int? aux = descarga(TipoCatDescarga, 1);
                if (aux == 1)
                { return View("index"); }
                else
                { return View(); }
            }
            else  //procesa archivos
            {
               

                if (TipoArchivo == "0")
                {
                    ViewBag.error = app_GlobalResources.Content.errorSeleccionTipoArchivo;
                    return View();
                }

                //lectura de archivos
                if ((files == null || files.ContentLength == 0) && listahojas == null)
                {
                    ViewBag.error = app_GlobalResources.Content.errorSeleccionArchivo;
                    return View();
                }
                else
                {
                    string nomb = DateTime.Now.ToString("dMyyyy HHmmss ff");
                    string path = "";

                    if (ruta != "")
                    {
                        path = ruta;
                        if (listahojas != null)
                        {
                            if (listahojas == "0")
                            {
                                ViewBag.error = app_GlobalResources.Content.errorSeleccionHoja;
                                return View();
                            }
                            //Procesa 
                            var excelData = new ExcelData(ruta);
                            DataTable infomacionDeHoja = excelData.getDataDT(listahojas);
                            descarga(TipoCatDescarga, 0);
                            DataTable dt = ProcesaInfo(infomacionDeHoja,  0);
                            DataTable dtAux = null;

                            if (dt != null)
                            {
                                //
                                ArchivoData a = new ArchivoData();
                                a.CreateTextDelimiterFile(path, dt, ",", true, true);
                                ViewBag.pathProcesa = path;
                                TempData["ModelName"] = dt;
                                dtAux = dt.Copy();

                                    if (dtAux.Columns.Contains("idProveedor"))
                                    { dtAux.Columns.Remove("idProveedor"); }

                                     if (dtAux.Columns.Contains("idProveedor"))
                                     { dtAux.Columns.Remove("idProveedor"); }

                                     if (dtAux.Columns.Contains("idMarca"))
                                     { dtAux.Columns.Remove("idMarca"); }

                                     if (dtAux.Columns.Contains("idColeccion"))
                                     { dtAux.Columns.Remove("idColeccion"); }

                                     if (dtAux.Columns.Contains("idTipoArticulo"))
                                     { dtAux.Columns.Remove("idTipoArticulo"); }

                                     if (dtAux.Columns.Contains("idCategoria"))
                                     { dtAux.Columns.Remove("idCategoria"); }

                                     if (dtAux.Columns.Contains("idSubCategoria"))
                                     { dtAux.Columns.Remove("idSubCategoria"); }

                                     if (dtAux.Columns.Contains("idPresentacion"))
                                     { dtAux.Columns.Remove("idPresentacion"); }


                                ViewBag.Nuevosdt = dt.Compute("count(ide)", "ide = '1'");
                                ViewBag.Actdt = dt.Compute("count(ide)", "ide = 2");
                                ViewBag.erroresdt = dt.Compute("count(ide)", "ide = 0");
                            }

                            return View(dtAux);
                        }
                    }
                    else
                    {

                        Ruta R = db.Ruta.Find(1604061725498493);

                        string rutabase = Server.MapPath(R.direccion);

                        if (!System.IO.Directory.Exists(rutabase))
                        {
                            System.IO.Directory.CreateDirectory(rutabase);
                        }

                        string nombreCompleto = nomb + Path.GetFileName(files.FileName);
                        path = System.IO.Path.Combine(rutabase, nombreCompleto);

                        //string nombreCompleto = nomb + Path.GetFileName(files.FileName);
                        //path = System.IO.Path.Combine(Server.MapPath("~/Content/"), nombreCompleto);

                        if (files.FileName.EndsWith("txt"))
                        {
                            if (System.IO.File.Exists(path))
                                System.IO.File.Delete(path);
                            files.SaveAs(path);

                            ViewBag.pathProcesa = path;

                            
                            ArchivoData a = new ArchivoData();
                            DataTable archivosData = a.lee_archivo_stream_separador(path, '|', true);
                            DataTable dt = ProcesaInfo(archivosData, 0);
                            TempData["ModelName"] = dt;

                             DataTable dtAux  = dt.Copy();

                            if (dtAux.Columns.Contains("idProveedor"))
                            { dtAux.Columns.Remove("idProveedor"); }

                            if (dtAux.Columns.Contains("idProveedor"))
                            { dtAux.Columns.Remove("idProveedor"); }

                            if (dtAux.Columns.Contains("idMarca"))
                            { dtAux.Columns.Remove("idMarca"); }

                            if (dtAux.Columns.Contains("idColeccion"))
                            { dtAux.Columns.Remove("idColeccion"); }

                            if (dtAux.Columns.Contains("idTipoArticulo"))
                            { dtAux.Columns.Remove("idTipoArticulo"); }

                            if (dtAux.Columns.Contains("idCategoria"))
                            { dtAux.Columns.Remove("idCategoria"); }

                            if (dtAux.Columns.Contains("idSubCategoria"))
                            { dtAux.Columns.Remove("idSubCategoria"); }

                            if (dtAux.Columns.Contains("idPresentacion"))
                            { dtAux.Columns.Remove("idPresentacion"); }
                          
                            ViewBag.Nuevosdt = dt.Compute("count(ide)", "ide = 1");
                            ViewBag.Actdt = dt.Compute("count(ide)", "ide = 2");
                            ViewBag.erroresdt = dt.Compute("count(ide)", "ide = 0");

                            View(dtAux);
                        }
                        if (files.FileName.EndsWith("xls") || files.FileName.EndsWith("xlsx"))
                        {
                            path = System.IO.Path.Combine(Server.MapPath("~/Content/"), nombreCompleto);
                            if (System.IO.File.Exists(path))
                                System.IO.File.Delete(path);
                            files.SaveAs(path);

                            ViewBag.pathProcesa = path;
                         
                            var excelData = new ExcelData(path);
                            var listahojas_c = excelData.getWorksheetNames();
                            SelectList custList = new SelectList(listahojas_c);
                            ViewBag.listahojas = custList;

                            return View();
                        }

                    }
                }
            }

            return View();
        }


        [HttpPost]
        public ActionResult ValidaInfo(string Catalogo)
        {
            var model = TempData["ModelName"] as DataTable; ;
            //DataTable dt = ProcesaInfo(model, Catalogo, 1);
            ViewBag.Catalogo = Catalogo;
            Session["Catalogo"] = Catalogo;
            TempData["ModelName"] = model;
            return RedirectToAction("Procesa");
        }


        [HttpPost]
        public ActionResult Procesa(string Catalogo)
        {
            ObtieneInfo();
            ViewBag.errorVal = 0;

            ViewBag.TerminoProceso = "0";
            var model = TempData["ModelName"] as DataTable; ;

            if (this.IsCaptchaValid("Captcha is not valid"))
            {
                //  return View("Index");
                DataTable dt = ProcesaInfo(model, 1);
                ViewBag.ErrMessage = "";
                ViewBag.TerminoProceso = "1";
                return View();
            }

            TempData["ModelName"] = model;
            ViewBag.ErrMessage = "Error: captcha no es valido.";
            return View();


        }

    }
}