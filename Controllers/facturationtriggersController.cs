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
    [Authorize(Roles = "Proera_REC, Proera_Admin")]
    public class facturationtriggersController : Controller
    {
        private PROERAEntities db = new PROERAEntities();

        // GET: facturationtriggers
        public ActionResult Index()
        {
            return View(db.facturationtrigger.ToList());
        }

        // GET: facturationtriggers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            facturationtrigger facturationtrigger = db.facturationtrigger.Find(id);
            if (facturationtrigger == null)
            {
                return HttpNotFound();
            }
            return View(facturationtrigger);
        }

        // GET: facturationtriggers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: facturationtriggers/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,datefacturation,periode,utilisateur")] facturationtrigger facturationtrigger)
        {
            if (ModelState.IsValid)
            {
                db.facturationtrigger.Add(facturationtrigger);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(facturationtrigger);
        }

        public ActionResult veliderFacturation([Bind(Include = "datefacturation,periode")] facturationtrigger facturationtrigger)
        {
            if (ModelState.IsValid)
            {
                facturationtrigger.utilisateur = User.Identity.Name;
                db.facturationtrigger.Add(facturationtrigger);
                db.SaveChanges();
                return RedirectToAction("facturer");
            }
            ViewBag.fait = 1;
            return View(facturationtrigger);
        }

        
        public ActionResult facturer()
        {
            var recouvs = db.recouvrements.Where(r => (r.active == 1) && (r.facturee == 0)).ToList();
            ViewBag.periode = new SelectList(recouvs, "periode", "periode");

            return View("~/Views/facturationtriggers/create.cshtml");
        }
        // GET: facturationtriggers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            facturationtrigger facturationtrigger = db.facturationtrigger.Find(id);
            if (facturationtrigger == null)
            {
                return HttpNotFound();
            }
            return View(facturationtrigger);
        }

        // POST: facturationtriggers/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,datefacturation,periode,utilisateur")] facturationtrigger facturationtrigger)
        {
            if (ModelState.IsValid)
            {
                db.Entry(facturationtrigger).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(facturationtrigger);
        }

        // GET: facturationtriggers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            facturationtrigger facturationtrigger = db.facturationtrigger.Find(id);
            if (facturationtrigger == null)
            {
                return HttpNotFound();
            }
            return View(facturationtrigger);
        }

        // POST: facturationtriggers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            facturationtrigger facturationtrigger = db.facturationtrigger.Find(id);
            db.facturationtrigger.Remove(facturationtrigger);
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
