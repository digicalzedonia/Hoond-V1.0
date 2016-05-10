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
	[MetadataType(typeof(SucursalMetadata))]

	public partial class Sucursal
	{

	}

	public class SucursalMetadata
	{

		[Key]
		[Display(Name = "DN_idSucursal", ResourceType = typeof(app_GlobalResources.Content))]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0}")]
		public decimal idSucursal { get; set; }

		[Display(Name = "DN_idEmpresa", ResourceType = typeof(app_GlobalResources.Content))]
		//[Required(ErrorMessageResourceName = "REQ_idEmpresa", ErrorMessageResourceType = typeof(app_GlobalResources.Content))]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0}")]
		public decimal idEmpresa { get; set; }

		[Display(Name = "DN_idTipoSucursal", ResourceType = typeof(app_GlobalResources.Content))]
		[Required(ErrorMessageResourceName = "REQ_idTipoSucursal", ErrorMessageResourceType = typeof(app_GlobalResources.Content))]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0}")]
		public decimal idTipoSucursal { get; set; }

		[Display(Name = "DN_nombreSucursal", ResourceType = typeof(app_GlobalResources.Content))]
		[Required(ErrorMessageResourceName = "REQ_nombreSucursal", ErrorMessageResourceType = typeof(app_GlobalResources.Content))]
		public string nombreSucursal { get; set; }

		[Display(Name = "DN_telefonoSucursal", ResourceType = typeof(app_GlobalResources.Content))]
		[Required(ErrorMessageResourceName = "REQ_telefonoSucursal", ErrorMessageResourceType = typeof(app_GlobalResources.Content))]
        [DataType(DataType.PhoneNumber)]
      //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessageResourceName = "DT_telefonoSucursal", ErrorMessageResourceType = typeof(app_GlobalResources.Content))]
		public string telefonoSucursal { get; set; }

		[Display(Name = "DN_dirSucursalPais", ResourceType = typeof(app_GlobalResources.Content))]
        [MaxLength(50, ErrorMessageResourceName = "MaxLEN_dirSucursalPais", ErrorMessageResourceType = typeof(app_GlobalResources.Content))]
		public string dirSucursalPais { get; set; }

		[Display(Name = "DN_dirSucursalEstado", ResourceType = typeof(app_GlobalResources.Content))]
        [MaxLength(50, ErrorMessageResourceName = "MaxLEN_dirSucursalEstado", ErrorMessageResourceType = typeof(app_GlobalResources.Content))]
		public string dirSucursalEstado { get; set; }

		[Display(Name = "DN_dirSucursalMunicipio", ResourceType = typeof(app_GlobalResources.Content))]
        [MaxLength(50, ErrorMessageResourceName = "MaxLEN_dirSucursalMunicipio", ErrorMessageResourceType = typeof(app_GlobalResources.Content))]
		public string dirSucursalMunicipio { get; set; }

		[Display(Name = "DN_dirSucursalCP", ResourceType = typeof(app_GlobalResources.Content))]
        [RegularExpression(@"^([1-9]{2}|[0-9][1-9]|[1-9][0-9])[0-9]{3}$", ErrorMessageResourceName = "DT_dirSucursalCP", ErrorMessageResourceType = typeof(app_GlobalResources.Content))]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0}")]
        public decimal dirSucursalCP { get; set; }

		[Display(Name = "DN_dirSucursalColonia", ResourceType = typeof(app_GlobalResources.Content))]
        [MaxLength(50, ErrorMessageResourceName = "MaxLEN_dirSucursalColonia", ErrorMessageResourceType = typeof(app_GlobalResources.Content))]
		public string dirSucursalColonia { get; set; }

		[Display(Name = "DN_dirSucursalCalle", ResourceType = typeof(app_GlobalResources.Content))]
        [MaxLength(50, ErrorMessageResourceName = "MaxLEN_dirSucursalCalle", ErrorMessageResourceType = typeof(app_GlobalResources.Content))]
		public string dirSucursalCalle { get; set; }

		[Display(Name = "DN_dirSucursalNoExterior", ResourceType = typeof(app_GlobalResources.Content))]
		public string dirSucursalNoExterior { get; set; }

		[Display(Name = "DN_dirSucursalNoInterior", ResourceType = typeof(app_GlobalResources.Content))]
		public string dirSucursalNoInterior { get; set; }

		//[Display(Name = "DN_idUsuario_alta", ResourceType = typeof(app_GlobalResources.Content))]
        [Display(Name = "DN_idUsuario_alta")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0}")]
		public decimal idUsuario_alta { get; set; }

		//[Display(Name = "DN_fecha_alta", ResourceType = typeof(app_GlobalResources.Content))]
        [Display(Name = "DN_fecha_alta")]
		[DataType(DataType.DateTime)]
        //[Required (ErrorMessageResourceName = "DT_fecha_alta", ErrorMessageResourceType = typeof(app_GlobalResources.Content))]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:g}")]
		public System.DateTime fecha_alta { get; set; }

		[Display(Name = "DN_activo", ResourceType = typeof(app_GlobalResources.Content))]
		public bool activo { get; set; }

	}
}
