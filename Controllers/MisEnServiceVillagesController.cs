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

    [Authorize(Roles = "Proera_SIG,Proera_Admin")]
    public class MisEnServiceVillagesController : Controller
    {
        private PROERAEntities db = new PROERAEntities();

        // GET: MisEnServiceVillages
        public ActionResult Index()
        {
            var misEnServiceVillages = db.MisEnServiceVillages.Include(m => m.villages);
            return View(misEnServiceVillages.ToList());
        }

        // GET: MisEnServiceVillages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MisEnServiceVillages misEnServiceVillages = db.MisEnServiceVillages.Find(id);
            if (misEnServiceVillages == null)
            {
                return HttpNotFound();
            }
            return View(misEnServiceVillages);
        }

        // GET: MisEnServiceVillages/Create
        public ActionResult Create()
        {

            var region = db.regions.ToList();
            var idregion = region[0].id;
            ViewBag.region = new SelectList(region, "id", "nom_region");
            var depts = db.departements.Where(d => d.idregion == idregion).ToList();
            ViewBag.departement = new SelectList(depts, "code_departement", "nom");
            var iddept = depts[0].code_departement;
            var coms = db.communes.Where(c => c.iddepartement == iddept).ToList();
            ViewBag.idLocalite = new SelectList(db.communes, "code_com", "nom");
            return View();
        }

        // POST: MisEnServiceVillages/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_SERVICE,REGION,DEPARTEMENT,COMMUNE,VILLAGE,dateRaccPremierClient,Etat_travaux,code_village,CapaciteTransformateur,PuissanceDHP,nombreSupport,LineaireBT,datemiseenservice")] MisEnServiceVillages misEnServiceVillages)
        {
            if (ModelState.IsValid)
            {
                db.MisEnServiceVillages.Add(misEnServiceVillages);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.code_village = new SelectList(db.villages, "code_village", "region", misEnServiceVillages.code_village);
            return View(misEnServiceVillages);
        }

        // GET: MisEnServiceVillages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MisEnServiceVillages misEnServiceVillages = db.MisEnServiceVillages.Find(id);
            if (misEnServiceVillages == null)
            {
                return HttpNotFound();
            }
            ViewBag.code_village = new SelectList(db.villages, "code_village", "region", misEnServiceVillages.code_village);
            return View(misEnServiceVillages);
        }

        public ActionResult mettreenservice2(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            villages villages = db.villages.Find(id);
            if (villages == null)
            {
                return HttpNotFound();
            }

            var mes = new MisEnServiceVillages
            {
                code_village = villages.code_village
            };
            return View(mes);
        }

        // POST: MisEnServiceVillages/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_SERVICE,REGION,DEPARTEMENT,COMMUNE,VILLAGE,dateRaccPremierClient,Etat_travaux,code_village,CapaciteTransformateur,PuissanceDHP,nombreSupport,LineaireBT, datemiseenservice")] MisEnServiceVillages misEnServiceVillages)
        {
            if (ModelState.IsValid)
            {
                db.Entry(misEnServiceVillages).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.code_village = new SelectList(db.villages, "code_village", "region", misEnServiceVillages.code_village);
            return View(misEnServiceVillages);
        }

        // GET: MisEnServiceVillages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MisEnServiceVillages misEnServiceVillages = db.MisEnServiceVillages.Find(id);
            if (misEnServiceVillages == null)
            {
                return HttpNotFound();
            }
            return View(misEnServiceVillages);
        }

        // POST: MisEnServiceVillages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MisEnServiceVillages misEnServiceVillages = db.MisEnServiceVillages.Find(id);
            db.MisEnServiceVillages.Remove(misEnServiceVillages);
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
