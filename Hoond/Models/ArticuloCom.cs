using System;
using System.Collections.Generic;

namespace Hoond.Models
{
    [Serializable]
    public class ArticuloCom
    {
        public decimal id { get; set; }
        public string sku { get; set; }
        public string barcode { get; set; }
        public string color { get; set; }
        public string talla { get; set; }
        public string claveColor { get; set; }
        public string claveTalla { get; set; }
    }

    [Serializable]
    public class ArticuloItem
    {
        public decimal idProveedor { get; set; }
        public decimal idTipoArticulo { get; set; }
        public decimal idMarca { get; set; }
        public decimal idColeccion { get; set; }
        public decimal temporada { get; set; }
        public decimal idCategoria { get; set; }
        public decimal idSubCategoria { get; set; }
        public string claveArticulo { get; set; }
        public string descArticulo { get; set; }
        public string nombreArticulo { get; set; }
        public string claveColor { get; set; }
        public string claveTalla { get; set; }
        public decimal idPresentacion { get; set; }
        public decimal cantidadPresentacion { get; set; }
    }

}