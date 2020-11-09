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
    public class hnivpuissancesController : Controller
    {
        private ERADEVEntities3 db = new ERADEVEntities3();

        // GET: hnivpuissances
        public ActionResult Index()
        {
            var hnivpuissances = db.hnivpuissances.Include(h => h.usages);
            return View(hnivpuissances.ToList());
        }

        // GET: hnivpuissances/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hnivpuissances hnivpuissances = db.hnivpuissances.Find(id);
            if (hnivpuissances == null)
            {
                return HttpNotFound();
            }
            return View(hnivpuissances);
        }

        // GET: hnivpuissances/Create
        public ActionResult Create()
        {
            ViewBag.Usage = new SelectList(db.usages, "id", "usage");
            return View();
        }

        // POST: hnivpuissances/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,NivPuissance,Usage")] hnivpuissances hnivpuissances)
        {
            if (ModelState.IsValid)
            {
                db.hnivpuissances.Add(hnivpuissances);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Usage = new SelectList(db.usages, "id", "usage", hnivpuissances.Usage);
            return View(hnivpuissances);
        }

        // GET: hnivpuissances/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hnivpuissances hnivpuissances = db.hnivpuissances.Find(id);
            if (hnivpuissances == null)
            {
                return HttpNotFound();
            }
            ViewBag.Usage = new SelectList(db.usages, "id", "usage", hnivpuissances.Usage);
            return View(hnivpuissances);
        }

        // POST: hnivpuissances/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,NivPuissance,Usage")] hnivpuissances hnivpuissances)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hnivpuissances).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Usage = new SelectList(db.usages, "id", "usage", hnivpuissances.Usage);
            return View(hnivpuissances);
        }

        // GET: hnivpuissances/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hnivpuissances hnivpuissances = db.hnivpuissances.Find(id);
            if (hnivpuissances == null)
            {
                return HttpNotFound();
            }
            return View(hnivpuissances);
        }

        // POST: hnivpuissances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            hnivpuissances hnivpuissances = db.hnivpuissances.Find(id);
            db.hnivpuissances.Remove(hnivpuissances);
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
