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
    public class miseenserviceclientsController : Controller
    {
        private ERADEVEntities3 db = new ERADEVEntities3();

        // GET: miseenserviceclients
        public ActionResult Index()
        {
            var miseenserviceclient = db.miseenserviceclient.Include(m => m.clients);
            return View(miseenserviceclient.ToList());
        }

        // GET: miseenserviceclients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            miseenserviceclient miseenserviceclient = db.miseenserviceclient.Find(id);
            if (miseenserviceclient == null)
            {
                return HttpNotFound();
            }
            return View(miseenserviceclient);
        }

        // GET: miseenserviceclients/Create
        public ActionResult Create()
        {
            ViewBag.referenceClient = new SelectList(db.clients, "Reference_Contrat", "Nom1");
            return View();
        }

        // POST: miseenserviceclients/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,interface,indexDePose,dateMiseEnService,referenceClient,statut,utilisateur")] miseenserviceclient miseenserviceclient)
        {
            if (ModelState.IsValid)
            {
                db.miseenserviceclient.Add(miseenserviceclient);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.referenceClient = new SelectList(db.clients, "Reference_Contrat", "Nom1", miseenserviceclient.referenceClient);
            return View(miseenserviceclient);
        }

        // GET: miseenserviceclients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            miseenserviceclient miseenserviceclient = db.miseenserviceclient.Find(id);
            if (miseenserviceclient == null)
            {
                return HttpNotFound();
            }
            ViewBag.referenceClient = new SelectList(db.clients, "Reference_Contrat", "Nom1", miseenserviceclient.referenceClient);
            return View(miseenserviceclient);
        }

        // POST: miseenserviceclients/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,interface,indexDePose,dateMiseEnService,referenceClient,statut,utilisateur")] miseenserviceclient miseenserviceclient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(miseenserviceclient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.referenceClient = new SelectList(db.clients, "Reference_Contrat", "Nom1", miseenserviceclient.referenceClient);
            return View(miseenserviceclient);
        }

        // GET: miseenserviceclients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            miseenserviceclient miseenserviceclient = db.miseenserviceclient.Find(id);
            if (miseenserviceclient == null)
            {
                return HttpNotFound();
            }
            return View(miseenserviceclient);
        }

        // POST: miseenserviceclients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            miseenserviceclient miseenserviceclient = db.miseenserviceclient.Find(id);
            db.miseenserviceclient.Remove(miseenserviceclient);
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
