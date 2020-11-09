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
    public class migrationsController : Controller
    {
        private ERADEVEntities3 db = new ERADEVEntities3();

        // GET: migrations
        public ActionResult Index()
        {
            var migration = db.migration.Include(m => m.clients);
            return View(migration.ToList());
        }

        // GET: migrations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            migration migration = db.migration.Find(id);
            if (migration == null)
            {
                return HttpNotFound();
            }
            return View(migration);
        }

        // GET: migrations/Create
        public ActionResult Create()
        {
            ViewBag.referenceclient = new SelectList(db.clients, "Reference_Contrat", "Nom1");
            return View();
        }

        // POST: migrations/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,anciennivpui,nouveaunivpui,frais,referenceclient,datemigration,interface,index")] migration migration)
        {
            if (ModelState.IsValid)
            {
                db.migration.Add(migration);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.referenceclient = new SelectList(db.clients, "Reference_Contrat", "Nom1", migration.referenceclient);
            return View(migration);
        }

        // GET: migrations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            migration migration = db.migration.Find(id);
            if (migration == null)
            {
                return HttpNotFound();
            }
            ViewBag.referenceclient = new SelectList(db.clients, "Reference_Contrat", "Nom1", migration.referenceclient);
            return View(migration);
        }

        // POST: migrations/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,anciennivpui,nouveaunivpui,frais,referenceclient,datemigration,interface,index")] migration migration)
        {
            if (ModelState.IsValid)
            {
                db.Entry(migration).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.referenceclient = new SelectList(db.clients, "Reference_Contrat", "Nom1", migration.referenceclient);
            return View(migration);
        }

        // GET: migrations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            migration migration = db.migration.Find(id);
            if (migration == null)
            {
                return HttpNotFound();
            }
            return View(migration);
        }

        // POST: migrations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            migration migration = db.migration.Find(id);
            db.migration.Remove(migration);
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
