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
    public class resiliationsController : Controller
    {
        private PROERAEntities1 db = new PROERAEntities1();

        // GET: resiliations
        public ActionResult Index()
        {
            var resiliation = db.resiliation.Include(r => r.clients);
            return View(resiliation.ToList());
        }

        // GET: resiliations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            resiliation resiliation = db.resiliation.Find(id);
            if (resiliation == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Index");
        }

        // GET: resiliations/Create
        public ActionResult Create()
        {
            //ViewBag.refclient = new SelectList(db.clients, "Reference_Contrat", "Nom1");
            ViewBag.motif = new SelectList(db.motifresiliation, "id", "motif");
            return View();
        }

        // POST: resiliations/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,refclient,date,utilisateur,commentaire,motif,netapayer,indexdepose,interface")] resiliation resiliation)
        {
            if (ModelState.IsValid)
            {
                resiliation.utilisateur = User.Identity.Name;
                db.resiliation.Add(resiliation);

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.refclient = new SelectList(db.clients, "Reference_Contrat", "Nom1", resiliation.refclient);
            ViewBag.motif = new SelectList(db.motifresiliation, "id", "motif", resiliation.motif);
            return View(resiliation);
        }

        // GET: resiliations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            resiliation resiliation = db.resiliation.Find(id);
            if (resiliation == null)
            {
                return HttpNotFound();
            }
            //ViewBag.refclient = new SelectList(db.clients, "Reference_Contrat", "Nom1", resiliation.refclient);
            ViewBag.motif = new SelectList(db.motifresiliation, "id", "motif", resiliation.motif);
            return View(resiliation);
        }

        // POST: resiliations/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,refclient,date,utilisateur,commentaire,motif,netapayer,indexdepose,interface")] resiliation resiliation)
        {
            if (ModelState.IsValid)
            {
                resiliation.utilisateur = User.Identity.Name;
                db.Entry(resiliation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.refclient = new SelectList(db.clients, "Reference_Contrat", "Nom1", resiliation.refclient);
            ViewBag.motif = new SelectList(db.motifresiliation, "id", "motif", resiliation.motif);
            return View(resiliation);
        }


        public ActionResult verifrefclient([Bind(Include = "refclient")] reclamation reclamation)
        {
            var cls = db.clients.Where(c => c.Reference_Contrat == reclamation.refclient).ToList();
            if (cls.Count() > 0)
            {
                return Json(new { message = "existe" });
            }

            return Json(new { message = "absent" });
        }

        // GET: resiliations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            resiliation resiliation = db.resiliation.Find(id);
            if (resiliation == null)
            {
                return HttpNotFound();
            }
            return View(resiliation);
        }

        // POST: resiliations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            resiliation resiliation = db.resiliation.Find(id);
            db.resiliation.Remove(resiliation);
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
