using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace proera.Controllers
{
    
    public class HomeController : Controller
    {
        private Data_PROERA db = new Data_PROERA();
        public ActionResult Index()
        {
            ViewBag.nbrclients = db.clients.Count();
            ViewBag.nbrclientsservis = db.clients.Where(c => c.Etat_Client == 1).Count();
            ViewBag.nbrclientsattente = db.clients.Where(c => c.Etat_Client == 5).Count();
            ViewBag.totalvillages = db.villages.Count();
                        return View();
        }

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

        public ActionResult tabCommercial()
        {
            ViewBag.Message = "Your contact page.";

            return View("commercial");
        }
    }
}