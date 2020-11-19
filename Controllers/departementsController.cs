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
    public class departementsController : Controller
    {
        private ERADEVEntities3 db = new ERADEVEntities3();

        // GET: departements
        public ActionResult Index()
        {
            var departements = db.departements.Include(d => d.regions);
            return View(departements.ToList());
        }

        // GET: departements/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            departements departements = db.departements.Find(id);
            if (departements == null)
            {
                return HttpNotFound();
            }
            return View(departements);
        }

        // GET: departements/Create
        public ActionResult Create()
        {
            ViewBag.idregion = new SelectList(db.regions, "id", "nom_region");
            return View();
        }

        // POST: departements/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nom,code_departement,geom,idregion")] departements departements)
        {
            if (ModelState.IsValid)
            {
                departements.code_departement = 0;
                db.departements.Add(departements);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idregion = new SelectList(db.regions, "id", "nom_region", departements.idregion);
            return View(departements);
        }

        // GET: departements/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            departements departements = db.departements.Find(id);
            if (departements == null)
            {
                return HttpNotFound();
            }
            ViewBag.idregion = new SelectList(db.regions, "id", "nom_region", departements.idregion);
            return View(departements);
        }

        // POST: departements/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nom,code_departement,geom,idregion")] departements departements)
        {
            if (ModelState.IsValid)
            {
                db.Entry(departements).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idregion = new SelectList(db.regions, "id", "nom_region", departements.idregion);
            return View(departements);
        }

        // GET: departements/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            departements departements = db.departements.Find(id);
            if (departements == null)
            {
                return HttpNotFound();
            }
            return View(departements);
        }

        // POST: departements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            departements departements = db.departements.Find(id);
            db.departements.Remove(departements);
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
