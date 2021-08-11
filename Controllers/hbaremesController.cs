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
    public class hbaremesController : Controller
    {
        private PROERAEntities1 db = new PROERAEntities1();

        // GET: hbaremes
        public ActionResult Index()
        {
            var hbaremes = db.hbaremes.Include(h => h.hbranches).Include(h => h.hcalibres).Include(h => h.hnivpuissances);
            return View(hbaremes.ToList());
        }

        // GET: hbaremes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hbaremes hbaremes = db.hbaremes.Find(id);
            if (hbaremes == null)
            {
                return HttpNotFound();
            }
            return View(hbaremes);
        }

        // GET: hbaremes/Create
        public ActionResult Create()
        {
            ViewBag.branch = new SelectList(db.hbranches, "Id", "Branch");
            ViewBag.Calibre2 = new SelectList(db.hcalibres, "Id", "Calibre");
            ViewBag.NiveauPuissance = new SelectList(db.hnivpuissances, "Id", "NivPuissance");
            ViewBag.usage = new SelectList(db.usages, "id", "usage");
            return View();
        }

        // POST: hbaremes/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Puissance,Montant,NiveauPuissance,Calibre2,branch,Usage")] hbaremes hbaremes)
        {
            if (ModelState.IsValid)
            {
                db.hbaremes.Add(hbaremes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.branch = new SelectList(db.hbranches, "Id", "Branch", hbaremes.branch);
            ViewBag.Calibre2 = new SelectList(db.hcalibres, "Id", "Calibre", hbaremes.Calibre2);
            ViewBag.NiveauPuissance = new SelectList(db.hnivpuissances, "Id", "NivPuissance", hbaremes.NiveauPuissance);
            return View(hbaremes);
        }

        // GET: hbaremes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hbaremes hbaremes = db.hbaremes.Find(id);
            if (hbaremes == null)
            {
                return HttpNotFound();
            }
            ViewBag.branch = new SelectList(db.hbranches, "Id", "Branch", hbaremes.branch);
            ViewBag.Calibre2 = new SelectList(db.hcalibres, "Id", "Calibre", hbaremes.Calibre2);
            ViewBag.NiveauPuissance = new SelectList(db.hnivpuissances, "Id", "NivPuissance", hbaremes.NiveauPuissance);
            return View(hbaremes);
        }

        // POST: hbaremes/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Puissance,Montant,NiveauPuissance,Calibre2,branch,Usage")] hbaremes hbaremes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hbaremes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.branch = new SelectList(db.hbranches, "Id", "Branch", hbaremes.branch);
            ViewBag.Calibre2 = new SelectList(db.hcalibres, "Id", "Calibre", hbaremes.Calibre2);
            ViewBag.NiveauPuissance = new SelectList(db.hnivpuissances, "Id", "NivPuissance", hbaremes.NiveauPuissance);
            return View(hbaremes);
        }

        // GET: hbaremes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hbaremes hbaremes = db.hbaremes.Find(id);
            if (hbaremes == null)
            {
                return HttpNotFound();
            }
            return View(hbaremes);
        }

        // POST: hbaremes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            hbaremes hbaremes = db.hbaremes.Find(id);
            db.hbaremes.Remove(hbaremes);
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
