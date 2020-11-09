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
    public class bordereauxController : Controller
    {
        private ERADEVEntities3 db = new ERADEVEntities3();

        // GET: bordereaux
        public ActionResult Index()
        {
            return View(db.bordereaux.ToList());
        }

        // GET: bordereaux/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bordereaux bordereaux = db.bordereaux.Find(id);
            if (bordereaux == null)
            {
                return HttpNotFound();
            }
            return View(bordereaux);
        }

        // GET: bordereaux/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: bordereaux/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,numero,datecreation,montant,ouvert,utilisateur,datevalidation,type")] bordereaux bordereaux)
        {
            if (ModelState.IsValid)
            {
                bordereaux.ouvert = 1;
                bordereaux.utilisateur = User.Identity.Name;
                db.bordereaux.Add(bordereaux);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bordereaux);
        }

        // GET: bordereaux/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bordereaux bordereaux = db.bordereaux.Find(id);
            if (bordereaux == null)
            {
                return HttpNotFound();
            }
            return View(bordereaux);
        }

        // POST: bordereaux/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,numero,datecreation,montant,ouvert,utilisateur,datevalidation,type")] bordereaux bordereaux)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bordereaux).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bordereaux);
        }

        // GET: bordereaux/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bordereaux bordereaux = db.bordereaux.Find(id);
            if (bordereaux == null)
            {
                return HttpNotFound();
            }
            return View(bordereaux);
        }

        // POST: bordereaux/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            bordereaux bordereaux = db.bordereaux.Find(id);
            db.bordereaux.Remove(bordereaux);
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
