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

    [Authorize(Roles = "Proera_BacfOffice, Proera_ADMIN")]
    public class correctionsoldesController : Controller
    {
        private ERADEVEntities3 db = new ERADEVEntities3();

        // GET: correctionsoldes
        public ActionResult Index()
        {
            return View(db.correctionsolde.ToList());
        }

        // GET: correctionsoldes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            correctionsolde correctionsolde = db.correctionsolde.Find(id);
            if (correctionsolde == null)
            {
                return HttpNotFound();
            }
            return View(correctionsolde);
        }

        // GET: correctionsoldes/Create
        public ActionResult Create()
        {

            return View();
        }


        public ActionResult changementrefclient([Bind(Include = "Reference_Contrat")] clients client)
        {
            var refcl = client.Reference_Contrat;
            var cli = db.clients.Find(refcl);

            if (cli != null)
            {
                    return Json(new
                    {
                        message = "existe",
                        nom = cli.Nom1,
                        prenom = cli.Prenom,
                        solde = cli.SoldeTotal,
                        villagess = db.villages.Find(cli.codevillage).village
                    });
            }

            return Json(new { message = "client inexistant" });
        }


        // POST: correctionsoldes/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,utilisateur,soldepre,soldepost,date,referenceclient,motif")] correctionsolde correctionsolde)
        {
            correctionsolde.utilisateur = User.Identity.Name;
            correctionsolde.date = DateTime.Now;
                db.correctionsolde.Add(correctionsolde);
                db.SaveChanges();
                return RedirectToAction("Index");
            return View(correctionsolde);
        }

        // GET: correctionsoldes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            correctionsolde correctionsolde = db.correctionsolde.Find(id);
            if (correctionsolde == null)
            {
                return HttpNotFound();
            }
            return View(correctionsolde);
        }

        // POST: correctionsoldes/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,utilisateur,soldepre,soldepost,date,referenceclient,motif")] correctionsolde correctionsolde)
        {
            if (ModelState.IsValid)
            {
                db.Entry(correctionsolde).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(correctionsolde);
        }

        // GET: correctionsoldes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            correctionsolde correctionsolde = db.correctionsolde.Find(id);
            if (correctionsolde == null)
            {
                return HttpNotFound();
            }
            return View(correctionsolde);
        }

        // POST: correctionsoldes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            correctionsolde correctionsolde = db.correctionsolde.Find(id);
            db.correctionsolde.Remove(correctionsolde);
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
