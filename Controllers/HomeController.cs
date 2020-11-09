using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace proera.Controllers
{
    
    public class HomeController : Controller
    {

        private ERADEVEntities3 db = new ERADEVEntities3();
        public ActionResult Index()
        {
            ViewBag.nbrclients = db.clients.Count();
            ViewBag.nbrclientsservis = db.clients.Where(c => c.Etat_Client == 1).Count();
            ViewBag.nbrclientsattente = ViewBag.nbrclients - ViewBag.nbrclientsservis;
            ViewBag.totalvillages = db.villages.Count();
                        return View();
        }

        [Authorize(Roles = "Administrateurs")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}