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
    public class usagesController : Controller
    {
        private PROERAEntities db = new PROERAEntities();

        // GET: usages
        public ActionResult Index()
        {
            return View(db.usages.ToList());
        }

        // GET: usages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            usages usages = db.usages.Find(id);
            if (usages == null)
            {
                return HttpNotFound();
            }
            return View(usages);
        }

        // GET: usages/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: usages/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,usage")] usages usages)
        {
            if (ModelState.IsValid)
            {
                db.usages.Add(usages);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(usages);
        }

        // GET: usages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            usages usages = db.usages.Find(id);
            if (usages == null)
            {
                return HttpNotFound();
            }
            return View(usages);
        }

        // POST: usages/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,usage")] usages usages)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usages).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usages);
        }

        // GET: usages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            usages usages = db.usages.Find(id);
            if (usages == null)
            {
                return HttpNotFound();
            }
            return View(usages);
        }

        // POST: usages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            usages usages = db.usages.Find(id);
            db.usages.Remove(usages);
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
