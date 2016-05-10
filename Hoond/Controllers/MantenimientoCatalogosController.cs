using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hoond.Models;
using System.Web.UI.WebControls;
using Hoond.app_GlobalResources;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.IO;
using Archivos;
using System.Xml;
using System.Xml.Xsl;
using Excel;
using System.Net;
using CaptchaMvc.HtmlHelpers;


namespace Hoond.Controllers
{
    [Authorize]

    public class MantenimientoCatalogosController : Controller
    {
        private HoondDBEntities db = new HoondDBEntities();
        private Comun DatComun = new Comun();



 #region "Metodos"
        void ObtieneInfo()
        {
            //Tipo de Archivo.
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem
            {
                Text = "------",
                Value = "0",
                Selected = true
            });

            items.Add(new SelectListItem { Text = "txt", Value = "1" });

            items.Add(new SelectListItem { Text = "xls", Value = "2" });

            ViewBag.TipoArchivo = items;
            ViewBag.TipoCatDescarga = items;

            //Tipo de Archivo.
            List<SelectListItem> itemsCat = new List<SelectListItem>();

            itemsCat.Add(new SelectListItem
            {
                Text = "------",
                Value = "0",
                Selected = true
            });

            itemsCat.Add(new SelectListItem { Text = "Proveedor", Value = "Proveedor" });
            itemsCat.Add(new SelectListItem { Text = "Marca", Value = "Marca" });
            itemsCat.Add(new SelectListItem { Text = "Coleccion", Value = "Coleccion" });
            itemsCat.Add(new SelectListItem { Text = "TipoArticulo", Value = "TipoArticulo" });
            itemsCat.Add(new SelectListItem { Text = "Categoria", Value = "Categoria" });
            itemsCat.Add(new SelectListItem { Text = "Subcategoria", Value = "Subcategoria" });
            itemsCat.Add(new SelectListItem { Text = "Presentacion", Value = "Presentacion" });
            itemsCat.Add(new SelectListItem { Text = "Color", Value = "Color" });
            itemsCat.Add(new SelectListItem { Text = "Talla", Value = "Talla" });


            ViewBag.TipoCatalogo = itemsCat;
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

                Ruta R = db.Ruta.Find(1604061724504365); 
                

                //Procesa la informacion.
                //string ruta = "C:\\ArchivosTemporales\\Pruebas";
                string ruta = Server.MapPath(R.direccion);

                if (!System.IO.Directory.Exists(ruta))
                {
                    System.IO.Directory.CreateDirectory(ruta);
                }

                var info_proveedor = (from prov in db.Proveedor
                                      select new
                                      {
                                          idProveedor = prov.idProveedor,
                                          nombreProveedor = prov.nombreProveedor
                                      });

                var info_marca = (from marcas in db.Marca
                                  select new
                                  {
                                      idMarca = marcas.idMarca,
                                      idProveedor = marcas.idProveedor,
                                      nombreProveedor = marcas.Proveedor.nombreProveedor,
                                      nombreMarca = marcas.nombreMarca,
                                  });

                var info_Coleccion = (from Coleccio in db.Coleccion
                                      select new
                                      {
                                          idColeccion = Coleccio.idColeccion,
                                          idMarca = Coleccio.idMarca,
                                          nombreMarca = Coleccio.Marca.nombreMarca,
                                          descColeccion = Coleccio.descColeccion,
                                      });


                var info_TipoArticulo = (from TipoA in db.TipoArticulo
                                         select new
                                         {
                                             idTipoArticulo = TipoA.idTipoArticulo,
                                             idMarca = TipoA.idMarca,
                                             nombreMarca = TipoA.Marca.nombreMarca,
                                             descTipoArticulo = TipoA.descTipoArticulo,
                                         });

                var info_Categoria = (from Categorias in db.Categoria
                                      select new
                                      {
                                          idCategoria = Categorias.idCategoria,
                                          idMarca = Categorias.TipoArticulo.Marca.idMarca,
                                          nombreMarca = Categorias.TipoArticulo.Marca.nombreMarca,
                                          idTipoArticulo = Categorias.idTipoArticulo,
                                          descTipoArticulo = Categorias.TipoArticulo.descTipoArticulo,
                                          descCategoria = Categorias.descCategoria,
                                      });

                var info_Subcategoria = (from SubCategorias in db.SubCategoria
                                         select new
                                         {
                                             idSubCategoria = SubCategorias.idSubCategoria,
                                             idMarca = SubCategorias.Categoria.TipoArticulo.Marca.idMarca,
                                             nombreMarca = SubCategorias.Categoria.TipoArticulo.Marca.nombreMarca,
                                             idTipoArticulo = SubCategorias.Categoria.TipoArticulo.idTipoArticulo,
                                             descTipoArticulo = SubCategorias.Categoria.TipoArticulo.descTipoArticulo,
                                             idCategoria = SubCategorias.idCategoria,
                                             descCategoria = SubCategorias.Categoria.descCategoria,
                                             descSubCategoria = SubCategorias.descSubCategoria,
                                         });

                var info_Presentacion = (from Pres in db.Presentacion
                                         select new
                                         {
                                             idPresentacion = Pres.idPresentacion,
                                             idMarca = Pres.TipoArticulo.Marca.idMarca,
                                             nombreMarca = Pres.TipoArticulo.Marca.nombreMarca,
                                             idTipoArticulo = Pres.idTipoArticulo,
                                             descTipoArticulo = Pres.TipoArticulo.descTipoArticulo,
                                             descPresentacion = Pres.descPresentacion,
                                         });

                var info_Color = (from colos in db.Color
                                  select new
                                  {
                                      claveColor = colos.claveColor,
                                      idMarca = colos.TipoArticulo.Marca.idMarca,
                                      nombreMarca = colos.TipoArticulo.Marca.nombreMarca,
                                      idTipoArticulo = colos.idTipoArticulo,
                                      descTipoArticulo = colos.TipoArticulo.descTipoArticulo,
                                      descColor = colos.descColor,
                                  });

                var info_Talla = (from tallas in db.Talla
                                  select new
                                  {
                                      claveTalla = tallas.claveTalla,
                                      idMarca = tallas.TipoArticulo.Marca.idMarca,
                                      nombreMarca = tallas.TipoArticulo.Marca.nombreMarca,
                                      idTipoArticulo = tallas.idTipoArticulo,
                                      descTipoArticulo = tallas.TipoArticulo.descTipoArticulo,
                                      descTalla = tallas.descTalla,
                                      posicion = tallas.posicion,
                                  });

                ArchivoData a = new ArchivoData();


                DataTable dt_proveedor = a.LINQResultToDataTable(info_proveedor);
                DataTable dt_marca = a.LINQResultToDataTable(info_marca);
                DataTable dt_Coleccion = a.LINQResultToDataTable(info_Coleccion);
                DataTable dt_TipoArticulo = a.LINQResultToDataTable(info_TipoArticulo);
                DataTable dt_Categoria = a.LINQResultToDataTable(info_Categoria);
                DataTable dt_Subcategoria = a.LINQResultToDataTable(info_Subcategoria);
                DataTable dt_Presentacion = a.LINQResultToDataTable(info_Presentacion);
                DataTable dt_Color = a.LINQResultToDataTable(info_Color);
                DataTable dt_Talla = a.LINQResultToDataTable(info_Talla);

                DataSet dts = new DataSet();


                dt_proveedor.TableName = "Proveedor";
                dt_marca.TableName = "Marca";
                dt_Coleccion.TableName = "Coleccion";
                dt_TipoArticulo.TableName = "TipoArticulo";
                dt_Categoria.TableName = "Categoria";
                dt_Subcategoria.TableName = "Subcategoria";
                dt_Presentacion.TableName = "Presentacion";
                dt_Color.TableName = "Color";
                dt_Talla.TableName = "Talla";

                dts.Tables.Add(dt_proveedor);
                dts.Tables.Add(dt_marca);
                dts.Tables.Add(dt_Coleccion);
                dts.Tables.Add(dt_TipoArticulo);
                dts.Tables.Add(dt_Categoria);
                dts.Tables.Add(dt_Subcategoria);
                dts.Tables.Add(dt_Presentacion);
                dts.Tables.Add(dt_Color);
                dts.Tables.Add(dt_Talla);

                TempData["dtsInfo"] = dts;

                if (accion == 1)
                {

                    if (TipoArchivo == "1")
                    {
                        //txt
                        string carpeta = "Catalogos" + DateTime.Now.ToString("dMyyyy HHmmss ff");

                        string rutaCarpeta = Path.Combine(ruta, carpeta);
                        if (System.IO.Directory.Exists(rutaCarpeta))
                        {
                            System.IO.Directory.Delete(rutaCarpeta);
                        }
                        System.IO.Directory.CreateDirectory(rutaCarpeta);

                        //se crean los archivos txt
                        for (int i = 0; i <= dts.Tables.Count - 1; i++)
                        {

                            string rutaCompleta = Path.Combine(ruta, carpeta + "//" + dts.Tables[i].TableName + ".txt");
                            a.CreateTextDelimiterFile(rutaCompleta, dts.Tables[i], "|", true, false);

                        }


                        //se zip la informacion
                        a.Comprime(rutaCarpeta, rutaCarpeta + ".zip");
                        //Se descarga
                        Response.Clear();
                        Response.ContentType = "application/octet-stream";
                        Response.Charset = "";
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + carpeta + ".zip");
                        Response.WriteFile(rutaCarpeta + ".zip");

                        Response.Flush();
                        Response.End();
                        GC.Collect();

                        System.IO.File.Delete(rutaCarpeta + ".zip");
                        System.IO.Directory.Delete(rutaCarpeta, true);
                    }
                    else
                    {
                        string nomb = DateTime.Now.ToString("dMyyyy HHmmss ff");
                        string rutaCompleta = Path.Combine(ruta, "Catalogos_" + nomb + ".xls");
                        ExcelData e = new ExcelData(rutaCompleta);

                        string xx = e.fncExcelExport(dts, rutaCompleta, "", 0, false);

                        Response.Clear();
                        Response.ContentType = "application/octet-stream";
                        Response.Charset = "";
                        Response.AddHeader("Content-Disposition", "attachment; filename=Catalogos.xls");
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

        private DataTable ProcesaInfo(DataTable infomacionDeHoja, string Catalogo, Int16 accion)
        {
            UserLogin User = (UserLogin)Session["user"];

            ViewBag.errorVal = "0";

            DataSet infomacionGen = TempData["dtsInfo"] as DataSet;

            if (Catalogo == "0")
            {
                ViewBag.error = app_GlobalResources.Content.errorSeleccionCatalogo;
                ViewBag.errorVal = "1";
                return null;
            }

            if (infomacionGen != null)
            {
                for (int c = 0; c <= infomacionGen.Tables[Catalogo].Columns.Count - 1; c++)
                {
                    if (!infomacionDeHoja.Columns.Contains(infomacionGen.Tables[Catalogo].Columns[c].ColumnName))
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

            switch (Catalogo)
            {
                case "Proveedor":


                    //si accion =0, valida la informacion recibida como la existencia de los id
                    for (Int32 r = 0; r <= infomacionDeHoja.Rows.Count - 1; r++)
                    {
                        if (accion == 0) //valida informacion
                        {
                            if (infomacionDeHoja.Columns.Contains("idProveedor"))
                            {
                                if (infomacionDeHoja.Rows[r]["idProveedor"].ToString() != "")
                                {
                                    if (infomacionDeHoja.Rows[r]["nombreProveedor"].ToString() == "")
                                    {
                                        infomacionDeHoja.Rows[r]["Accion"] = app_GlobalResources.Content.msgProveedorNulo;
                                        infomacionDeHoja.Rows[r]["ide"] = 0;
                                        infomacionDeHoja.AcceptChanges();
                                    }
                                    else
                                    {
                                        Proveedor prov = db.Proveedor.Find(decimal.Parse(infomacionDeHoja.Rows[r]["idProveedor"].ToString()));
                                        if (prov == null)
                                        {
                                            infomacionDeHoja.Rows[r]["Accion"] = string.Format(app_GlobalResources.Content.msgProveedorNulo, "idProveedor");
                                            infomacionDeHoja.Rows[r]["ide"] = 0;
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
                                else
                                {
                                    infomacionDeHoja.Rows[r]["Accion"] = app_GlobalResources.Content.msgAlta;
                                    infomacionDeHoja.Rows[r]["ide"] = 1;
                                    infomacionDeHoja.AcceptChanges();
                                }
                            }
                            else
                            {
                                infomacionDeHoja.Rows[r]["Accion"]= app_GlobalResources.Content.msgAlta;
                                infomacionDeHoja.Rows[r]["ide"] = 1;
                                infomacionDeHoja.AcceptChanges();
                            }
                        }
                        else //guarda en la bd
                        {
                            if (infomacionDeHoja.Rows[r]["ide"].ToString() != "0")
                            {
                                if (infomacionDeHoja.Rows[r]["ide"].ToString() == "2")
                                {
                                    //modificacion

                                    Proveedor prov = db.Proveedor.Find(decimal.Parse(infomacionDeHoja.Rows[r]["idProveedor"].ToString()));

                                    prov.nombreProveedor = infomacionDeHoja.Rows[r]["nombreProveedor"].ToString();
                                    db.Entry(prov).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                                else
                                {
                                    //alta
                                    newID id_proveedor = new newID();
                                    Proveedor prov = new Proveedor();
                                    prov.idProveedor = id_proveedor.CalculaId(1);
                                    prov.nombreProveedor = infomacionDeHoja.Rows[r]["nombreProveedor"].ToString();
                                    prov.idUsuario_alta = User.idUsuario; ;
                                    prov.idEmpresa = User.idEmpresa;
                                    prov.activo = true;
                                    prov.fecha_alta = DateTime.Now;
                                    db.Proveedor.Add(prov);
                                    db.SaveChanges();
                                }
                            }

                        }

                    }



                    break;
                case "Marca":

                    for (Int32 r = 0; r <= infomacionDeHoja.Rows.Count - 1; r++)
                    {
                        if (accion == 0)
                        {
                          
                                    if (infomacionDeHoja.Rows[r]["idProveedor"].ToString() == "")
                                    {
                                        infomacionDeHoja.Rows[r]["Accion"] = string.Format(app_GlobalResources.Content.msgProveedorNulo, "idProveedor");
                                        infomacionDeHoja.Rows[r]["ide"] = 0;
                                        infomacionDeHoja.AcceptChanges();
                                    }
                                    else
                                    {
                                        if (infomacionDeHoja.Rows[r]["idMarca"].ToString() != "")
                                        {
                                            Marca marcas = db.Marca.Find(decimal.Parse(infomacionDeHoja.Rows[r]["idMarca"].ToString()));
                                            Proveedor prov = db.Proveedor.Find(decimal.Parse(infomacionDeHoja.Rows[r]["idProveedor"].ToString()));

                                            if (marcas == null)
                                            {
                                                infomacionDeHoja.Rows[r]["Accion"] = string.Format(app_GlobalResources.Content.msgIDNoExiste, "idMarca");
                                                infomacionDeHoja.Rows[r]["ide"] = 0;
                                                infomacionDeHoja.AcceptChanges();
                                            }
                                            else
                                            {
                                                if (prov == null)
                                                {
                                                    infomacionDeHoja.Rows[r]["Accion"] = string.Format(app_GlobalResources.Content.msgIDNoExiste, "idProveedor");
                                                    infomacionDeHoja.Rows[r]["ide"] = 0;
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
                                        else 
                                        {
                                            infomacionDeHoja.Rows[r]["Accion"] = app_GlobalResources.Content.msgAlta;
                                            infomacionDeHoja.Rows[r]["ide"] = 1;
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
                                    Marca marca_A = db.Marca.Find(decimal.Parse(infomacionDeHoja.Rows[r]["idMarca"].ToString()));
                                    marca_A.nombreMarca = infomacionDeHoja.Rows[r]["nombreMarca"].ToString();
                                    db.Entry(marca_A).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                                else
                                {
                                    //alta

                                    newID idmarca = new newID();
                                    Marca marca_A = new Marca();

                                    if (infomacionDeHoja.Rows[r]["ide"].ToString() == "1")
                                    {
                                        marca_A.idMarca = idmarca.CalculaId(1);
                                    }
                                    else
                                    {
                                        marca_A.idMarca = Convert.ToDecimal(infomacionDeHoja.Rows[r]["idMarca"].ToString());
                                    }
                                  
                                    marca_A.nombreMarca = infomacionDeHoja.Rows[r]["nombreMarca"].ToString();
                                    marca_A.idProveedor = Convert.ToDecimal(infomacionDeHoja.Rows[r]["idProveedor"].ToString());
                                    marca_A.idUsuario_alta = User.idUsuario;
                                    marca_A.idEmpresa = User.idEmpresa;
                                    marca_A.activo = true;
                                    marca_A.fecha_alta = DateTime.Now;
                                    db.Marca.Add(marca_A);
                                    db.SaveChanges();
                                }
                            }

                        }
                    }

                    break;
                case "Coleccion":

                    for (Int32 r = 0; r <= infomacionDeHoja.Rows.Count - 1; r++)
                    {
                        if (accion == 0)
                        {
                              if (infomacionDeHoja.Rows[r]["idMarca"].ToString() == "" || infomacionDeHoja.Rows[r]["descColeccion"].ToString() == "")
                                    {
                                        infomacionDeHoja.Rows[r]["Accion"] = infomacionDeHoja.Rows[r]["idMarca"].ToString() == "" ? string.Format(app_GlobalResources.Content.msgIDNulo, "idMarca") :string.Format(app_GlobalResources.Content.msgIDNulo, "descColeccion");
                                        infomacionDeHoja.Rows[r]["ide"] = 0;
                                        infomacionDeHoja.AcceptChanges();
                                    }
                                else 
                                    {
                                      
                                      decimal idMarcaa = decimal.Parse(infomacionDeHoja.Rows[r]["idMarca"].ToString());
                                  
                                      Marca marcas = db.Marca.Find(decimal.Parse(infomacionDeHoja.Rows[r]["idMarca"].ToString()));

                                                    if (marcas == null)
                                                    {
                                                        infomacionDeHoja.Rows[r]["Accion"] = string.Format(app_GlobalResources.Content.msgIDNoExiste, "idMarca"); ;
                                                        infomacionDeHoja.Rows[r]["ide"] = 0;
                                                        infomacionDeHoja.AcceptChanges();
                                                    }
                                                    else
                                                    {
                                                        if (infomacionDeHoja.Rows[r]["idColeccion"].ToString() == "")
                                                        {
                                                            infomacionDeHoja.Rows[r]["Accion"]= app_GlobalResources.Content.msgAlta;
                                                            infomacionDeHoja.Rows[r]["ide"] = 1;
                                                            infomacionDeHoja.AcceptChanges();
                                                        }
                                                        else 
                                                        {
                                                            Coleccion coleccion = db.Coleccion.Find(decimal.Parse(infomacionDeHoja.Rows[r]["idColeccion"].ToString()));
                                                            if (coleccion == null)
                                                            {
                                                                infomacionDeHoja.Rows[r]["Accion"] = string.Format(app_GlobalResources.Content.msgIDNoExiste, "idColeccion");
                                                                infomacionDeHoja.Rows[r]["ide"] = 0;
                                                                infomacionDeHoja.AcceptChanges();
                                                            }
                                                            else
                                                            {
                                                                infomacionDeHoja.Rows[r]["Accion"]= app_GlobalResources.Content.msgModificacion;
                                                                infomacionDeHoja.Rows[r]["ide"] = 2;
                                                                infomacionDeHoja.AcceptChanges();
                                                            }
                                                            
                                                        }
                                                     
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
                                    Coleccion coleccion_A = db.Coleccion.Find(decimal.Parse(infomacionDeHoja.Rows[r]["idColeccion"].ToString()));
                                    coleccion_A.descColeccion = infomacionDeHoja.Rows[r]["descColeccion"].ToString();
                                    coleccion_A.idMarca = Convert.ToDecimal(infomacionDeHoja.Rows[r]["idMarca"].ToString());
                                    db.Entry(coleccion_A).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                                else
                                {
                                    //alta
                                    newID idmarca = new newID();
                                    Coleccion coleccion_A = new Coleccion();
                                    if (infomacionDeHoja.Rows[r]["ide"].ToString() == "1")
                                    {
                                        coleccion_A.idColeccion = idmarca.CalculaId(1);
                                    }
                                    else 
                                    {
                                        coleccion_A.idColeccion = Convert.ToDecimal(infomacionDeHoja.Rows[r]["idColeccion"].ToString());
                                    }
                                    
                                    coleccion_A.descColeccion = infomacionDeHoja.Rows[r]["descColeccion"].ToString();
                                    coleccion_A.idMarca = Convert.ToDecimal(infomacionDeHoja.Rows[r]["idMarca"].ToString());
                                    coleccion_A.idUsuario_alta = User.idUsuario;
                                    coleccion_A.idEmpresa = User.idEmpresa;
                                    coleccion_A.activo = true;
                                    coleccion_A.fecha_alta = DateTime.Now;
                                    db.Coleccion.Add(coleccion_A);
                                    db.SaveChanges();
                                }
                            }

                        }
                    }

                    break;
                case "TipoArticulo":

                    for (Int32 r = 0; r <= infomacionDeHoja.Rows.Count - 1; r++)
                    {
                        if (accion == 0)
                        {
                          if (infomacionDeHoja.Rows[r]["idMarca"].ToString() == "" || infomacionDeHoja.Rows[r]["descTipoArticulo"].ToString() == "")
                             {
                                        infomacionDeHoja.Rows[r]["Accion"] = infomacionDeHoja.Rows[r]["idMarca"].ToString() == "" ?string.Format(app_GlobalResources.Content.msgIDNulo, "idMarca")  :string.Format(app_GlobalResources.Content.msgIDNulo, "descTipoArticulo") ;
                                        infomacionDeHoja.Rows[r]["ide"] = 0;
                                        infomacionDeHoja.AcceptChanges();
                           }
                         else 
                           {

                              Marca marcas = db.Marca.Find(decimal.Parse(infomacionDeHoja.Rows[r]["idMarca"].ToString()));

                              if (marcas == null)
                                  {
                                      infomacionDeHoja.Rows[r]["Accion"] = string.Format(app_GlobalResources.Content.msgIDNoExiste, "idMarca");
                                      infomacionDeHoja.Rows[r]["ide"] = 0;
                                      infomacionDeHoja.AcceptChanges();      
                                  }
                               else
                                   {
                                           decimal aidmarca = decimal.Parse(infomacionDeHoja.Rows[r]["idMarca"].ToString());
                                          
                                               if (infomacionDeHoja.Rows[r]["idTipoArticulo"].ToString() == "")
                                               {
                                                   infomacionDeHoja.Rows[r]["Accion"]= app_GlobalResources.Content.msgAlta;
                                                   infomacionDeHoja.Rows[r]["ide"] = 1;
                                                   infomacionDeHoja.AcceptChanges();
                                               }
                                               else 
                                               {
                                                   TipoArticulo TipoA = db.TipoArticulo.Find(decimal.Parse(infomacionDeHoja.Rows[r]["idTipoArticulo"].ToString()));
                                                   if (TipoA == null)
                                                   {

                                                       infomacionDeHoja.Rows[r]["Accion"] = string.Format(app_GlobalResources.Content.msgIDNoExiste, "idTipoArticulo");
                                                       infomacionDeHoja.Rows[r]["ide"] = 0;
                                                       infomacionDeHoja.AcceptChanges();
                                                   }
                                                   else
                                                   {
                                                       infomacionDeHoja.Rows[r]["Accion"]= app_GlobalResources.Content.msgModificacion;
                                                       infomacionDeHoja.Rows[r]["ide"] = 2;
                                                       infomacionDeHoja.AcceptChanges();
                                                   }
                                               } 
                                           
                                           
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
                                    TipoArticulo TipoArticulo_A = db.TipoArticulo.Find(decimal.Parse(infomacionDeHoja.Rows[r]["idTipoArticulo"].ToString()));
                                    TipoArticulo_A.descTipoArticulo = infomacionDeHoja.Rows[r]["descTipoArticulo"].ToString();
                                    TipoArticulo_A.idMarca = Convert.ToDecimal(infomacionDeHoja.Rows[r]["idMarca"].ToString());
                                    db.Entry(TipoArticulo_A).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                                else
                                {
                                    //alta
                                    newID idmarca = new newID();
                                    TipoArticulo TipoArticulo_A = new TipoArticulo();
                                    if (infomacionDeHoja.Rows[r]["ide"].ToString() == "1")
                                    { TipoArticulo_A.idTipoArticulo = idmarca.CalculaId(1); }
                                    else
                                    { TipoArticulo_A.idTipoArticulo = Convert.ToDecimal(infomacionDeHoja.Rows[r]["idTipoArticulo"].ToString()); }
                                 
                                    TipoArticulo_A.descTipoArticulo = infomacionDeHoja.Rows[r]["descTipoArticulo"].ToString();
                                    TipoArticulo_A.idMarca = Convert.ToDecimal(infomacionDeHoja.Rows[r]["idMarca"].ToString());
                                    TipoArticulo_A.idUsuario_alta = User.idUsuario;
                                    TipoArticulo_A.idEmpresa = User.idEmpresa;
                                    TipoArticulo_A.activo = true;
                                    TipoArticulo_A.fecha_alta = DateTime.Now;
                                    db.TipoArticulo.Add(TipoArticulo_A);
                                    db.SaveChanges();
                                }
                            }

                        }
                    }

                    break;
                case "Categoria":

                    for (Int32 r = 0; r <= infomacionDeHoja.Rows.Count - 1; r++)
                    {
                        if (accion == 0)
                        {
                            
                                    if (infomacionDeHoja.Rows[r]["idTipoArticulo"].ToString() == "" || infomacionDeHoja.Rows[r]["idMarca"].ToString() == "" || infomacionDeHoja.Rows[r]["descCategoria"].ToString() == "")
                                    {
                                        infomacionDeHoja.Rows[r]["Accion"] = infomacionDeHoja.Rows[r]["idTipoArticulo"].ToString() == "" ? string.Format(app_GlobalResources.Content.msgIDNulo, "idTipoArticulo")  : infomacionDeHoja.Rows[r]["idMarca"].ToString() == "" ? string.Format(app_GlobalResources.Content.msgIDNulo, "idMarca"): string.Format(app_GlobalResources.Content.msgIDNulo, "descCategoria") ;
                                        infomacionDeHoja.Rows[r]["ide"] = 0;
                                        infomacionDeHoja.AcceptChanges();
                                    }
                                    else 
                                    {
                                        TipoArticulo TipoA = db.TipoArticulo.Find(decimal.Parse(infomacionDeHoja.Rows[r]["idTipoArticulo"].ToString()));
                                        Marca marcas = db.Marca.Find(decimal.Parse(infomacionDeHoja.Rows[r]["idMarca"].ToString()));

                                        if (TipoA == null || marcas == null)
                                        {
                                            infomacionDeHoja.Rows[r]["Accion"] = TipoA == null ? string.Format(app_GlobalResources.Content.msgIDNoExiste, "idTipoArticulo") : string.Format(app_GlobalResources.Content.msgIDNoExiste, "idMarca"); ;
                                            infomacionDeHoja.Rows[r]["ide"] = 0;
                                            infomacionDeHoja.AcceptChanges();
                                        }
                                        else
                                        {
                                           decimal aidmarca = decimal.Parse(infomacionDeHoja.Rows[r]["idMarca"].ToString());
                                           decimal atipoarticulo = decimal.Parse(infomacionDeHoja.Rows[r]["idTipoArticulo"].ToString());
                                           
                                                //se valida que el idmarca corresponda
                                           var infoTipoA = (from TipoArticuloa in db.TipoArticulo
                                                                 where TipoArticuloa.idMarca == aidmarca && TipoArticuloa.idTipoArticulo == atipoarticulo
                                                                 select TipoArticuloa).ToList();

                                           

                                           if (infoTipoA.Count == 0 )
                                                {
                                                    infomacionDeHoja.Rows[r]["Accion"] =string.Format(app_GlobalResources.Content.msgErrorRelacion, "TipoArticulo","Marca");
                                                    infomacionDeHoja.Rows[r]["ide"] = 0;
                                                    infomacionDeHoja.AcceptChanges();
                                                }
                                           else
                                                {
                                                    if (infomacionDeHoja.Rows[r]["idCategoria"].ToString() != "")
                                                    {
                                                        Categoria cate = db.Categoria.Find(decimal.Parse(infomacionDeHoja.Rows[r]["idCategoria"].ToString()));
                                                        if (cate == null)
                                                        {
                                                            infomacionDeHoja.Rows[r]["Accion"] = string.Format(app_GlobalResources.Content.msgIDNoExiste, "idCategoria"); 
                                                            infomacionDeHoja.Rows[r]["ide"] = 0;
                                                            infomacionDeHoja.AcceptChanges();
                                                        }
                                                        else
                                                        {
                                                            infomacionDeHoja.Rows[r]["Accion"]= app_GlobalResources.Content.msgModificacion;
                                                            infomacionDeHoja.Rows[r]["ide"] = 2;
                                                            infomacionDeHoja.AcceptChanges();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        infomacionDeHoja.Rows[r]["Accion"]= app_GlobalResources.Content.msgAlta;
                                                        infomacionDeHoja.Rows[r]["ide"] = 1;
                                                        infomacionDeHoja.AcceptChanges();
                                                    }
                                                    
                                                    
                                                }

                                            
                                          
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
                                    Categoria Categoria_a = db.Categoria.Find(decimal.Parse(infomacionDeHoja.Rows[r]["idCategoria"].ToString()));
                                    Categoria_a.descCategoria = infomacionDeHoja.Rows[r]["descCategoria"].ToString();

                                    db.Entry(Categoria_a).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                                else
                                {
                                    //alta
                                    newID idmarca = new newID();
                                    Categoria TipoArticulo_A = new Categoria();
                                    if (infomacionDeHoja.Rows[r]["ide"].ToString() == "1")
                                    { TipoArticulo_A.idCategoria = idmarca.CalculaId(1); }
                                    else
                                    { TipoArticulo_A.idCategoria = decimal.Parse(infomacionDeHoja.Rows[r]["idCategoria"].ToString()); }
                                   
                                    TipoArticulo_A.descCategoria = infomacionDeHoja.Rows[r]["descCategoria"].ToString();
                                    TipoArticulo_A.idTipoArticulo = decimal.Parse(infomacionDeHoja.Rows[r]["idTipoArticulo"].ToString());
                                    TipoArticulo_A.idUsuario_alta = User.idUsuario;
                                    TipoArticulo_A.idEmpresa = User.idEmpresa;
                                    TipoArticulo_A.activo = true;
                                    TipoArticulo_A.fecha_alta = DateTime.Now;
                                    db.Categoria.Add(TipoArticulo_A);
                                    db.SaveChanges();
                                }
                            }

                        }
                    }

                    break;
                case "Subcategoria":

                    for (Int32 r = 0; r <= infomacionDeHoja.Rows.Count - 1; r++)
                    {
                        if (accion == 0)
                        {
                           
                              
                                    if (infomacionDeHoja.Rows[r]["idCategoria"].ToString() == "" || infomacionDeHoja.Rows[r]["idMarca"].ToString() == "" || infomacionDeHoja.Rows[r]["idTipoArticulo"].ToString() == "")
                                    {
                                        infomacionDeHoja.Rows[r]["Accion"] = infomacionDeHoja.Rows[r]["idCategoria"].ToString() == "" ? string.Format(app_GlobalResources.Content.msgIDNulo, "idCategoria") : infomacionDeHoja.Rows[r]["idMarca"].ToString() == "" ? string.Format(app_GlobalResources.Content.msgIDNulo, "idMarca") : string.Format(app_GlobalResources.Content.msgIDNulo, "idTipoArticulo"); 
                                        infomacionDeHoja.Rows[r]["ide"] = 0;
                                        infomacionDeHoja.AcceptChanges();
                                    }
                                    else 
                                    {
                                      
                                        Categoria cate = db.Categoria.Find(decimal.Parse(infomacionDeHoja.Rows[r]["idCategoria"].ToString()));

                                        if (cate == null)
                                        {
                                            infomacionDeHoja.Rows[r]["Accion"] = string.Format(app_GlobalResources.Content.msgIDNoExiste, "idCategoria");
                                            infomacionDeHoja.Rows[r]["ide"] = 0;
                                            infomacionDeHoja.AcceptChanges();
                                        }
                                        else
                                        {
                                            decimal aidmarca = decimal.Parse(infomacionDeHoja.Rows[r]["idMarca"].ToString());
                                            decimal aidtipoarticulo = decimal.Parse(infomacionDeHoja.Rows[r]["idTipoArticulo"].ToString());
                                            

                                            var infoTipoA = (from TipoArticuloa in db.TipoArticulo
                                                             where TipoArticuloa.idMarca == aidmarca && TipoArticuloa.idTipoArticulo == aidtipoarticulo
                                                             select TipoArticuloa).ToList();

                                          

                                            if (infoTipoA.Count == 0 )
                                            {
                                                infomacionDeHoja.Rows[r]["Accion"] =string.Format(app_GlobalResources.Content.msgErrorRelacion, "TipoArticulo","Marca");
                                                infomacionDeHoja.Rows[r]["ide"] = 0;
                                                infomacionDeHoja.AcceptChanges();
                                            }
                                            else 
                                            {
                                                if (infomacionDeHoja.Rows[r]["idSubCategoria"].ToString() == "")
                                                {
                                                    infomacionDeHoja.Rows[r]["Accion"]= app_GlobalResources.Content.msgAlta;
                                                    infomacionDeHoja.Rows[r]["ide"] = 1;
                                                    infomacionDeHoja.AcceptChanges();
                                                }
                                                else
                                                {
                                                    SubCategoria subCat = db.SubCategoria.Find(decimal.Parse(infomacionDeHoja.Rows[r]["idSubCategoria"].ToString()));
                                                    if (subCat == null)
                                                    {
                                                        infomacionDeHoja.Rows[r]["Accion"] = string.Format(app_GlobalResources.Content.msgIDNoExiste, "idSubCategoria");
                                                        infomacionDeHoja.Rows[r]["ide"] = 0;
                                                        infomacionDeHoja.AcceptChanges();
                                                    }
                                                    else
                                                    {
                                                        infomacionDeHoja.Rows[r]["Accion"]= app_GlobalResources.Content.msgModificacion;
                                                        infomacionDeHoja.Rows[r]["ide"] = 2;
                                                        infomacionDeHoja.AcceptChanges();
                                                    }
                                                   
                                                }
                                               
                                            }
                                           
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
                                    SubCategoria TipoArticulo_A = db.SubCategoria.Find(decimal.Parse(infomacionDeHoja.Rows[r]["idSubCategoria"].ToString()));
                                    TipoArticulo_A.descSubCategoria = infomacionDeHoja.Rows[r]["descSubCategoria"].ToString();

                                    db.Entry(TipoArticulo_A).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                                else
                                {
                                    //alta
                                    newID idmarca = new newID();
                                    SubCategoria TipoArticulo_A = new SubCategoria();
                                    if (infomacionDeHoja.Rows[r]["ide"].ToString() == "1")
                                    { TipoArticulo_A.idSubCategoria = idmarca.CalculaId(1); }
                                    else
                                    { TipoArticulo_A.idSubCategoria = Convert.ToDecimal(infomacionDeHoja.Rows[r]["idSubCategoria"].ToString()); }

                                   
                                    TipoArticulo_A.descSubCategoria = infomacionDeHoja.Rows[r]["descSubCategoria"].ToString();
                                    TipoArticulo_A.idCategoria = Convert.ToDecimal( infomacionDeHoja.Rows[r]["idCategoria"].ToString());
                                    TipoArticulo_A.idUsuario_alta = User.idUsuario;

                                    TipoArticulo_A.activo = true;
                                    TipoArticulo_A.fecha_alta = DateTime.Now;
                                    db.SubCategoria.Add(TipoArticulo_A);
                                    db.SaveChanges();
                                }
                            }

                        }
                    }
                    break;
                case "Presentacion":

                    for (Int32 r = 0; r <= infomacionDeHoja.Rows.Count - 1; r++)
                    {
                        if (accion == 0)
                        {
                           
                                    if (infomacionDeHoja.Rows[r]["idTipoArticulo"].ToString() == "" || infomacionDeHoja.Rows[r]["idMarca"].ToString() == "" || infomacionDeHoja.Rows[r]["descPresentacion"].ToString() == "")
                                    {
                                        infomacionDeHoja.Rows[r]["Accion"] = infomacionDeHoja.Rows[r]["idTipoArticulo"].ToString() == "" ?  string.Format(app_GlobalResources.Content.msgIDNulo, "idTipoArticulo"): infomacionDeHoja.Rows[r]["idMarca"].ToString() == "" ?  string.Format(app_GlobalResources.Content.msgIDNulo, "idMarca")  :  string.Format(app_GlobalResources.Content.msgIDNulo, "descPresentacion");
                                        infomacionDeHoja.Rows[r]["ide"] = 0;
                                        infomacionDeHoja.AcceptChanges();
                                    }
                                    else
                                    {
                                     
                                        TipoArticulo TipoA = db.TipoArticulo.Find(decimal.Parse(infomacionDeHoja.Rows[r]["idTipoArticulo"].ToString()));

                                        if (TipoA == null)
                                        {
                                            infomacionDeHoja.Rows[r]["Accion"] = string.Format(app_GlobalResources.Content.msgIDNoExiste, "idTipoArticulo");
                                            infomacionDeHoja.Rows[r]["ide"] = 0;
                                            infomacionDeHoja.AcceptChanges();
                                        }
                                        else
                                        {
                                            decimal aidmarca = decimal.Parse(infomacionDeHoja.Rows[r]["idMarca"].ToString());
                                            decimal aidtipoarticulo = decimal.Parse(infomacionDeHoja.Rows[r]["idTipoArticulo"].ToString());
                                         

                                                var infoTipoA = (from TipoArticuloa in db.TipoArticulo
                                                                 where TipoArticuloa.idMarca == aidmarca && TipoArticuloa.idTipoArticulo == aidtipoarticulo
                                                                 select TipoArticuloa).ToList();

                                               

                                                if (infoTipoA.Count == 0 )
                                                {
                                                    infomacionDeHoja.Rows[r]["Accion"] = string.Format(app_GlobalResources.Content.msgErrorRelacion, "TipoArticulo", "Marca");
                                                    infomacionDeHoja.Rows[r]["ide"] = 0;
                                                    infomacionDeHoja.AcceptChanges();
                                                }
                                                else 
                                                {
                                                    if (infomacionDeHoja.Rows[r]["idPresentacion"].ToString() == "")
                                                    {
                                                        infomacionDeHoja.Rows[r]["Accion"]= app_GlobalResources.Content.msgAlta;
                                                        infomacionDeHoja.Rows[r]["ide"] = 1;
                                                        infomacionDeHoja.AcceptChanges();
                                                    }
                                                    else
                                                    {
                                                        

                                                        Presentacion presenta = db.Presentacion.Find(decimal.Parse(infomacionDeHoja.Rows[r]["idPresentacion"].ToString()));
                                                        if (presenta == null)
                                                        {
                                                            infomacionDeHoja.Rows[r]["Accion"] = string.Format(app_GlobalResources.Content.msgIDNoExiste, "idPresentacion");
                                                            infomacionDeHoja.Rows[r]["ide"] = 0;
                                                            infomacionDeHoja.AcceptChanges();
                                                        }
                                                        else
                                                        {
                                                            infomacionDeHoja.Rows[r]["Accion"]= app_GlobalResources.Content.msgModificacion;
                                                            infomacionDeHoja.Rows[r]["ide"] = 2;
                                                            infomacionDeHoja.AcceptChanges();
                                                        }
                                                        
                                                    }
                                                   
                                                }
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
                                    Presentacion TipoArticulo_A = db.Presentacion.Find(decimal.Parse(infomacionDeHoja.Rows[r]["idPresentacion"].ToString()));
                                    TipoArticulo_A.descPresentacion = infomacionDeHoja.Rows[r]["descPresentacion"].ToString();

                                    db.Entry(TipoArticulo_A).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                                else
                                {
                                    //alta
                                    newID idmarca = new newID();
                                    Presentacion TipoArticulo_A = new Presentacion();
                                    if (infomacionDeHoja.Rows[r]["ide"].ToString() == "1")
                                    { TipoArticulo_A.idPresentacion = idmarca.CalculaId(1); }
                                    else
                                    { TipoArticulo_A.idPresentacion = Convert.ToDecimal( infomacionDeHoja.Rows[r]["idPresentacion"].ToString()); }

                                   
                                    TipoArticulo_A.descPresentacion = infomacionDeHoja.Rows[r]["descPresentacion"].ToString();
                                    TipoArticulo_A.idTipoArticulo = Convert.ToDecimal(infomacionDeHoja.Rows[r]["idTipoArticulo"].ToString());
                                    TipoArticulo_A.idUsuario_alta = User.idUsuario;
                                    TipoArticulo_A.idEmpresa = User.idEmpresa;
                                    TipoArticulo_A.activo = true;
                                    TipoArticulo_A.fecha_alta = DateTime.Now;
                                    db.Presentacion.Add(TipoArticulo_A);
                                    db.SaveChanges();
                                }
                            }

                        }
                    }

                    break;
                case "Color":
                    for (Int32 r = 0; r <= infomacionDeHoja.Rows.Count - 1; r++)
                    {
                        if (accion == 0)
                        {
                           
                                if (infomacionDeHoja.Rows[r]["claveColor"].ToString() != "")
                                {
                                    if (infomacionDeHoja.Rows[r]["idTipoArticulo"].ToString() == "" || infomacionDeHoja.Rows[r]["idMarca"].ToString() == "" || infomacionDeHoja.Rows[r]["descColor"].ToString() == "")
                                    {
                                        infomacionDeHoja.Rows[r]["Accion"] = infomacionDeHoja.Rows[r]["idTipoArticulo"].ToString() == "" ? string.Format(app_GlobalResources.Content.msgIDNulo, "idTipoArticulo") : infomacionDeHoja.Rows[r]["idMarca"].ToString() == "" ?  string.Format(app_GlobalResources.Content.msgIDNulo, "idMarca")  : string.Format(app_GlobalResources.Content.msgIDNulo, "descColor") ;
                                        infomacionDeHoja.Rows[r]["ide"] = 0;
                                        infomacionDeHoja.AcceptChanges();
                                    }
                                    else 
                                    {
                                       
                                        TipoArticulo TipoA = db.TipoArticulo.Find(decimal.Parse(infomacionDeHoja.Rows[r]["idTipoArticulo"].ToString()));

                                        if (TipoA == null)
                                        {

                                            infomacionDeHoja.Rows[r]["Accion"] = string.Format(app_GlobalResources.Content.msgIDNoExiste, "idTipoArticulo");
                                            infomacionDeHoja.Rows[r]["ide"] = 0;
                                            infomacionDeHoja.AcceptChanges();

                                           
                                        }
                                        else
                                        {
                                            
                                                decimal aidmarca = decimal.Parse(infomacionDeHoja.Rows[r]["idMarca"].ToString());
                                                decimal aidtipoarticulo = decimal.Parse(infomacionDeHoja.Rows[r]["idTipoArticulo"].ToString());
                                              

                                                var infoTipoA = (from TipoArticuloa in db.TipoArticulo
                                                                 where TipoArticuloa.idMarca == aidmarca && TipoArticuloa.idTipoArticulo == aidtipoarticulo
                                                                 select TipoArticuloa).ToList();


                                                if (infoTipoA.Count == 0 )
                                                {
                                                    infomacionDeHoja.Rows[r]["Accion"] = string.Format(app_GlobalResources.Content.msgErrorRelacion, "TipoArticulo", "Marca");
                                                    infomacionDeHoja.Rows[r]["ide"] = 0;
                                                    infomacionDeHoja.AcceptChanges();
                                                }
                                                else 
                                                {
                                                    Color colo = db.Color.Find(infomacionDeHoja.Rows[r]["claveColor"].ToString());

                                                    //se cuenta los que hay en el datatable

                                                  var validaDobles = Convert.ToDecimal( infomacionDeHoja.Compute("count(ide)", "claveColor='" + infomacionDeHoja.Rows[r]["claveColor"].ToString() + "'"));

                                                  if (validaDobles > 0)
                                                  {
                                                      infomacionDeHoja.Rows[r]["Accion"] = app_GlobalResources.Content.msgErrorValorDuplicado;
                                                      infomacionDeHoja.Rows[r]["ide"] = 0;
                                                      infomacionDeHoja.AcceptChanges();
                                                  }
                                                  else
                                                  {
                                                      if (colo == null)
                                                      {
                                                          infomacionDeHoja.Rows[r]["Accion"]= app_GlobalResources.Content.msgAlta;
                                                          infomacionDeHoja.Rows[r]["ide"] = 1;
                                                          infomacionDeHoja.AcceptChanges();
                                                      }
                                                      else
                                                      {
                                                          infomacionDeHoja.Rows[r]["Accion"]= app_GlobalResources.Content.msgModificacion;
                                                          infomacionDeHoja.Rows[r]["ide"] = 2;
                                                          infomacionDeHoja.AcceptChanges();
                                                      }
                                                  }

                                                    
                                                    
                                                }
                                        }
                                    }
                                    
                                }
                                else
                                {
                                    infomacionDeHoja.Rows[r]["Accion"] = string.Format(app_GlobalResources.Content.msgIDNulo, "claveColor");
                                    infomacionDeHoja.Rows[r]["ide"] = "0";
                                    infomacionDeHoja.AcceptChanges();
                                }
                            
                           
                        }
                        else //guarda en la bd
                        {
                            if (infomacionDeHoja.Rows[r]["ide"].ToString() != "0")
                            {
                                if (infomacionDeHoja.Rows[r]["ide"].ToString() == "2")
                                {
                                    //modificacion
                                    Color color_a = db.Color.Find(infomacionDeHoja.Rows[r]["claveColor"].ToString());
                                    color_a.descColor = infomacionDeHoja.Rows[r]["descColor"].ToString();
                                    db.Entry(color_a).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                                else
                                {
                                    //alta
                                   
                                    Color color_A = new Color();
                                    color_A.claveColor = infomacionDeHoja.Rows[r]["claveColor"].ToString();
                                    color_A.descColor = infomacionDeHoja.Rows[r]["descColor"].ToString();
                                    color_A.idTipoArticulo = Convert.ToDecimal(infomacionDeHoja.Rows[r]["idTipoArticulo"].ToString());
                                    color_A.idUsuario_alta = User.idUsuario;
                                    color_A.idEmpresa = User.idEmpresa;
                                    color_A.activo = true;
                                    color_A.fecha_alta = DateTime.Now;
                                    db.Color.Add(color_A);
                                    db.SaveChanges();
                                }
                            }

                        }
                    }
                    break;
                case "Talla":
                    for (Int32 r = 0; r <= infomacionDeHoja.Rows.Count - 1; r++)
                    {
                        if (accion == 0)
                        {
                           
                           if (infomacionDeHoja.Rows[r]["idTipoArticulo"].ToString() == "" || infomacionDeHoja.Rows[r]["idMarca"].ToString() == "" || infomacionDeHoja.Rows[r]["descTalla"].ToString() == "" || infomacionDeHoja.Rows[r]["posicion"].ToString() == "")
                                    {
                                        infomacionDeHoja.Rows[r]["Accion"] = infomacionDeHoja.Rows[r]["idTipoArticulo"].ToString() == "" ?  string.Format(app_GlobalResources.Content.msgIDNulo, "idTipoArticulo") : infomacionDeHoja.Rows[r]["idMarca"].ToString() == "" ? string.Format(app_GlobalResources.Content.msgIDNulo, "idMarca")  : infomacionDeHoja.Rows[r]["descTalla"].ToString() == "" ? string.Format(app_GlobalResources.Content.msgIDNulo, "descTalla") : string.Format(app_GlobalResources.Content.msgIDNulo, "posicion");
                                        infomacionDeHoja.Rows[r]["ide"] = 0;
                                        infomacionDeHoja.AcceptChanges();
                                    }
                           else 
                                    {
                                      
                                        TipoArticulo TipoA = db.TipoArticulo.Find(decimal.Parse(infomacionDeHoja.Rows[r]["idTipoArticulo"].ToString()));

                               if (TipoA == null)
                                            {
                                                infomacionDeHoja.Rows[r]["Accion"] = string.Format(app_GlobalResources.Content.msgIDNoExiste, "idTipoArticulo");
                                                infomacionDeHoja.Rows[r]["ide"] = 0;
                                                infomacionDeHoja.AcceptChanges();
                                            }
                                           else 
                                            {

                                                decimal aidmarca = decimal.Parse(infomacionDeHoja.Rows[r]["idMarca"].ToString());
                                                decimal aidtipoarticulo = decimal.Parse(infomacionDeHoja.Rows[r]["idTipoArticulo"].ToString());
                                          

                                                var infoTipoA = (from TipoArticuloa in db.TipoArticulo
                                                                 where TipoArticuloa.idMarca == aidmarca && TipoArticuloa.idTipoArticulo == aidtipoarticulo
                                                                 select TipoArticuloa).ToList();

                                            

                                                if (infoTipoA.Count == 0 )
                                                {
                                                    infomacionDeHoja.Rows[r]["Accion"] = string.Format(app_GlobalResources.Content.msgErrorRelacion, "TipoArticulo", "Marca");
                                                    infomacionDeHoja.Rows[r]["ide"] = 0;
                                                    infomacionDeHoja.AcceptChanges();
                                                }
                                                else 
                                                {
                                                    if (infomacionDeHoja.Rows[r]["claveTalla"].ToString() == "")
                                                    {
                                                        infomacionDeHoja.Rows[r]["Accion"] =  string.Format(app_GlobalResources.Content.msgIDNulo, "claveTalla");
                                                        infomacionDeHoja.Rows[r]["ide"] = 0;
                                                        infomacionDeHoja.AcceptChanges();
                                                    }
                                                    else
                                                    {
                                                        Talla tallas = db.Talla.Find(infomacionDeHoja.Rows[r]["claveTalla"].ToString());
                                                        var validaDobles = Convert.ToDecimal(infomacionDeHoja.Compute("count(ide)", "claveTalla='" + infomacionDeHoja.Rows[r]["claveTalla"].ToString() + "'"));

                                                        if (validaDobles > 0)
                                                        {
                                                            infomacionDeHoja.Rows[r]["Accion"] = app_GlobalResources.Content.msgErrorValorDuplicado;
                                                            infomacionDeHoja.Rows[r]["ide"] = 0;
                                                            infomacionDeHoja.AcceptChanges();
                                                        }
                                                        else
                                                        {
                                                            if (tallas == null)
                                                            {
                                                                infomacionDeHoja.Rows[r]["Accion"]= app_GlobalResources.Content.msgAlta;
                                                                infomacionDeHoja.Rows[r]["ide"] = 1;
                                                                infomacionDeHoja.AcceptChanges();
                                                            }
                                                            else
                                                            {
                                                                infomacionDeHoja.Rows[r]["Accion"]= app_GlobalResources.Content.msgModificacion;
                                                                infomacionDeHoja.Rows[r]["ide"] = 2;
                                                                infomacionDeHoja.AcceptChanges();
                                                            }
                                                    }
                                                      
                                                       
                                                    }
                                                  
                                                }
                                               
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
                                    Talla talla_A = db.Talla.Find(infomacionDeHoja.Rows[r]["claveTalla"].ToString());
                                    talla_A.claveTalla = infomacionDeHoja.Rows[r]["claveTalla"].ToString();
                                    talla_A.descTalla = infomacionDeHoja.Rows[r]["descTalla"].ToString();
                                    talla_A.idTipoArticulo = Convert.ToDecimal(infomacionDeHoja.Rows[r]["idTipoArticulo"].ToString());
                                    talla_A.idUsuario_alta = User.idUsuario;
                                    talla_A.idEmpresa = User.idEmpresa;
                                    db.Entry(talla_A).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                                else
                                {
                                    //alta

                                    Talla talla_A = new Talla();
                                    talla_A.claveTalla = infomacionDeHoja.Rows[r]["claveTalla"].ToString();
                                    talla_A.descTalla = infomacionDeHoja.Rows[r]["descTalla"].ToString();
                                    talla_A.idTipoArticulo = Convert.ToDecimal(infomacionDeHoja.Rows[r]["idTipoArticulo"].ToString());
                                    talla_A.idUsuario_alta = User.idUsuario;
                                    talla_A.idEmpresa = User.idEmpresa;
                                    talla_A.activo = true;
                                    talla_A.fecha_alta = DateTime.Now;
                                    db.Talla.Add(talla_A);
                                    db.SaveChanges();
                                }
                            }

                        }
                    }
                    break;

            }

            return infomacionDeHoja;
        }

        #endregion


        public ActionResult Index()
        {

            ObtieneInfo();

            return View();
        }

        public ActionResult Procesa()
        {
            var model = TempData["ModelName"] as DataTable; ;
            TempData["ModelName"] = model;
            return View();
        }

       
        //
        // POST: /MantenimientoCatalogos/Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string cmdsubmit, string TipoArchivo, string TipoCatDescarga, HttpPostedFileBase files, string TipoCatalogo, string listahojas, string ruta)
        {
            ViewBag.errorVal = 0;
            ViewBag.Catalogo = TipoCatalogo;
            ObtieneInfo();

            if (!string.IsNullOrWhiteSpace(cmdsubmit))
            {
                //Descarga informacion
                int? aux = descarga(TipoCatDescarga,1);
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
                            DataTable infomacionDeHoja = excelData.getDataDT(TipoCatalogo);
                            descarga(TipoCatDescarga, 0);
                            DataTable dt = ProcesaInfo(infomacionDeHoja, TipoCatalogo, 0);
                            DataTable dtAux =null;

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
                                
                               

                                ViewBag.catalgoSeleecionado = TipoCatalogo;
                                ViewBag.Nuevosdt = dt.Compute("count(ide)", "ide = '1'");
                                ViewBag.Actdt = dt.Compute("count(ide)", "ide = 2");
                                ViewBag.erroresdt = dt.Compute("count(ide)", "ide = 0");
                               
                            }

                            return View(dtAux);
                        }
                    }
                    else 
                    {
                        if (TipoCatalogo == "0")
                        {
                            ViewBag.error = app_GlobalResources.Content.errorSeleccionTipoCatalogo;
                            return View();
                        }

                        Ruta R = db.Ruta.Find(1604061725498493);

                        string rutabase = Server.MapPath(R.direccion);

                        if (!System.IO.Directory.Exists(rutabase))
                        {
                            System.IO.Directory.CreateDirectory(rutabase);
                        }

                        string nombreCompleto = nomb + Path.GetFileName(files.FileName);
                        path = System.IO.Path.Combine(rutabase, nombreCompleto);

                        if (files.FileName.EndsWith("txt"))
                        {
                            if (System.IO.File.Exists(path))
                                System.IO.File.Delete(path);
                            files.SaveAs(path);

                            ViewBag.pathProcesa = path;
                        
                            ViewBag.Catalogo = TipoCatalogo;
                            ArchivoData a = new ArchivoData();
                            DataTable archivosData = a.lee_archivo_stream_separador(path, '|', true);
                            DataTable dt = ProcesaInfo(archivosData, TipoCatalogo, 0);
                            TempData["ModelName"] = dt;

                            DataTable dtAux = dt.Copy();

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

                            ViewBag.catalgoSeleecionado = TipoCatalogo;
                            ViewBag.Nuevosdt = dt.Compute("count(ide)", "ide = 1");
                            ViewBag.Actdt = dt.Compute("count(ide)", "ide = 2");
                            ViewBag.erroresdt = dt.Compute("count(ide)", "ide = 0");

                          

                            View(dtAux);
                        }
                        if (files.FileName.EndsWith("xls") || files.FileName.EndsWith("xlsx"))
                        {
                            path = System.IO.Path.Combine(rutabase, nombreCompleto);
                            if (System.IO.File.Exists(path))
                                System.IO.File.Delete(path);
                            files.SaveAs(path);
                     
                            ViewBag.pathProcesa = path;
                            ViewBag.Catalogo = TipoCatalogo;
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

       

        //
        // POST: /MantenimientoCatalogos/Create

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
          
            ViewBag.errorVal = 0;

            ViewBag.TerminoProceso = "0";
            var model = TempData["ModelName"] as DataTable; ;

            if (this.IsCaptchaValid("Captcha is not valid"))
            {
                ObtieneInfo();
                //  return View("Index");
                DataTable dt = ProcesaInfo(model, Catalogo, 1);
                ViewBag.ErrMessage = "";
                ViewBag.TerminoProceso ="1";
                return View();
            }

            TempData["ModelName"] = model;
            ViewBag.ErrMessage = app_GlobalResources.Content.msgErrorCaptcha; 
            return View(); 

          
        }

       

  
    }
}
