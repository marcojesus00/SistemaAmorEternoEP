//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MSistema.Contexto
{
    using System;
    using System.Collections.Generic;
    
    public partial class GestionCobro
    {
        public int idLine { get; set; }
        public string Codigo_clie { get; set; }
        public string Nombre_clie { get; set; }
        public string Cont_numero { get; set; }
        public string Cobrador { get; set; }
        public Nullable<System.DateTime> Fecha { get; set; }
        public string Hora { get; set; }
        public Nullable<int> CodigoMotivo { get; set; }
        public string Motivo { get; set; }
        public string Comentario { get; set; }
        public string Lider { get; set; }
    }
}
