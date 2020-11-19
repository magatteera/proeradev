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

    [Authorize(Roles = "Proera_Admin")]
    [Authorize(Users = "i.dia")]
    public class regionsController : Controller
    {
        private ERADEVEntities3 db = new ERADEVEntities3();

        // GET: regions
        public ActionResult Index()
        {
            return View(db.regions.ToList());
        }

        // GET: regions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            regions regions = db.regions.Find(id);
            if (regions == null)
            {
                return HttpNotFound();
            }
            return View(regions);
        }

        // GET: regions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: regions/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nom_region,geom")] regions regions)
        {
            if (ModelState.IsValid)
            {
                db.regions.Add(regions);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(regions);
        }

        // GET: regions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            regions regions = db.regions.Find(id);
            if (regions == null)
            {
                return HttpNotFound();
            }
            return View(regions);
        }

        // POST: regions/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nom_region,geom")] regions regions)
        {
            if (ModelState.IsValid)
            {
                db.Entry(regions).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(regions);
        }

        // GET: regions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            regions regions = db.regions.Find(id);
            if (regions == null)
            {
                return HttpNotFound();
            }
            return View(regions);
        }

        // POST: regions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            regions regions = db.regions.Find(id);
            db.regions.Remove(regions);
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
