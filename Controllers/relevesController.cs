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
    public class relevesController : Controller
    {
        private PROERAEntities db = new PROERAEntities();

        // GET: releves
        public ActionResult Index()
        {
            var releves = db.releves.Include(r => r.clients);
            return View(releves.ToList());
        }

        // GET: releves/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            releves releves = db.releves.Find(id);
            if (releves == null)
            {
                return HttpNotFound();
            }
            return View(releves);
        }

        // GET: releves/Create
        public ActionResult Create()
        {
            //ViewBag.Reference_Contrat = new SelectList(db.clients, "Reference_Contrat", "Nom1");
            var periode = db.recouvrements.Where(r => r.active == 1).ToList();
            ViewBag.periode = new SelectList(periode, "periode", "periode");
            return View();
        }

        // POST: releves/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Reference_Contrat,N__COMPTEUR,Ancien_index,Nouvel_index,consommation,date_de_relève,nom1,prenom,village,commune,periode,prevbill,nivpuissance,nbreJour,departement,region,categorie,nivservice")] releves releves)
        {
            if (ModelState.IsValid)
            {
                db.releves.Add(releves);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Reference_Contrat = new SelectList(db.clients, "Reference_Contrat", "Nom1", releves.Reference_Contrat);
            return View(releves);
        }

        public ActionResult changementreference([Bind(Include = "Reference_Contrat")] releves releves)
        {
            var client = db.clients.Where(c => c.Reference_Contrat == releves.Reference_Contrat).ToList();
            var relevess = db.releves.Where(r => r.Reference_Contrat == releves.Reference_Contrat).ToList();
            var relevesss = relevess.OrderByDescending(r => r.ID).ToList();
            if (client.Count > 0)
            {
                return Json(new { message = "found", ancienindex = relevesss[0].Nouvel_index });
            }
            else
                return Json(new { message = "inexistant" });
        }
        
        // GET: releves/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            releves releves = db.releves.Find(id);
            if (releves == null)
            {
                return HttpNotFound();
            }
            ViewBag.Reference_Contrat = new SelectList(db.clients, "Reference_Contrat", "Nom1", releves.Reference_Contrat);
            return View(releves);
        }

        // POST: releves/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,id2,Reference_Contrat,N__COMPTEUR,Ancien_index,Nouvel_index,consommation,date_de_relève,nom1,prenom,village,commune,periode,prevbill,nivpuissance,nbreJour,departement,region,categorie,nivservice")] releves releves)
        {
            if (ModelState.IsValid)
            {
                db.Entry(releves).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Reference_Contrat = new SelectList(db.clients, "Reference_Contrat", "Nom1", releves.Reference_Contrat);
            return View(releves);
        }

        // GET: releves/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            releves releves = db.releves.Find(id);
            if (releves == null)
            {
                return HttpNotFound();
            }
            return View(releves);
        }

        // POST: releves/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            releves releves = db.releves.Find(id);
            db.releves.Remove(releves);
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
