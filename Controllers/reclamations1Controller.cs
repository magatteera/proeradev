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
    public class reclamations1Controller : Controller
    {
        private ERADEVEntities3 db = new ERADEVEntities3();

        // GET: reclamations1
        public ActionResult Index()
        {
            var reclamation = db.reclamation.Include(r => r.clients).Include(r => r.typereclamation);
            return View(reclamation.ToList());
        }

        // GET: reclamations1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            reclamation reclamation = db.reclamation.Find(id);
            if (reclamation == null)
            {
                return HttpNotFound();
            }
            return View(reclamation);
        }

        // GET: reclamations1/Create
        public ActionResult Create()
        {
            //ViewBag.refclient = new SelectList(db.clients, "Reference_Contrat", "Nom1");
            ViewBag.type = new SelectList(db.typereclamation, "id", "type");
            return View();
        }

        // POST: reclamations1/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,refclient,date,type,priorite,commentaire,nivintervention,telephone,localite,statut,utilisateur")] reclamation reclamation)
        {
            if (ModelState.IsValid)
            {
                reclamation.utilisateur = User.Identity.Name;
                db.reclamation.Add(reclamation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.refclient = new SelectList(db.clients, "Reference_Contrat", "Nom1", reclamation.refclient);
            ViewBag.type = new SelectList(db.typereclamation, "id", "type", reclamation.type);
            return View(reclamation);
        }

        

        public ActionResult verifrefclient([Bind(Include = "refclient")] reclamation reclamation)
        {
            var cls = db.clients.Where(c => c.Reference_Contrat == reclamation.refclient).ToList();
            if(cls.Count() > 0)
            {
                return Json(new { message = "existe" });
            }

            return Json(new { message = "absent" });
        }


        // GET: reclamations1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            reclamation reclamation = db.reclamation.Find(id);
            if (reclamation == null)
            {
                return HttpNotFound();
            }
            //ViewBag.refclient = new SelectList(db.clients, "Reference_Contrat", "Nom1", reclamation.refclient);
            ViewBag.type = new SelectList(db.typereclamation, "id", "type", reclamation.type);
            return View(reclamation);
        }

        // POST: reclamations1/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,refclient,date,type,priorite,commentaire,nivintervention,telephone,localite,statut,utilisateur")] reclamation reclamation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reclamation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.refclient = new SelectList(db.clients, "Reference_Contrat", "Nom1", reclamation.refclient);
            ViewBag.type = new SelectList(db.typereclamation, "id", "type", reclamation.type);
            return View(reclamation);
        }

        // GET: reclamations1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            reclamation reclamation = db.reclamation.Find(id);
            if (reclamation == null)
            {
                return HttpNotFound();
            }
            return View(reclamation);
        }

        // POST: reclamations1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            reclamation reclamation = db.reclamation.Find(id);
            db.reclamation.Remove(reclamation);
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
