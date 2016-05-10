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
	[MetadataType(typeof(TipoAlmacenMetadata))]

	public partial class TipoAlmacen
	{

	}

	public class TipoAlmacenMetadata
	{

		[Key]
		[Display(Name = "DN_idTipoAlmacen", ResourceType = typeof(app_GlobalResources.Content))]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0}")]
		public decimal idTipoAlmacen { get; set; }

		[Display(Name = "DN_idEmpresa", ResourceType = typeof(app_GlobalResources.Content))]
		//[Required(ErrorMessageResourceName = "REQ_idEmpresa", ErrorMessageResourceType = typeof(app_GlobalResources.Content))]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0}")]
		public decimal idEmpresa { get; set; }

		[Display(Name = "DN_descTipoAlmacen", ResourceType = typeof(app_GlobalResources.Content))]
		[Required(ErrorMessageResourceName = "REQ_descTipoAlmacen", ErrorMessageResourceType = typeof(app_GlobalResources.Content))]
		[MinLength(5,ErrorMessageResourceName = "MinLEN_descTipoAlmacen", ErrorMessageResourceType = typeof(app_GlobalResources.Content))]
		public string descTipoAlmacen { get; set; }

		//[Display(Name = "DN_idUsuario_alta", ResourceType = typeof(app_GlobalResources.Content))]
        [Display(Name = "DN_idUsuario_alta")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0}")]
		public decimal idUsuario_alta { get; set; }

		//[Display(Name = "DN_fecha_alta", ResourceType = typeof(app_GlobalResources.Content))]
        [Display(Name = "DN_fecha_alta")]
		[DataType(DataType.DateTime)]
       // [Required( ErrorMessageResourceName = "DT_fecha_alta", ErrorMessageResourceType = typeof(app_GlobalResources.Content))]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:g}")]
		public System.DateTime fecha_alta { get; set; }

		[Display(Name = "DN_activo", ResourceType = typeof(app_GlobalResources.Content))]
		public bool activo { get; set; }

	}
}
