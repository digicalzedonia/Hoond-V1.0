using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.DynamicData;
using System.Web.Mvc;

namespace Hoond.Models
{
	[MetadataType(typeof(UsuarioSucursalMetadata))]

	public partial class UsuarioSucursal
	{

	}

	public class UsuarioSucursalMetadata
	{

		[Key]
		[Display(Name = "DN_idSucursal", ResourceType = typeof(app_GlobalResources.Content))]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0}")]
		public decimal idSucursal { get; set; }

		[Key]
		[Display(Name = "DN_idUsuario", ResourceType = typeof(app_GlobalResources.Content))]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0}")]
		public decimal idUsuario { get; set; }

		[Display(Name = "DN_idTipoUsuarioSucursal", ResourceType = typeof(app_GlobalResources.Content))]
		[Required(ErrorMessageResourceName = "REQ_idTipoUsuarioSucursal", ErrorMessageResourceType = typeof(app_GlobalResources.Content))]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0}")]
		public decimal idTipoUsuarioSucursal { get; set; }

		[Display(Name = "DN_idUsuario_alta", ResourceType = typeof(app_GlobalResources.Content))]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0}")]
		public decimal idUsuario_alta { get; set; }

		[Display(Name = "DN_fecha_alta", ResourceType = typeof(app_GlobalResources.Content))]
		[DataType(DataType.DateTime)]
       // [Required(ErrorMessageResourceName = "DT_fecha_alta", ErrorMessageResourceType = typeof(app_GlobalResources.Content))]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:g}")]
		public System.DateTime fecha_alta { get; set; }

		[Display(Name = "DN_activo", ResourceType = typeof(app_GlobalResources.Content))]
		public bool activo { get; set; }

	}
}
