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
    //[Authorize(Roles = "REC")]

    [Authorize(Roles = "Proera_REC, Proera_ENC, Proera_Admin")]
    public class encaissementsController : Controller
    {
        private PROERAEntities1 db = new PROERAEntities1();

        // GET: encaissements
        public ActionResult Index()
        {
            var bordereaux = db.bordereaux.Where(b => (b.ouvert == 1) && (b.type == "encaissement") && (b.utilisateur == User.Identity.Name)).ToList();
            ViewBag.numero = new SelectList(bordereaux, "id", "numero");
            return View();
        }

        // GET: encaissements/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            encaissements encaissements = db.encaissements.Find(id);
            if (encaissements == null)
            {
                return HttpNotFound();
            }
            return View(encaissements);
        }

        // GET: encaissements/Create

        public ActionResult Create(string id)
        {
            var bordereaux = db.bordereaux.Where(b => (b.ouvert == 1) && (b.type== "encaissement") &&(b.utilisateur == User.Identity.Name)).ToList();
            ViewBag.idbordereau = new SelectList(bordereaux, "id", "numero");
            //ViewBag.idfacture = new SelectList(db.factures, "id", "nom");

            ViewBag.ajout = false;
            if (id != null)
            {
                if (Int32.Parse(id) == 1)
                    ViewBag.ajout = true;
            }
            return View();

            //return View("~/Views/encaissements/CreateSansRestrict.cshtml");
        }

        [Authorize(Roles = "Proera_BackOffice, Proera_ADMIN")]
        public ActionResult encaissersansrestrict()
        {
            var bordereaux = db.bordereaux.Where(b => (b.ouvert == 1) && (b.type == "encaissement")).ToList();
            ViewBag.idbordereau = new SelectList(bordereaux, "id", "numero");
            return View("~/Views/encaissements/CreateSansRestrict.cshtml");
        }


        [Authorize(Roles = "Proera_CA, Proera_Admin")]
        public ActionResult arretercaisse(string id)
        {
            var bordereaux = db.bordereaux.Where(b => (b.ouvert == 1) && (b.type == "encaissement")).ToList();
            IEnumerable<SelectListItem> selectListbord = from s in bordereaux
                                                         select new SelectListItem
                                                         {
                                                             Value = s.id + "",
                                                             Text = s.numero
                                                         };
            ViewBag.bordereaux = new SelectList(selectListbord, "Value", "Text");


            return View("arretercaisse");
        }

        public ActionResult changementbordereaux([Bind(Include = "id")] bordereaux bordereaux)
        {
            //var encaiss = db.encaissements.Where(e => e.idbordereau == bordereaux.id).ToList();
            //string tableencaissements = "";
            //double sommeencaisse = 0;
            var bord = db.bordereaux.Find(bordereaux.id);
            var details = db.View_DetailsEncaissement.Where(v => v.id == bord.id).ToList()[0];
            //if (encaiss.Count > 0)
            //{
            //    foreach (encaissements enc in encaiss)
            //    {
            //        //if (cli.etatclient.id == 5 && cli.Contrat == 5)

            //        var cl = db.clients.Find(enc.refclient);

            //        tableencaissements += "<tr>" +
            //            "<td>" + enc.refclient + "</td>" +
            //            "<td>" + cl.Nom1 + "</td>" +
            //            "<td>" + cl.Prenom + "</td>" +
            //            "<td>" + enc.numerorecue + "</td>" +
            //            "<td>" + enc.montantencaisee + "</td>" +
            //            "<td>" + enc.soldeprepaiement + "</td>" +
            //            "<td>" + enc.soldepostpaiement + "</td>" +
            //            "<td>" + enc.dateencaissement + "</td>" +
            //            "<td>" + enc.idfacture + "</td>" +
            //             "</tr>";

            //        //"<td><a href='/clients/mettreEnVigueurValide/"+ cli.Bordereau +"'>Edit</a>" +

            //        sommeencaisse += (double)enc.montantencaisee;
            //    }

            //    return Json(new {numbord = bord.numero, table = tableencaissements, montantencaisse = sommeencaisse, montantbordereau = bord.montant, ecart = sommeencaisse - bord.montant });
            //}

            //else

            //var venc = db.

            return Json(new { numbord = bord.numero, montantencaisse = details.sommeenc, montantbordereau = details.montant,
                ecart = details.sommeenc - details.montant, datecreat = details.datecreation, nbrencs = details.nbrenc, 
                util = details.utilisateur,
                
            });
            //return "";
        }

        public ActionResult changementbordereaux2([Bind(Include = "id")] bordereaux bordereaux)
        {
            var encaiss = db.encaissements.Where(e => e.idbordereau == bordereaux.id).ToList();
            string tableencaissements = "";
            double sommeencaisse = 0;
            var vils = db.villages.ToList();
            if (encaiss.Count > 0)
            {
                foreach (encaissements enc in encaiss)
                {
                    //if (cli.etatclient.id == 5 && cli.Contrat == 5)

                    var cl = db.clients.Find(enc.refclient);

                    tableencaissements += "<tr>" +
                        "<td>" + enc.refclient + "</td>" +
                        "<td>" + cl.Nom1 + "</td>" +
                        "<td>" + cl.Prenom + "</td>" +
                        "<td>" + vils[vils.FindIndex(f => f.code_village == cl.codevillage)].village + "</td>" +
                        "<td>" + enc.montantencaisee + "</td>" +
                        "<td>" + enc.soldeprepaiement + "</td>" +
                        "<td>" + enc.soldepostpaiement + "</td>" +
                        "<td>" + enc.dateencaissement + "</td>" +
                        "<td>" + enc.commentaire + "</td>" +
                        "<td>" + enc.idfacture + "</td>" +
                        "<td><button type='button' id='btncorrigerreleve"+enc.id+"' class='btn btn-outline-success btn-sm' refclient='"+enc.refclient+"'" +
                        "ancienmontant='" + enc.montantencaisee + "'" +
                        "soldeprepaiement='" + enc.soldeprepaiement + "'" +
                        "soldepostpaiement='" + enc.soldepostpaiement + "'" +
                        "idfacture='" + enc.idfacture + "'" +
                        "idenc='" + enc.id + "'" +
                        "numerobordereau='" + enc.idbordereau + "'" +
                        ">Corriger</button></td>" +
                         "</tr>";

                    //"<td><a href='/clients/mettreEnVigueurValide/"+ cli.Bordereau +"'>Edit</a>" +

                    sommeencaisse += (double)enc.montantencaisee;
                }

                return Json(new { table = tableencaissements, montant = sommeencaisse });
            }

            else

                return Json(new { table = tableencaissements, montant = 0 });

        }

        public ActionResult savechangement([Bind(Include = "ancienmontant, nouveaumontant, idfacture, numerobordereau, idenc")] modifencaissement modif)
        {
            try
            {
                modif.utilisateur = User.Identity.Name;
                modif.date = DateTime.Now;
                db.modifencaissement.Add(modif);
                db.SaveChanges();

                var enc = db.encaissements.Find(modif.idenc);

                var cli = db.clients.Find(enc.refclient);

                cli.SoldeTotal = (double)(cli.SoldeTotal - modif.nouveaumontant + modif.ancienmontant);

                db.Entry(cli).State = EntityState.Modified;
                db.SaveChanges();

                    enc.soldepostpaiement = cli.SoldeTotal;
                enc.montantencaisee = modif.nouveaumontant;



                db.Entry(enc).State = EntityState.Modified;
                db.SaveChanges();



                return Json(new { message = "Modification faite avec succes" });
            }
            catch(Exception e)
            {

                //return Json(new { message = "Une erreur s'est produite" });
                return Json(new { message = e.Message });
            }


        }


        



        public ActionResult validerCloture([Bind(Include = "id")] bordereaux bordereaux)
        {

            var bord = db.bordereaux.Find(bordereaux.id);
            bord.ouvert = 0;

            try
            {
                db.Entry(bord).State = EntityState.Modified;
                db.SaveChanges();
                string brd= "<option selected value=''>Selectionnez la Periode</option>";
                var bords= db.bordereaux.Where(b => (b.ouvert == 1) &&(b.type == "encaissement")).ToList();
                foreach (var b in bords)
                {
                    brd += "<option value='"+b.id+"'>"+b.numero+"--"+ b.montant +"</option>";
                }

                return Json(new { message = "Caisse Arretee avec Succes.", ok=1, bordereaux = brd });
            } catch(Exception e)
            {
                return Json(new { message = "Une erreur s'est produite.", ok=0});
            }


        }

        


        public String savebordereau([Bind(Include = "numero,datecreation,montant,ouvert,datevalidation,type")] bordereaux bordereaux)
        {
            bordereaux.ouvert = 1;
            bordereaux.utilisateur = User.Identity.Name;
            bordereaux.datecreation = DateTime.Now;
            db.bordereaux.Add(bordereaux);
            db.SaveChanges();


            var bord1 = db.bordereaux.Where(b => (b.ouvert == 1 & b.type.Contains("encaissement")) && (b.utilisateur == User.Identity.Name)).ToList();
            var bord = bord1.OrderByDescending(b => b.id);

            string bords = "";
            foreach (bordereaux b in bord)
            {
                bords += "<option value='" + b.id + "'>" + b.numero + "</option>";
            }
            //return "<option value='" + 12 + "'>" + 20000 + "</option>";
            return bords;
        }

        // POST: encaissements/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public String Create([Bind(Include = "idfacture,refclient,montantencaisee,datesysteme,soldeprepaiement,soldepostpaiement,dateencaissement,commentaire,idbordereau")] encaissements encaissements)
        {
            if (ModelState.IsValid)
            {
                encaissements.utilisateur = User.Identity.Name;
                db.encaissements.Add(encaissements);
                db.SaveChanges();
                return "Encaissement fait avec succes!";
            }

            ViewBag.idbordereau = new SelectList(db.bordereaux.Where(b => (b.ouvert == 1) && (b.utilisateur.Equals(User.Identity.Name))), "id", "numero", encaissements.idbordereau);
            return "Une erreur s'est produite!";
        }


        public string validencaissement([Bind(Include = "idfacture,refclient,datesysteme,montantencaisee,soldeprepaiement,soldepostpaiement,dateencaissement,commentaire,idbordereau,numerorecue,periode,fraiscoupure")] encaissements encaissement)
        {
            //if (ModelState.IsValid)
            //{
                try
                {
                    var idFac = encaissement.idfacture;
                    encaissement.utilisateur = User.Identity.Name;
                encaissement.datesysteme = DateTime.Now;
                    db.encaissements.Add(encaissement);
                    db.SaveChanges();
    /*
                    var fac = db.factures.Where(f => f.id+"" == idFac).ToList()[0];
                    //var fac = fact[0];
                    fac.Paiement = 1;
                    db.Entry(fac).State = EntityState.Modified;
                    db.SaveChanges();

                    var cli = db.clients.Find(fac.RefClient);
                    cli.SoldeTotal = (double)encaissements.soldepostpaiement;
                    db.Entry(cli).State = EntityState.Modified;
                    db.SaveChanges();*/

                    return "Encaissement fait avec succes!";

                    //return "montant : "+encaissements.montantencaisee+" pre : " + encaissements.soldeprepaiement + " post : " + encaissements.soldepostpaiement + " " + encaissements.periode;
                }
                catch(Exception e)
                {
                    return "Une erreur s'est produite!";
                }

            //}
            //else
            //{

            //    ViewBag.idbordereau = new SelectList(db.bordereaux, "id", "numero", encaissements.idbordereau);
            //    return "Une erreur s'est produite!";
            //}
        }

        public ActionResult changementfacture([Bind(Include = "idfacture")] encaissements encaissements)
        {

            var facture = db.factures.Where(f => f.id+"" == encaissements.idfacture).ToList();
            if (facture.Count > 0)
            {
                var cli = db.clients.Find(facture[0].RefClient);

                return Json(new { montant = facture[0].netPayer, soldefacture = facture[0].solde, penalite = facture[0].penalite, periode = facture[0].PeriodeFacturee, solde = cli.SoldeTotal, refclient = facture[0].RefClient });
            }
            else
                return Json(new { message = "inexistant" });
        }

        public ActionResult changementrefclient([Bind(Include = "refclient")] encaissements encaissements) 
        {
            var refcl = encaissements.refclient;
            var cli = db.clients.Find(refcl);

            if(cli != null)
            {

                var paiements = db.encaissements.Where(e => e.refclient == cli.Reference_Contrat).OrderByDescending(e => e.dateencaissement).Take(10).ToList();
                string paiess = "";
                foreach(var p in paiements)
                {
                    paiess += "<tr>" +
                        "<td>" + p.dateencaissement + "</td>" +
                        "<td>" + p.numerorecue + "</td>" +
                        "<td>" + p.montantencaisee + "</td>" +
                        "<td>" + p.soldeprepaiement + "</td>" +
                        "<td>" + p.soldepostpaiement + "</td>" +
                        "<td>" + p.idfacture + "</td>" +
                        "<td>" + p.periode + "</td>" +
                         "</tr>";
                }

                var facture = db.factures.Where(f => (f.RefClient == refcl) && (f.Paiement == 0)).ToList();
                var facs = facture.OrderByDescending(f => f.dateReleve);
                if (facture.Count > 0)
                {
                    string listefactures = "<option value=''>Selectionnez la facture</option>";
                    foreach (var fac in facture)
                    {
                        listefactures = listefactures + "<option value='" + fac.id + "'>" + fac.IdFacture + "</option>";
                    }



                    return Json(new
                    {
                        listefacture = listefactures,
                        message = "",
                        nom = cli.Nom1,
                        prenom = cli.Prenom,
                        villagess = db.villages.Find(cli.codevillage).village,
                        paiements = paiess
                    });
                }
                else
                {

                    return Json(new { message = "aucune facture", paiements = paiess,
                        nom = cli.Nom1,
                        prenom = cli.Prenom,
                        villagess = db.villages.Find(cli.codevillage).village
                    });
                }
            }

            return Json(new { message = "client inexistant" });
        }

        public ActionResult changementbordereau([Bind(Include = "idbordereau")] encaissements encaissements)
        {

            var encaisse = db.encaissements.Where(e => e.idbordereau == encaissements.idbordereau).OrderByDescending(e => e.id).ToList();

            double sommeenc = 0;
            string liste = "";
            foreach(var enc in encaisse)
            {
                sommeenc += (double)enc.montantencaisee;
                //liste = liste + "<li> Id Facture : "+ enc.idfacture + "  montant : " + enc.montantencaisee +"</li>";
                liste = liste + "<tr><td>" + enc.idfacture + "</td><td>" + enc.refclient + "</td><td>" + enc.montantencaisee + "</td></tr>";
            }
            if(encaisse.Count() > 0)
                return Json(new { message = "", listes = liste, sommeencs = sommeenc  });
            return Json(new { message = "aucune facture" });
        }

   



        // GET: encaissements/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            encaissements encaissements = db.encaissements.Find(id);
            if (encaissements == null)
            {
                return HttpNotFound();
            }
            ViewBag.idbordereau = new SelectList(db.bordereaux, "id", "numero", encaissements.idbordereau);
            ViewBag.idfacture = new SelectList(db.factures, "id", "nom", encaissements.idfacture);
            return View(encaissements);
        }

        // POST: encaissements/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,idfacture,refclient,datesysteme,montantencaisee,soldeprepaiement,soldepostpaiement,dateencaissement,utilisateur,commentaire,idbordereau")] encaissements encaissements)
        {
            if (ModelState.IsValid)
            {
                db.Entry(encaissements).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idbordereau = new SelectList(db.bordereaux, "id", "numero", encaissements.idbordereau);
            ViewBag.idfacture = new SelectList(db.factures, "id", "nom", encaissements.idfacture);
            return View(encaissements);
        }

        public ActionResult encaisser()
        {
            return View("~/Views/recouvrements/encaisser.cshtml");
        }





        // GET: encaissements/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            encaissements encaissements = db.encaissements.Find(id);
            if (encaissements == null)
            {
                return HttpNotFound();
            }
            return View(encaissements);
        }

        // POST: encaissements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            encaissements encaissements = db.encaissements.Find(id);
            db.encaissements.Remove(encaissements);
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
