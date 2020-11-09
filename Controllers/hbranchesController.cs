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
    public class hbranchesController : Controller
    {
        private ERADEVEntities3 db = new ERADEVEntities3();

        // GET: hbranches
        public ActionResult Index()
        {
            return View(db.hbranches.ToList());
        }

        // GET: hbranches/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hbranches hbranches = db.hbranches.Find(id);
            if (hbranches == null)
            {
                return HttpNotFound();
            }
            return View(hbranches);
        }

        // GET: hbranches/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: hbranches/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Branch")] hbranches hbranches)
        {
            if (ModelState.IsValid)
            {
                db.hbranches.Add(hbranches);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hbranches);
        }

        // GET: hbranches/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hbranches hbranches = db.hbranches.Find(id);
            if (hbranches == null)
            {
                return HttpNotFound();
            }
            return View(hbranches);
        }

        // POST: hbranches/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Branch")] hbranches hbranches)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hbranches).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hbranches);
        }

        // GET: hbranches/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hbranches hbranches = db.hbranches.Find(id);
            if (hbranches == null)
            {
                return HttpNotFound();
            }
            return View(hbranches);
        }

        // POST: hbranches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            hbranches hbranches = db.hbranches.Find(id);
            db.hbranches.Remove(hbranches);
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
