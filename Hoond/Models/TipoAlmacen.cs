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
    
    public partial class TipoAlmacen
    {
        public TipoAlmacen()
        {
            this.Almacen = new HashSet<Almacen>();
        }
    
        public decimal idTipoAlmacen { get; set; }
        public decimal idEmpresa { get; set; }
        public string descTipoAlmacen { get; set; }
        public decimal idUsuario_alta { get; set; }
        public System.DateTime fecha_alta { get; set; }
        public bool activo { get; set; }
    
        public virtual Empresa Empresa { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual ICollection<Almacen> Almacen { get; set; }
    }
}
