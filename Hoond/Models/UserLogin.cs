using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Hoond.Models
{
    [Serializable]
    public class UserLogin
    {
        public bool isAdmin { get; set; }
        public decimal idRol { get; set; }
        public decimal idUsuario { get; set; }
        public decimal idEmpresa { get; set; }
        public string nombreUsuario { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "titulo_NombreUsuario", ResourceType = typeof(app_GlobalResources.Content))]
        public string username { get; set; }
        public string SortField { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name = "titulo_Password", ResourceType = typeof(app_GlobalResources.Content))]
        [DataType(DataType.Password)]
        public string password { get; set; }
        public string sessionId { get; set; }

    }

    public class Cpassw
    {
        public decimal idUsuario { get; set; }

        [Display(Name = "titulo_NombreUsuario", ResourceType = typeof(app_GlobalResources.Content))]
        public string nombreUsuario { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name = "titulo_NewPassword", ResourceType = typeof(app_GlobalResources.Content))]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name = "titulo_ConfPassword", ResourceType = typeof(app_GlobalResources.Content))]
        [DataType(DataType.Password)]
        public string passwordC { get; set; }

    }

    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }

    public class RegisterExternalLoginModel
    {
        [Required]
        [StringLength(20)]
        [Display(Name = "titulo_NombreUsuario", ResourceType = typeof(app_GlobalResources.Content))]
        public string nombreusuario { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name = "titulo_Usuario", ResourceType = typeof(app_GlobalResources.Content))]
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string accesstoken { get; set; }
        public string id { get; set; }
        public string provider { get; set; }
    }

}