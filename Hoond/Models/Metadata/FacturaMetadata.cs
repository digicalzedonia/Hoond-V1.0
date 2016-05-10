﻿using System;using System.Collections.Generic;using System.Linq;using System.Web;using System.ComponentModel.DataAnnotations;using System.ComponentModel;using System.Web.DynamicData;using System.Web.Mvc;namespace Hoond.Models{	[MetadataType(typeof(FacturaMetadata))]	public partial class Factura	{	}	public class FacturaMetadata	{		[Key]		[Display(Name = "DN_idFactura", ResourceType = typeof(app_GlobalResources.Content))]		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0}")]		public decimal idFactura { get; set; }		[Display(Name = "DN_archivoPDF", ResourceType = typeof(app_GlobalResources.Content))]		public string archivoPDF { get; set; }		[Display(Name = "DN_archivoXML", ResourceType = typeof(app_GlobalResources.Content))]		public string archivoXML { get; set; }		[Display(Name = "DN_idRuta", ResourceType = typeof(app_GlobalResources.Content))]		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0}")]		public decimal idRuta { get; set; }		[Display(Name = "DN_serie", ResourceType = typeof(app_GlobalResources.Content))]		[Required(ErrorMessageResourceName = "REQ_serie", ErrorMessageResourceType = typeof(app_GlobalResources.Content))]		public string serie { get; set; }		[Display(Name = "DN_foio", ResourceType = typeof(app_GlobalResources.Content))]		[Required(ErrorMessageResourceName = "REQ_foio", ErrorMessageResourceType = typeof(app_GlobalResources.Content))]		public string foio { get; set; }		[Display(Name = "DN_fechaFactura", ResourceType = typeof(app_GlobalResources.Content))]		[Required(ErrorMessageResourceName = "REQ_fechaFactura", ErrorMessageResourceType = typeof(app_GlobalResources.Content))]		[DataType(DataType.DateTime, ErrorMessageResourceName = "DT_fechaFactura", ErrorMessageResourceType = typeof(app_GlobalResources.Content))]		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:g}")]		public System.DateTime fechaFactura { get; set; }		[Display(Name = "DN_moneda", ResourceType = typeof(app_GlobalResources.Content))]		[Required(ErrorMessageResourceName = "REQ_moneda", ErrorMessageResourceType = typeof(app_GlobalResources.Content))]		public string moneda { get; set; }		[Display(Name = "DN_subtotal", ResourceType = typeof(app_GlobalResources.Content))]		[Required(ErrorMessageResourceName = "REQ_subtotal", ErrorMessageResourceType = typeof(app_GlobalResources.Content))]		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0}")]		public decimal subtotal { get; set; }		[Display(Name = "DN_tasaIVA", ResourceType = typeof(app_GlobalResources.Content))]		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0}")]		public decimal tasaIVA { get; set; }		[Display(Name = "DN_importeIVA", ResourceType = typeof(app_GlobalResources.Content))]		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0}")]		public decimal importeIVA { get; set; }		[Display(Name = "DN_total", ResourceType = typeof(app_GlobalResources.Content))]		[Required(ErrorMessageResourceName = "REQ_total", ErrorMessageResourceType = typeof(app_GlobalResources.Content))]		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0}")]		public decimal total { get; set; }		[Display(Name = "DN_emisorRfc", ResourceType = typeof(app_GlobalResources.Content))]		[Required(ErrorMessageResourceName = "REQ_emisorRfc", ErrorMessageResourceType = typeof(app_GlobalResources.Content))]		public string emisorRfc { get; set; }		[Display(Name = "DN_emisorNombre", ResourceType = typeof(app_GlobalResources.Content))]		[Required(ErrorMessageResourceName = "REQ_emisorNombre", ErrorMessageResourceType = typeof(app_GlobalResources.Content))]		public string emisorNombre { get; set; }		[Display(Name = "DN_emisorCalle", ResourceType = typeof(app_GlobalResources.Content))]		public string emisorCalle { get; set; }		[Display(Name = "DN_emisorNoExterior", ResourceType = typeof(app_GlobalResources.Content))]		public string emisorNoExterior { get; set; }		[Display(Name = "DN_emisorNoInterior", ResourceType = typeof(app_GlobalResources.Content))]		public string emisorNoInterior { get; set; }		[Display(Name = "DN_emisorColonia", ResourceType = typeof(app_GlobalResources.Content))]		public string emisorColonia { get; set; }		[Display(Name = "DN_emisorLocalidad", ResourceType = typeof(app_GlobalResources.Content))]		public string emisorLocalidad { get; set; }		[Display(Name = "DN_emisorMunicipio", ResourceType = typeof(app_GlobalResources.Content))]		public string emisorMunicipio { get; set; }		[Display(Name = "DN_emisorEstado", ResourceType = typeof(app_GlobalResources.Content))]		public string emisorEstado { get; set; }		[Display(Name = "DN_emisorPais", ResourceType = typeof(app_GlobalResources.Content))]		public string emisorPais { get; set; }		[Display(Name = "DN_emisorCP", ResourceType = typeof(app_GlobalResources.Content))]		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0}")]		public decimal emisorCP { get; set; }		[Display(Name = "DN_receptorRfc", ResourceType = typeof(app_GlobalResources.Content))]		[Required(ErrorMessageResourceName = "REQ_receptorRfc", ErrorMessageResourceType = typeof(app_GlobalResources.Content))]		public string receptorRfc { get; set; }		[Display(Name = "DN_receptorNombre", ResourceType = typeof(app_GlobalResources.Content))]		[Required(ErrorMessageResourceName = "REQ_receptorNombre", ErrorMessageResourceType = typeof(app_GlobalResources.Content))]		public string receptorNombre { get; set; }		[Display(Name = "DN_receptorCalle", ResourceType = typeof(app_GlobalResources.Content))]		public string receptorCalle { get; set; }		[Display(Name = "DN_receptorNoExterior", ResourceType = typeof(app_GlobalResources.Content))]		public string receptorNoExterior { get; set; }		[Display(Name = "DN_receptorNoInterior", ResourceType = typeof(app_GlobalResources.Content))]		public string receptorNoInterior { get; set; }		[Display(Name = "DN_receptorColonia", ResourceType = typeof(app_GlobalResources.Content))]		public string receptorColonia { get; set; }		[Display(Name = "DN_receptorLocalidad", ResourceType = typeof(app_GlobalResources.Content))]		public string receptorLocalidad { get; set; }		[Display(Name = "DN_receptorMunicipio", ResourceType = typeof(app_GlobalResources.Content))]		public string receptorMunicipio { get; set; }		[Display(Name = "DN_receptorEstado", ResourceType = typeof(app_GlobalResources.Content))]		public string receptorEstado { get; set; }		[Display(Name = "DN_receptorPais", ResourceType = typeof(app_GlobalResources.Content))]		public string receptorPais { get; set; }		[Display(Name = "DN_receptorCP", ResourceType = typeof(app_GlobalResources.Content))]		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0}")]		public decimal receptorCP { get; set; }		[Display(Name = "DN_idUsuario_alta", ResourceType = typeof(app_GlobalResources.Content))]		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0}")]		public decimal idUsuario_alta { get; set; }		[Display(Name = "DN_fecha_alta", ResourceType = typeof(app_GlobalResources.Content))]		[DataType(DataType.DateTime, ErrorMessageResourceName = "DT_fecha_alta", ErrorMessageResourceType = typeof(app_GlobalResources.Content))]		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:g}")]		public System.DateTime fecha_alta { get; set; }		[Display(Name = "DN_activo", ResourceType = typeof(app_GlobalResources.Content))]		public bool activo { get; set; }	}}