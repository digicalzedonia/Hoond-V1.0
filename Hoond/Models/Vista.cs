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
    
    public partial class Vista
    {
        public Vista()
        {
            this.Menu = new HashSet<Menu>();
        }
    
        public decimal idVista { get; set; }
        public string descVista { get; set; }
        public string controller { get; set; }
        public string action { get; set; }
        public decimal idIcono { get; set; }
        public string parametro { get; set; }
        public decimal idUsuario_alta { get; set; }
        public System.DateTime fecha_alta { get; set; }
        public bool activo { get; set; }
    
        public virtual Icono Icono { get; set; }
        public virtual ICollection<Menu> Menu { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
