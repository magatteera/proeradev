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
    public class changemodefactsController : Controller
    {
        private PROERAEntities1 db = new PROERAEntities1();

        // GET: changemodefacts
        public ActionResult Index()
        {
            return View(db.changemodefact.ToList());
        }

        // GET: changemodefacts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            changemodefact changemodefact = db.changemodefact.Find(id);
            if (changemodefact == null)
            {
                return HttpNotFound();
            }
            return View(changemodefact);
        }

        // GET: changemodefacts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: changemodefacts/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,refclient,ancienmode,nouveaumode,solde,date")] changemodefact changemodefact)
        {
            if (ModelState.IsValid)
            {
                db.changemodefact.Add(changemodefact);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(changemodefact);
        }

        // GET: changemodefacts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            changemodefact changemodefact = db.changemodefact.Find(id);
            if (changemodefact == null)
            {
                return HttpNotFound();
            }
            return View(changemodefact);
        }


        public ActionResult changement([Bind(Include = "refclient")] changemodefact client)
        {
            var refcl = client.refclient;
            var cli = db.clients.Find(refcl);

            if (cli != null)
            {
                return Json(new
                {
                    message = "existe",
                    solde = Math.Round(cli.SoldeTotal),
                    prenom = cli.Prenom,
                    nom = cli.Nom1,
                    mode = cli.modefacturation
                });
            }

            return Json(new { message = "client inexistant" });
        }


        // POST: changemodefacts/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,refclient,ancienmode,nouveaumode,solde,date")] changemodefact changemodefact)
        {
            if (ModelState.IsValid)
            {
                db.Entry(changemodefact).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(changemodefact);
        }

        // GET: changemodefacts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            changemodefact changemodefact = db.changemodefact.Find(id);
            if (changemodefact == null)
            {
                return HttpNotFound();
            }
            return View(changemodefact);
        }

        // POST: changemodefacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            changemodefact changemodefact = db.changemodefact.Find(id);
            db.changemodefact.Remove(changemodefact);
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
