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
    
    public partial class ArticuloCaracteristica
    {
        public decimal idArticulo { get; set; }
        public decimal idCaracteristica { get; set; }
        public decimal idValorCaracteristica { get; set; }
        public decimal idUsuario_alta { get; set; }
        public System.DateTime fecha_alta { get; set; }
        public bool activo { get; set; }
    
        public virtual Usuario Usuario { get; set; }
        public virtual Articulo Articulo { get; set; }
        public virtual Caracteristica Caracteristica { get; set; }
        public virtual ValorCaracteristica ValorCaracteristica { get; set; }
    }
}
