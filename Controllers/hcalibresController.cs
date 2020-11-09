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
    public class hcalibresController : Controller
    {
        private ERADEVEntities3 db = new ERADEVEntities3();

        // GET: hcalibres
        public ActionResult Index()
        {
            return View(db.hcalibres.ToList());
        }

        // GET: hcalibres/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hcalibres hcalibres = db.hcalibres.Find(id);
            if (hcalibres == null)
            {
                return HttpNotFound();
            }
            return View(hcalibres);
        }

        // GET: hcalibres/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: hcalibres/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Calibre")] hcalibres hcalibres)
        {
            if (ModelState.IsValid)
            {
                db.hcalibres.Add(hcalibres);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hcalibres);
        }

        // GET: hcalibres/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hcalibres hcalibres = db.hcalibres.Find(id);
            if (hcalibres == null)
            {
                return HttpNotFound();
            }
            return View(hcalibres);
        }

        // POST: hcalibres/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Calibre")] hcalibres hcalibres)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hcalibres).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hcalibres);
        }

        // GET: hcalibres/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hcalibres hcalibres = db.hcalibres.Find(id);
            if (hcalibres == null)
            {
                return HttpNotFound();
            }
            return View(hcalibres);
        }

        // POST: hcalibres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            hcalibres hcalibres = db.hcalibres.Find(id);
            db.hcalibres.Remove(hcalibres);
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
