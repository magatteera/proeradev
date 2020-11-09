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
    public class SAVsController : Controller
    {
        private ERADEVEntities3 db = new ERADEVEntities3();

        // GET: SAVs
        public ActionResult Index()
        {
            return View(db.SAV.ToList());
        }

        // GET: SAVs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SAV sAV = db.SAV.Find(id);
            if (sAV == null)
            {
                return HttpNotFound();
            }
            return View(sAV);
        }

        // GET: SAVs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SAVs/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,REFERENCE,NOM,PRENOM,VILLAGE,OLDETAT,NEWETAT,DATE,ACTION,MOTIF,REMARQUE,PERIODE,createby,createdate,index,periodedue")] SAV sAV)
        {
            if (ModelState.IsValid)
            {
                db.SAV.Add(sAV);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sAV);
        }

        // GET: SAVs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SAV sAV = db.SAV.Find(id);
            if (sAV == null)
            {
                return HttpNotFound();
            }
            return View(sAV);
        }

        // POST: SAVs/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,REFERENCE,NOM,PRENOM,VILLAGE,OLDETAT,NEWETAT,DATE,ACTION,MOTIF,REMARQUE,PERIODE,createby,createdate,index,periodedue")] SAV sAV)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sAV).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sAV);
        }

        // GET: SAVs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SAV sAV = db.SAV.Find(id);
            if (sAV == null)
            {
                return HttpNotFound();
            }
            return View(sAV);
        }

        // POST: SAVs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SAV sAV = db.SAV.Find(id);
            db.SAV.Remove(sAV);
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
