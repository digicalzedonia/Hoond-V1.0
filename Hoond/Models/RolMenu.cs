//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Hoond.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class RolMenu
    {
        public decimal idRol { get; set; }
        public decimal idMenu { get; set; }
        public decimal idTipoPermiso { get; set; }
        public decimal idUsuario_alta { get; set; }
        public System.DateTime fecha_alta { get; set; }
        public bool activo { get; set; }
    
        public virtual Menu Menu { get; set; }
        public virtual Rol Rol { get; set; }
        public virtual TipoPermiso TipoPermiso { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
