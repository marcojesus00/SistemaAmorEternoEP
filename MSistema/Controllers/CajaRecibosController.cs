using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MSistema.Contexto;
using MSistema.Models;
namespace MSistema.Controllers
{
    public class CajaRecibosController : Controller
    {

        CajaReciboBL _cajreci;

        public CajaRecibosController()
        {
            _cajreci = new CajaReciboBL();
        }


        // GET: CajaRecibos
        public ActionResult Index()
        {
            
            var listacaja = _cajreci.ObtieneCajRecib();

                      return View(listacaja);
        }
    }
}