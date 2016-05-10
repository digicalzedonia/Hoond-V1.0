﻿using System;using System.Collections.Generic;using System.Linq;using System.Web;using System.ComponentModel.DataAnnotations;using System.ComponentModel;using System.Web.DynamicData;using System.Web.Mvc;namespace Hoond.Models{	[MetadataType(typeof(RutaMetadata))]	public partial class Ruta	{	}	public class RutaMetadata	{		[Key]		[Display(Name = "DN_idRuta", ResourceType = typeof(app_GlobalResources.Content))]		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0}")]		public decimal idRuta { get; set; }		[Display(Name = "DN_descRuta", ResourceType = typeof(app_GlobalResources.Content))]		[Required(ErrorMessageResourceName = "REQ_descRuta", ErrorMessageResourceType = typeof(app_GlobalResources.Content))]		[MinLength(5,ErrorMessageResourceName = "MinLEN_descRuta", ErrorMessageResourceType = typeof(app_GlobalResources.Content))]		public string descRuta { get; set; }		[Display(Name = "DN_direccion", ResourceType = typeof(app_GlobalResources.Content))]		[Required(ErrorMessageResourceName = "REQ_direccion", ErrorMessageResourceType = typeof(app_GlobalResources.Content))]		public string direccion { get; set; }		[Display(Name = "DN_idUsuario_alta", ResourceType = typeof(app_GlobalResources.Content))]		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0}")]		public decimal idUsuario_alta { get; set; }		[Display(Name = "DN_fecha_alta", ResourceType = typeof(app_GlobalResources.Content))]		[DataType(DataType.DateTime, ErrorMessageResourceName = "DT_fecha_alta", ErrorMessageResourceType = typeof(app_GlobalResources.Content))]		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:g}")]		public System.DateTime fecha_alta { get; set; }		[Display(Name = "DN_activo", ResourceType = typeof(app_GlobalResources.Content))]		public bool activo { get; set; }	}}
