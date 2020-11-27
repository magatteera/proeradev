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
    public class villagesController : Controller
    {
        private ERADEVEntities3 db = new ERADEVEntities3();

        // GET: villages
        public ActionResult Index()
        {
            var villages = db.villages.Include(v => v.communes).Include(v => v.programmes);
            return View(villages.ToList());
        }

        // GET: villages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            villages villages = db.villages.Find(id);
            if (villages == null)
            {
                return HttpNotFound();
            }
            return View(villages);
        }

        // GET: villages/Create
        public ActionResult Create()
        {
            ViewBag.idLocalite = new SelectList(db.communes, "code_com", "nom");
            ViewBag.code_prog = new SelectList(db.programmes, "ID", "nom");
            return View();
        }

        // POST: villages/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,region,departement,commune,village,population,menage,education,sante,forage,antenne_bts,moulin,longitude,latitude,code_village,code_prog,idLocalite")] villages villages)
        {
            if (ModelState.IsValid)
            {
                db.villages.Add(villages);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idLocalite = new SelectList(db.communes, "code_com", "nom", villages.idLocalite);
            ViewBag.code_prog = new SelectList(db.programmes, "ID", "nom", villages.code_prog);
            return View(villages);
        }

        // GET: villages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            villages villages = db.villages.Find(id);
            if (villages == null)
            {
                return HttpNotFound();
            }
            ViewBag.idLocalite = new SelectList(db.communes, "code_com", "nom", villages.idLocalite);
            ViewBag.code_prog = new SelectList(db.programmes, "ID", "nom", villages.code_prog);
            return View(villages);
        }

        // POST: villages/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,region,departement,commune,village,population,menage,education,sante,forage,antenne_bts,moulin,longitude,latitude,code_village,code_prog,idLocalite")] villages villages)
        {
            if (ModelState.IsValid)
            {
                db.Entry(villages).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idLocalite = new SelectList(db.communes, "code_com", "nom", villages.idLocalite);
            ViewBag.code_prog = new SelectList(db.programmes, "ID", "nom", villages.code_prog);
            return View(villages);
        }

        // GET: villages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            villages villages = db.villages.Find(id);
            if (villages == null)
            {
                return HttpNotFound();
            }
            return View(villages);
        }

        // POST: villages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            villages villages = db.villages.Find(id);
            db.villages.Remove(villages);
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
