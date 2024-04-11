using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MSistema.Models
{
    public class RecibosCLS
    {
        [Key]
        [DisplayName("Documento")]
        public string Num_doc { get; set; }
        public string Tdoc { get; set; }
        public string rtipodoc { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> Fecha_recib { get; set; }


        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> RFECHA { get; set; }
        public string RCONCEPTO { get; set; }
        public string RCONCEP2 { get; set; }
        public Nullable<decimal> RCAMBIO { get; set; }
        public Nullable<decimal> Por_lempira { get; set; }
        public string rlugar { get; set; }
        public string rhora { get; set; }
        public string COBRADO_EN { get; set; }
        public string rtermino { get; set; }
        public string Codigo_clie { get; set; }
        public string rcodigocli { get; set; }
        public string CXC_CTIPO { get; set; }
        public string CXC_PARTIDA { get; set; }
        public string rusuario { get; set; }
        public string rterminal { get; set; }
        public string RECIBMODIFI { get; set; }
        public Nullable<System.DateTime> RECIBFMODIF { get; set; }
        public Nullable<short> rduradeb { get; set; }
        public string RTIPODEBI { get; set; }
        public string rsucursal { get; set; }
        public string codigo_cobr { get; set; }
        public string RNOMBRECLI { get; set; }
        public Nullable<decimal> rprima { get; set; }
        public Nullable<short> rno_letras { get; set; }
        public Nullable<decimal> VEFECTI { get; set; }
        public Nullable<decimal> VCHEQUE { get; set; }
        public string PAGOCK { get; set; }
        public string PAGOBANCO { get; set; }
        public string RECIBCUENTA { get; set; }
        public string rconciliado { get; set; }
        public string rnumefa { get; set; }
        public Nullable<decimal> RCUOTA { get; set; }
        public string RCODVEND { get; set; }
        public string rcomp { get; set; }
        public string rlider { get; set; }
        public string rsupervi { get; set; }

        //Campos Llaves
        public string NombreCliente { get; set; }
        public string ReciboCaja { get; set; }
        
    }
}