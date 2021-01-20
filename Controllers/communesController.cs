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

    [Authorize(Roles = "Proera_ADMIN, Proera_SIG")]
    public class communesController : Controller
    {
        private Data_PROERA db = new Data_PROERA();

        // GET: communes
        public ActionResult Index()
        {
            var communes = db.communes.Include(c => c.departements);
            return View(communes.ToList());
        }

        // GET: communes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            communes communes = db.communes.Find(id);
            if (communes == null)
            {
                return HttpNotFound();
            }
            return View(communes);
        }

        // GET: communes/Create
        public ActionResult Create()
        {

            var region = db.regions.ToList();
            var idregion = region[0].id;
            ViewBag.regions = new SelectList(region, "id", "nom_region");
            var depts = db.departements.Where(d => d.idregion == idregion).ToList();
            ViewBag.iddepartement = new SelectList(depts, "code_departement", "nom");
            //ViewBag.iddepartement = new SelectList(db.departements, "code_departement", "nom");
            return View();
        }

        // POST: communes/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "nom,iddepartement")] communes communes)
        {
            if (ModelState.IsValid)
            {
                communes.code_com = 0;
                communes.geom = null;
                communes.id = 0;
                var codedept = db.communes.Where(c => c.iddepartement == communes.iddepartement).ToList();
                var code = communes.iddepartement + "" + (codedept.Count()+1);
                communes.code_com = Int32.Parse(code);
                db.communes.Add(communes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.iddepartement = new SelectList(db.departements, "code_departement", "nom", communes.iddepartement);
            return View(communes);
        }

        // GET: communes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            communes communes = db.communes.Find(id);
            if (communes == null)
            {
                return HttpNotFound();
            }
            ViewBag.iddepartement = new SelectList(db.departements, "code_departement", "nom", communes.iddepartement);
            return View(communes);
        }

        // POST: communes/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nom,code_com,geom,iddepartement")] communes communes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(communes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.iddepartement = new SelectList(db.departements, "code_departement", "nom", communes.iddepartement);
            return View(communes);
        }

        // GET: communes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            communes communes = db.communes.Find(id);
            if (communes == null)
            {
                return HttpNotFound();
            }
            return View(communes);
        }

        // POST: communes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            communes communes = db.communes.Find(id);
            db.communes.Remove(communes);
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
