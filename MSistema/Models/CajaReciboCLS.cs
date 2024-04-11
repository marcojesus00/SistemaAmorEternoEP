using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MSistema.Models
{
    public class CajaReciboCLS
    {
        [Key]
        public string caj_numero { get; set; }


        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]

        public Nullable<System.DateTime> caj_fecha { get; set; }
        [DisplayName("Valor")]
        public Nullable<decimal> caj_valor { get; set; }
        [DisplayName("Cliente")]
        public string caj_cliente { get; set; }
        [DisplayName("Nombre")]
        public string caj_nombre { get; set; }
        [DisplayName("Concepto")]
        public string caj_concept { get; set; }
        [DisplayName("Concepto2")]
        public string caj_concep2 { get; set; }
        [DisplayName("Cobrador")]
        public string caj_cobrado { get; set; }
        [DisplayName("Compañia")]
        public string caj_compa { get; set; }
        [DisplayName("Sucursal")]
        public string caj_sucurs { get; set; }
        [DisplayName("Hora")]
        public string CAJ_HORA { get; set; }
        [DisplayName("Usuario")]
        public string CAJ_USUARIO { get; set; }
        
    }
}