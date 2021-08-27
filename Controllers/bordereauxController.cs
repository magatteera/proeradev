using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using proera;

namespace proera.Controllers
{
    public class bordereauxController : Controller
    {
        private PROERAEntities db = new PROERAEntities();

        // GET: bordereaux
        public ActionResult Index()
        {
            return View(db.bordereaux.ToList());
        }


        [Authorize(Roles = "Proera_DAF, Proera_Admin")]
        public ActionResult bordereauxdaf()
        {
            return View("listedaf");
        }


        [Authorize(Roles = "Proera_ENC, Proera_Admin")]
        public ActionResult bordereauxcaisse()
        {
            return View("listecaisse");
        }

        //chargerbordereaux

            



        public ActionResult chargerbordereauxdaf([Bind(Include = "ouvert")] bordereaux bord)
        {
            var bords = new List<bordereaux>();
            if (bord.ouvert == 1)
            {
                bords = db.bordereaux.Where(b => (b.ouvert == 1) && (b.type == "encaissement")).ToList();
            } else
            {
                bords = db.bordereaux.Where(b => (b.ouvert == 0) && (b.type == "encaissement")).ToList();
            }

            var listebr = "";

            foreach (bordereaux br in bords)
            {
                listebr += "<tr>" +
                    "<td>" + br.numero + "</td>" +
                    "<td>" + br.utilisateur + "</td>" +
                    "<td>" + db.encaissements.Where(e => e.idbordereau == br.id).Sum(e => e.montantencaisee) + "</td>" +
                    "<td>" + br.montant + "</td>" +
                    "<td>" + "<button class='btn btn-warning btn-sm' id='btnmodifier-"+br.id+ "' montant='" + br.montant + "' idbord='" + br.id+"'>Modifier</button>" + "</td>" +
                    "</tr>";
            }

            return Json(new
            {
                listebrs = listebr
            });
        }

        public ActionResult chargerbordereauxcaisse([Bind(Include = "ouvert")] bordereaux bord)
        {
            var bords = new List<bordereaux>();
            if (bord.ouvert == 1)
            {
                bords = db.bordereaux.Where(b => (b.ouvert == 1) && (b.type == "encaissement") && (b.utilisateur == User.Identity.Name)).ToList();
            }
            else
            {
                bords = db.bordereaux.Where(b => (b.ouvert == 0) && (b.type == "encaissement") && (b.utilisateur == User.Identity.Name)).ToList();
            }

            var listebr = "";

            foreach (bordereaux br in bords)
            {
                listebr += "<tr>" +
                    "<td>" + br.numero + "</td>" +
                    "<td>" + br.utilisateur + "</td>" +
                    "<td>" + db.encaissements.Where(e => e.idbordereau == br.id).Sum(e => e.montantencaisee) + "</td>" +
                    "<td>" + br.montant + "</td>" +
                    "<td>" + "<button class='btn btn-warning btn-sm' id='btnmodifier-" + br.id + "' montant='" + br.montant + "' idbord='" + br.id + "'>Modifier</button>" + "</td>" +
                    "</tr>";
            }

            return Json(new
            {
                listebrs = listebr
            });
        }




        // GET: bordereaux/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bordereaux bordereaux = db.bordereaux.Find(id);
            if (bordereaux == null)
            {
                return HttpNotFound();
            }
            return View(bordereaux);
        }

        // GET: bordereaux/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: bordereaux/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,numero,datecreation,montant,ouvert,utilisateur,datevalidation,type")] bordereaux bordereaux)
        {
            if (ModelState.IsValid)
            {
                bordereaux.ouvert = 1;
                bordereaux.utilisateur = User.Identity.Name;
                db.bordereaux.Add(bordereaux);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bordereaux);
        }

        // GET: bordereaux/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bordereaux bordereaux = db.bordereaux.Find(id);
            if (bordereaux == null)
            {
                return HttpNotFound();
            }
            return View(bordereaux);
        }

        // POST: bordereaux/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,montant, type")] bordereaux bordereaux)
        {
            var bord = db.bordereaux.Find(bordereaux.id);
            bord.montant = bordereaux.montant;
            if (ModelState.IsValid)
            {
                db.Entry(bord).State = EntityState.Modified;
                db.SaveChanges();
                if(bordereaux.type == "daf")
                    return RedirectToAction("bordereauxdaf");
                else
                    return RedirectToAction("bordereauxcaisse");
            }
            if (bordereaux.type == "daf")
                return RedirectToAction("bordereauxdaf");
            else
                return RedirectToAction("bordereauxcaisse");
        }

        // GET: bordereaux/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bordereaux bordereaux = db.bordereaux.Find(id);
            if (bordereaux == null)
            {
                return HttpNotFound();
            }
            return View(bordereaux);
        }

        // POST: bordereaux/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            bordereaux bordereaux = db.bordereaux.Find(id);
            db.bordereaux.Remove(bordereaux);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
