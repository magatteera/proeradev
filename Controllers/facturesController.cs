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
    public class facturesController : Controller
    {
        private ERADEVEntities3 db = new ERADEVEntities3();

        // GET: factures
        public ActionResult Index()
        {

            var periode = db.recouvrements.Where(r => r.active == 1).ToList();
            ViewBag.periode = new SelectList(periode, "periode", "periode");
            return View();
        }

        // GET: factures/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            factures factures = db.factures.Find(id);
            if (factures == null)
            {
                return HttpNotFound();
            }
            return View(factures);
        }

        // GET: factures/Create
        public ActionResult Create()
        {
            ViewBag.PeriodeFacturee = new SelectList(db.recouvrements, "periode", "utilisateur");
            return View();
        }

        // POST: factures/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,idreleve,nom,prenom,village,dateReleve,conso,RedTableau,PartEnergetique,Red_electRural,assTVA,TVA,netPayer,TCO,facturation,AncienIndex,NouvIndex,PeriodeFacturee,ReportArr,IdFacture,solde,penalite,idref,RembInst,RefClient,Paiement,NivServ,NivPuissance,NbreJours,reference")] factures factures)
        {
            if (ModelState.IsValid)
            {
                db.factures.Add(factures);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PeriodeFacturee = new SelectList(db.recouvrements, "periode", "utilisateur", factures.PeriodeFacturee);
            return View(factures);
        }

        // GET: factures/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            factures factures = db.factures.Find(id);
            if (factures == null)
            {
                return HttpNotFound();
            }
            ViewBag.PeriodeFacturee = new SelectList(db.recouvrements, "periode", "utilisateur", factures.PeriodeFacturee);
            return View(factures);
        }

        public ActionResult corriger()
        {
           return View("corriger");
        }

        // POST: factures/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,idreleve,nom,prenom,village,dateReleve,conso,RedTableau,PartEnergetique,Red_electRural,assTVA,TVA,netPayer,TCO,facturation,AncienIndex,NouvIndex,PeriodeFacturee,ReportArr,IdFacture,solde,penalite,idref,RembInst,RefClient,Paiement,NivServ,NivPuissance,NbreJours,reference")] factures factures)
        {
            if (ModelState.IsValid)
            {
                db.Entry(factures).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PeriodeFacturee = new SelectList(db.recouvrements, "periode", "utilisateur", factures.PeriodeFacturee);
            return View(factures);
        }

        public ActionResult factureACorriger([Bind(Include = "id")] factures factures)
        {
            var facs = db.factures.Where(f => (f.id == factures.id) && (f.Paiement == 0)).ToList();
            if (facs.Count() > 0)
            {
                var fac = facs[0];
                    return Json(new
                    {
                        message = "OK",
                        idFacture = fac.IdFacture,
                        montant = fac.netPayer,
                        ancienindex = fac.AncienIndex,
                        nouvelindex = fac.NouvIndex,
                        consom = fac.conso
                    });
            }
            else
                return Json(new
                {
                    message = "Facture inexistante ou deja payee"
                });
        }


        


        public ActionResult validerModif([Bind(Include = "id, conso, NouvIndex, AncienIndex")] factures factures)
        {

            var facs = db.factures.Where(f => f.id == factures.id).ToList();
            var fac = facs[0];
            modiffacture mdf = new modiffacture();
            mdf.ancienneconso = (int)fac.conso;
            mdf.nouvelleconso = (int)factures.conso;
            mdf.utilisateur = User.Identity.Name;
            mdf.idfacture = factures.id;
            mdf.date = DateTime.Now;

            db.modiffacture.Add(mdf);
            db.SaveChanges();

            fac.conso = factures.conso;
            fac.NouvIndex = factures.NouvIndex;
            fac.AncienIndex = factures.AncienIndex;


            db.Entry(fac).State = EntityState.Modified;
            db.SaveChanges();

            return Json(new
                {
                    message = "Facture inexistante"
                });
        }



        // GET: factures/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            factures factures = db.factures.Find(id);
            if (factures == null)
            {
                return HttpNotFound();
            }
            return View(factures);
        }

        // POST: factures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            factures factures = db.factures.Find(id);
            db.factures.Remove(factures);
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
