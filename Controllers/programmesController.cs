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

    [Authorize(Roles = "Proera_SIG,Proera_Admin")]
    public class programmesController : Controller
    {
        private Data_PROERA db = new Data_PROERA();

        // GET: programmes
        public ActionResult Index()
        {
            return View(db.programmes.ToList());
        }

        // GET: programmes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            programmes programmes = db.programmes.Find(id);
            if (programmes == null)
            {
                return HttpNotFound();
            }
            return View(programmes);
        }

        // GET: programmes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: programmes/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "nom,ID,tech,DateDebut,delai,budget,MTavance,niveau_decaissement,avenant,DateFin,descriptProg,remarque,operateur")] programmes programmes)
        {
            if (ModelState.IsValid)
            {
                db.programmes.Add(programmes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(programmes);
        }

        // GET: programmes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            programmes programmes = db.programmes.Find(id);
            if (programmes == null)
            {
                return HttpNotFound();
            }
            return View(programmes);
        }

        // POST: programmes/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "nom,ID,tech,DateDebut,delai,budget,MTavance,niveau_decaissement,avenant,DateFin,descriptProg,remarque,operateur")] programmes programmes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(programmes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(programmes);
        }

        // GET: programmes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            programmes programmes = db.programmes.Find(id);
            if (programmes == null)
            {
                return HttpNotFound();
            }
            return View(programmes);
        }

        // POST: programmes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            programmes programmes = db.programmes.Find(id);
            db.programmes.Remove(programmes);
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
