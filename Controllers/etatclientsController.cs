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
    public class etatclientsController : Controller
    {
        private PROERAEntities db = new PROERAEntities();

        // GET: etatclients
        public ActionResult Index()
        {
            return View(db.etatclient.ToList());
        }

        // GET: etatclients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            etatclient etatclient = db.etatclient.Find(id);
            if (etatclient == null)
            {
                return HttpNotFound();
            }
            return View(etatclient);
        }

        // GET: etatclients/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: etatclients/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,etat")] etatclient etatclient)
        {
            if (ModelState.IsValid)
            {
                db.etatclient.Add(etatclient);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(etatclient);
        }

        // GET: etatclients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            etatclient etatclient = db.etatclient.Find(id);
            if (etatclient == null)
            {
                return HttpNotFound();
            }
            return View(etatclient);
        }

        // POST: etatclients/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,etat")] etatclient etatclient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(etatclient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(etatclient);
        }

        // GET: etatclients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            etatclient etatclient = db.etatclient.Find(id);
            if (etatclient == null)
            {
                return HttpNotFound();
            }
            return View(etatclient);
        }

        // POST: etatclients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            etatclient etatclient = db.etatclient.Find(id);
            db.etatclient.Remove(etatclient);
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
