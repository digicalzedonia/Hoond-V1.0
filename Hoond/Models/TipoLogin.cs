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
    
    public partial class TipoLogin
    {
        public TipoLogin()
        {
            this.Usuario1 = new HashSet<Usuario>();
        }
    
        public decimal idTipoLogin { get; set; }
        public string descTipoLogin { get; set; }
        public decimal idUsuario_alta { get; set; }
        public System.DateTime fecha_alta { get; set; }
        public bool activo { get; set; }
    
        public virtual Usuario Usuario { get; set; }
        public virtual ICollection<Usuario> Usuario1 { get; set; }
    }
}
