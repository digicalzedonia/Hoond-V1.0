using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hoond.Models;
using Archivos;
using System.IO;


namespace Hoond.Controllers
{
    public class FacturaController : Controller
    {
        private HoondDBEntities db = new HoondDBEntities();
        private Comun DatComun = new Comun();

        //
        // GET: /Factura/

        public ActionResult Index()
        {
            var factura = db.Factura.Include(f => f.Usuario).Include(f => f.Ruta).Where(f => f.activo==true);
            return View(factura.ToList());
        }

        public ActionResult LeerXML()
        {
            var factura = db.Factura.Include(f => f.Usuario).Include(f => f.Ruta);
            return View(factura.ToList());
        }

        //
        // GET: /Factura/Details/5

        public ActionResult Details(decimal id = 0)
        {
            Factura factura = db.Factura.Find(id);
            if (factura == null)
            {
                return HttpNotFound();
            }
            return View(factura);
        }

        //
        // GET: /Factura/Create

        public ActionResult Create()
        {
            ViewBag.idUsuario_alta = new SelectList(db.Usuario, "idUsuario", "nombreUsuario");
            ViewBag.idPath = new SelectList(db.Ruta, "idPath", "descPath");
            return View();
        }

        //
        // POST: /Factura/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Factura factura)
        {
            UserLogin User = (UserLogin)Session["user"];

            if (ModelState.IsValid)
            {
                factura.idFactura = Convert.ToDecimal(String.Format("{0:yyMMddHmmss}", DateTime.Now));
                factura.idUsuario_alta = User.idUsuario;
                factura.activo = true;
                factura.fecha_alta = DateTime.Now;
                factura.archivoPDF ="";
                factura.archivoXML = "";
                db.Factura.Add(factura);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idUsuario_alta = new SelectList(db.Usuario, "idUsuario", "nombreUsuario", factura.idUsuario_alta);
            ViewBag.idPath = new SelectList(db.Ruta, "idPath", "descPath", factura.idRuta);
            return View(factura);
        }


        [HttpPost]
        public ActionResult BajarPDF(Factura factura)
        {
           // string ruta = factura.Ruta.direccion + factura.archivoPDF;
            string ruta = Path.Combine(Server.MapPath(factura.Ruta.direccion), factura.archivoPDF);
            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + factura.archivoPDF);
            Response.WriteFile(ruta);
            Response.Flush();
            Response.End();
            return null;
        }

        [HttpPost]
        public ActionResult BajarXML(Factura factura)
        {
           // string ruta = factura.Ruta.direccion + factura.archivoXML;
            string ruta = Path.Combine(Server.MapPath(factura.Ruta.direccion), factura.archivoXML);

            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + factura.archivoXML);
            Response.WriteFile(ruta);
            Response.Flush();
            Response.End();
            return null;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LeerXML(HttpPostedFileBase XMLfiles, HttpPostedFileBase PDFfiles)
        {
            if (XMLfiles == null || XMLfiles.ContentLength == 0)
            {
                ViewBag.error = app_GlobalResources.Content.msg_SeleccioneArchivo;
                return View("Create");
            }
            else
            {
                UserLogin User = (UserLogin)Session["user"];

                if (XMLfiles.FileName.EndsWith("xml"))
                {
                    Hoond.Models.Ruta ubicacion = db.Ruta.Find(160308180634);
                   // string ruta = ubicacion.direccion;
                    string ruta =Server.MapPath(ubicacion.direccion);
                    string nombreCompleto = "";
                    string nombePRF = "";
                    //valida el segundo archivo que sea PDF.
                    if (PDFfiles != null)
                    {
                        if (PDFfiles.ContentLength != 0)
                        {    if (PDFfiles.FileName.EndsWith("pdf") ==false)
                        {
                            ViewBag.error = app_GlobalResources.Content.msg_SeleccionePDF;
                            return View();
                        }
                        nombePRF = System.IO.Path.GetFileName(PDFfiles.FileName);
                         nombreCompleto = DateTime.Now.ToString("ddMyyyy HHmmss tt") + "_" + nombePRF;
                        string pathPDF = System.IO.Path.Combine(ruta, nombreCompleto);

                        if (!System.IO.Directory.Exists(ruta))
                        {
                            System.IO.Directory.CreateDirectory(ruta);
                        }

                        if (System.IO.File.Exists(pathPDF))
                            System.IO.File.Delete(pathPDF);
                        PDFfiles.SaveAs(pathPDF);
                        
                        }
                     
                    }


                    string nombeXML = DateTime.Now.ToString("dMyyyy HHmmss ff") + "_" + System.IO.Path.GetFileName(XMLfiles.FileName);
                    string path = System.IO.Path.Combine(ruta, nombeXML);

                    if (!System.IO.Directory.Exists(ruta))
                    {
                        System.IO.Directory.CreateDirectory(ruta);
                    }

                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                    XMLfiles.SaveAs(path);

                    ArchivoData a = new ArchivoData();
                    DataSet archivosData = a.LeerXML(path);

                    newID idfactura = new newID();

                    decimal ids = idfactura.CalculaId(1);

                    Factura f = new Factura(); ;
                    f.idFactura = ids;
                    f.archivoPDF = nombreCompleto;
                    f.archivoXML = nombeXML;
                    f.idRuta = 160308180634;
                    f.serie=   Convert.ToString(archivosData.Tables["Comprobante"].Rows[0]["serie"].ToString());
                    f.foio=   Convert.ToString(archivosData.Tables["Comprobante"].Rows[0]["folio"].ToString());
                    f.fechaFactura=  Convert.ToDateTime( archivosData.Tables["Comprobante"].Rows[0]["fecha"].ToString());
                    f.moneda=  archivosData.Tables["Comprobante"].Rows[0]["Moneda"].ToString();
                    f.subtotal=  Convert.ToDecimal( archivosData.Tables["Comprobante"].Rows[0]["subTotal"].ToString());
                    f.tasaIVA=  Convert.ToDecimal( archivosData.Tables["Traslado"].Rows[0]["tasa"].ToString());
                    f.importeIVA=  Convert.ToDecimal( archivosData.Tables["Traslado"].Rows[0]["importe"].ToString());
                    f.total=  Convert.ToDecimal( archivosData.Tables["Comprobante"].Rows[0]["total"].ToString());
                    f.emisorRfc=  Convert.ToString( archivosData.Tables["Emisor"].Rows[0]["rfc"].ToString());
                    f.emisorNombre=  Convert.ToString( archivosData.Tables["Emisor"].Rows[0]["nombre"].ToString());
                    f.emisorCalle=  Convert.ToString( archivosData.Tables["DomicilioFiscal"].Rows[0]["calle"].ToString());
                    f.emisorNoExterior=  Convert.ToString( archivosData.Tables["DomicilioFiscal"].Rows[0]["noExterior"].ToString());
                    f.emisorNoInterior=  Convert.ToString( archivosData.Tables["DomicilioFiscal"].Rows[0]["noInterior"].ToString());
                    f.emisorColonia=  Convert.ToString( archivosData.Tables["DomicilioFiscal"].Rows[0]["colonia"].ToString());
                    f.emisorLocalidad=  Convert.ToString( archivosData.Tables["DomicilioFiscal"].Rows[0]["localidad"].ToString());
                    f.emisorMunicipio=  Convert.ToString( archivosData.Tables["DomicilioFiscal"].Rows[0]["municipio"].ToString());
                    f.emisorEstado=  Convert.ToString( archivosData.Tables["DomicilioFiscal"].Rows[0]["estado"].ToString());
                    f.emisorPais=  Convert.ToString( archivosData.Tables["DomicilioFiscal"].Rows[0]["pais"].ToString());
                    f.emisorCP=  Convert.ToDecimal( archivosData.Tables["DomicilioFiscal"].Rows[0]["codigoPostal"].ToString());
                    f.receptorRfc=  Convert.ToString( archivosData.Tables["Receptor"].Rows[0]["rfc"].ToString());
                    f.receptorNombre=  Convert.ToString( archivosData.Tables["Receptor"].Rows[0]["nombre"].ToString());
                    f.receptorCalle=  Convert.ToString( archivosData.Tables["Domicilio"].Rows[0]["calle"].ToString());
                    f.receptorNoExterior=  Convert.ToString( archivosData.Tables["Domicilio"].Rows[0]["noExterior"].ToString());
                    f.receptorNoInterior=  Convert.ToString( archivosData.Tables["Domicilio"].Rows[0]["noInterior"].ToString());
                    f.receptorColonia=  Convert.ToString( archivosData.Tables["Domicilio"].Rows[0]["colonia"].ToString());
                    f.receptorLocalidad=  Convert.ToString( archivosData.Tables["Domicilio"].Rows[0]["localidad"].ToString());
                    f.receptorMunicipio=  Convert.ToString( archivosData.Tables["Domicilio"].Rows[0]["municipio"].ToString());
                    f.receptorEstado=  Convert.ToString( archivosData.Tables["Domicilio"].Rows[0]["estado"].ToString());
                    f.receptorPais=  Convert.ToString( archivosData.Tables["Domicilio"].Rows[0]["pais"].ToString());
                    f.receptorCP=  Convert.ToDecimal( archivosData.Tables["Domicilio"].Rows[0]["codigoPostal"].ToString());
                    f.idUsuario_alta = User.idUsuario;
                    f.fecha_alta = DateTime.Now;
                    f.activo =true;
                    db.Factura.Add(f);
                    db.SaveChanges();

                    //'se inserta en concepto'

                    foreach (DataRow row in archivosData.Tables["Concepto"].Rows)
                    {
                        FacturaConcepto fc = new FacturaConcepto();
                        fc.idFactura = ids;
                        fc.idFacturaConcepto = idfactura.CalculaId(1);
                        fc.cantidad = Convert.ToDecimal(row["cantidad"].ToString());
                        fc.unidad = row["unidad"].ToString();
                        fc.descripcion = row["descripcion"].ToString();
                        fc.valorUnitario = Convert.ToDecimal(row["valorUnitario"].ToString());
                        fc.importe = Convert.ToDecimal(row["importe"].ToString());
                        fc.idUsuario_alta = User.idUsuario;
                        fc.fecha_alta = DateTime.Now;
                        fc.activo = true;

                        db.FacturaConcepto.Add(fc);
                        db.SaveChanges();

                       

                    }

                   
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = app_GlobalResources.Content.msg_TipoArchivo;
                    return View("Create");
                }
            }
        }

        //
        // GET: /Factura/Edit/5

        public ActionResult Edit(decimal id = 0)
        {
            Factura factura = db.Factura.Find(id);
            if (factura == null)
            {
                return HttpNotFound();
            }
            ViewBag.idUsuario_alta = new SelectList(db.Usuario, "idUsuario", "nombreUsuario", factura.idUsuario_alta);
            ViewBag.idPath = new SelectList(db.Ruta, "idPath", "descPath", factura.idRuta);
            return View(factura);
        }

        //
        // POST: /Factura/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Factura factura)
        {
            if (ModelState.IsValid)
            {
                db.Entry(factura).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idUsuario_alta = new SelectList(db.Usuario, "idUsuario", "nombreUsuario", factura.idUsuario_alta);
            ViewBag.idPath = new SelectList(db.Ruta, "idPath", "descPath", factura.idRuta);
            return View(factura);
        }

        //
        // GET: /Factura/Delete/5

        public ActionResult Delete(decimal id = 0)
        {
            Factura factura = db.Factura.Find(id);
           
            if (factura == null)
            {
                return HttpNotFound();
            }
            return View(factura);
        }

        //
        // POST: /Factura/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            Factura factura = db.Factura.Find(id);
            //db.factura.Remove(factura);
            factura.activo = false;
            //db.path.Remove(path);
            db.Entry(factura).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}