using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MSistema.Contexto;

namespace MSistema.Models
{
    public class CajaReciboBL

    {
        FUNAMOREntities _contextoFunamor = new FUNAMOREntities();

        public List<CajaReciboCLS> ListadeCaja { get; set; }

        public List<CajaReciboCLS> ObtieneCajRecib()
        {

            
                List<CajaReciboCLS> ListadeCaja = (from caja in _contextoFunamor.CAJRECI
                                                   where caja.CAJ_USUARIO == "Ramses" && caja.caj_fecha == DateTime.Today
                                                   select new CajaReciboCLS
                                                   {
                                                       caj_numero = caja.caj_numero,
                                                       caj_cliente = caja.caj_cliente,
                                                       caj_fecha = caja.caj_fecha,
                                                       caj_valor = caja.caj_valor,
                                                       caj_cobrado = caja.caj_cobrado,
                                                       caj_compa = caja.caj_compa,
                                                       caj_concept = caja.caj_concept,
                                                       //caj_concep2 = caja.caj_concep2,
                                                       caj_nombre = caja.caj_nombre,
                                                       CAJ_HORA = caja.CAJ_HORA,
                                                       caj_sucurs = caja.caj_sucurs,
                                                       CAJ_USUARIO = caja.CAJ_USUARIO,

                                                   }).ToList();
            

            return ListadeCaja;
        }
        
    }
}