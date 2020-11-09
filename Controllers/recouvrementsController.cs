using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CsvHelper;
using proera;
using proera.Models;

namespace proera.Controllers
{
    public class recouvrementsController : Controller
    {
        private ERADEVEntities3 db = new ERADEVEntities3();

        // GET: recouvrements
        public ActionResult Index(string id)
        {

            ViewBag.ajout = false;
            if (id != null)
            {
                if(Int32.Parse(id) == 1)
                ViewBag.ajout = true;
            }
            return View(db.recouvrements.ToList());
        }

        // GET: recouvrements/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            recouvrements recouvrements = db.recouvrements.Find(id);
            if (recouvrements == null)
            {
                return HttpNotFound();
            }
            return View(recouvrements);
        }

        // GET: recouvrements/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: recouvrements/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,opendate,closedate,periode,utilisateur,montantRecouvre,nbrFactures,nbrFacturesPayees")] recouvrements recouvrements)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.recouvrements.Add(recouvrements);
                    db.SaveChanges();

                    return RedirectToAction("Index/" + 1);
                }
                catch(Exception e)
                {

                }
            }

            return View(recouvrements);
        }

        public ActionResult ouvrirperioderecouvrement()
        {
            return View("~/Views/recouvrements/create.cshtml");
        }


        public string changementperiode([Bind(Include = "periode")] recouvrements recouvrements)
        {
            string rep = "";
            var periodess = db.recouvrements.Where(r => r.periode == recouvrements.periode).ToList();
            if (periodess.Count() > 0)
                rep = "existe";

            return rep;

        }

        public ActionResult ouvrirperiode(HttpPostedFileBase file, [Bind(Include = "periode, opendate")] recouvrements recouvrements)
        {
            

            string path = null;
            List<releves> relevestoadd = new List<releves>();
            try
            {
                if(file.ContentLength > 0)
                {
                    var filename = Path.GetFileName(file.FileName);
                    
                    //file.SaveAs(path);

                    path = Path.Combine(Server.MapPath("~/App_Data"), filename);
                    //path = "C:\\Users\\m.war\\source\\repos\\proera\\App_Data\\upload\\" + filename;
                    file.SaveAs(path);

                    var csv = new CsvReader(new StreamReader(path), CultureInfo.CurrentCulture);

                    //ViewBag.test2 = csv.GetRecord<>.ToString();
                    var releveslist = csv.GetRecords<recouv>().ToList();

                    var listeclients = db.clients.ToList();

                    ViewBag.test = recouvrements.periode;
                    recouvrements.utilisateur = User.Identity.Name;
                    recouvrements.active = 1;
                    //var rel = db.releves.Where(r=>r.periode == recouvrements.periode)
                    recouvrements.facturee = 0;
                    db.recouvrements.Add(recouvrements);
                    db.SaveChanges();

                    int nbrreleves = 0;
                    int nbrdepart = 0;
                    int dupliques = 0;
                    int inexistant = 0;
                    int nonenservice = 0;

                    var listeajoutes = new List<recouv>();

                    foreach(var r in releveslist)
                    {
                        nbrdepart++;

                        if (listeclients.FindIndex(re => re.Reference_Contrat == Int32.Parse(r.ReferenceContrat)) == -1)
                        {
                            inexistant++;
                            tempReleves temp = new tempReleves();
                            temp.ReferenceContrat = Int32.Parse(r.ReferenceContrat);
                            temp.numcompteur = r.numcompteur;
                            temp.Ancienindex = Int32.Parse(r.Ancienindex);
                            temp.datereleve = DateTime.Parse(r.datereleve);
                            temp.consommation = Int32.Parse(r.consommation);
                            temp.periode = recouvrements.periode;
                            temp.Nouvelindex = Int32.Parse(r.Nouvelindex);
                            temp.nombrejours = Int32.Parse(r.nombrejours);
                            temp.type = "client inexistant";

                            db.tempReleves.Add(temp);
                            db.SaveChanges();
                        } else
                        {

                            if (listeclients[listeclients.FindIndex(re => re.Reference_Contrat == Int32.Parse(r.ReferenceContrat))].Etat_Client != 1)
                            {
                                tempReleves temp = new tempReleves();

                                temp.ReferenceContrat = Int32.Parse(r.ReferenceContrat);
                                temp.numcompteur = r.numcompteur;
                                temp.Ancienindex = Int32.Parse(r.Ancienindex);
                                temp.datereleve = DateTime.Parse(r.datereleve);
                                temp.consommation = Int32.Parse(r.consommation);
                                temp.periode = recouvrements.periode;
                                temp.Nouvelindex = Int32.Parse(r.Nouvelindex);
                                temp.nombrejours = Int32.Parse(r.nombrejours);
                                temp.type = "Client non en service";

                                db.tempReleves.Add(temp);
                                db.SaveChanges();

                                nonenservice++;
                            }
                            else
                            {


                                if (listeajoutes.FindIndex(re => re.ReferenceContrat == r.ReferenceContrat) == -1)
                                {
                                    releves rel = new releves();
                                    rel.Reference_Contrat = Int32.Parse(r.ReferenceContrat);
                                    rel.numcompteur = r.numcompteur;
                                    rel.Ancien_index = Int32.Parse(r.Ancienindex);
                                    rel.date_de_relève = r.datereleve;
                                    rel.consommation = Int32.Parse(r.consommation);
                                    rel.periode = recouvrements.periode;
                                    rel.Nouvel_index = Int32.Parse(r.Nouvelindex);
                                    rel.nbreJour = Int32.Parse(r.nombrejours);
                                    rel.categorie = r.categorie;
                                    rel.facturee = 0;
                                    try
                                    {
                                        db.releves.Add(rel);
                                        db.SaveChanges();
                                        nbrreleves++;
                                    }
                                    catch (Exception e)
                                    {
                                        tempReleves temp = new tempReleves();

                                        temp.ReferenceContrat = Int32.Parse(r.ReferenceContrat);
                                        temp.numcompteur = r.numcompteur;
                                        temp.Ancienindex = Int32.Parse(r.Ancienindex);
                                        temp.datereleve = DateTime.Parse(r.datereleve);
                                        temp.consommation = Int32.Parse(r.consommation);
                                        temp.periode = recouvrements.periode;
                                        temp.Nouvelindex = Int32.Parse(r.Nouvelindex);
                                        temp.nombrejours = Int32.Parse(r.nombrejours);
                                        temp.type = "erreur inconnue";

                                        db.tempReleves.Add(temp);
                                        db.SaveChanges();
                                    }
                                }
                                else
                                {
                                    dupliques++;
                                    tempReleves temp = new tempReleves();

                                    temp.ReferenceContrat = Int32.Parse(r.ReferenceContrat);
                                    temp.numcompteur = r.numcompteur;
                                    temp.Ancienindex = Int32.Parse(r.Ancienindex);
                                    temp.datereleve = DateTime.Parse(r.datereleve);
                                    temp.consommation = Int32.Parse(r.consommation);
                                    temp.periode = recouvrements.periode;
                                    temp.Nouvelindex = Int32.Parse(r.Nouvelindex);
                                    temp.nombrejours = Int32.Parse(r.nombrejours);
                                    temp.type = "duplique";

                                    db.tempReleves.Add(temp);
                                    db.SaveChanges();
                                }
                                listeajoutes.Add(r);
                            }
                        }

                           
                        //}
                    }
                    ViewBag.erreur = 0;
                    ViewBag.nbrdepart = nbrdepart;
                    ViewBag.dupliques = dupliques;
                    ViewBag.nbrreleves = nbrreleves;
                    ViewBag.inexistant = inexistant; 
                    ViewBag.nonenservice = nonenservice;

                    

                    return View("~/Views/recouvrements/test.cshtml");
                }
            } catch(Exception e)
            {
                ViewBag.erreur = 1;

                ViewBag.nbrdepart = 0;
                ViewBag.nbrreleves = 0;
                return View("~/Views/recouvrements/test.cshtml");
            }


            //ViewBag.opendate = recouvrements.opendate;
            //ViewBag.filename = file.FileName;
            ViewBag.erreur = 2;

            ViewBag.nbrdepart = 0;
            ViewBag.nbrreleves = 0;
            return View("~/Views/recouvrements/test.cshtml");

        }

        // GET: recouvrements/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            recouvrements recouvrements = db.recouvrements.Find(id);
            if (recouvrements == null) 
            {
                return HttpNotFound();
            }
            return View(recouvrements);
        }


        public ActionResult cloturer()
        {
            var recouvs = db.recouvrements.Where(r => r.active == 1).ToList();
            ViewBag.periode = new SelectList(recouvs, "periode", "periode");
            return View("~/Views/recouvrements/cloture.cshtml");
        }
        
        public ActionResult validerCloture([Bind(Include = "periode")] recouvrements recouvrements)
        {
            var recouv = db.recouvrements.Where(r => r.periode == recouvrements.periode).ToList();
            var rec = recouv[0];
            rec.active = 0;
            db.Entry(rec).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("cloturer");
        }

        public ActionResult changementrecouvrements([Bind(Include = "periode")] recouvrements recouvrements)
        {
            var factures = db.factures.Where(f => f.PeriodeFacturee == recouvrements.periode).ToList();
            //var encaissements = db.encaissements.Where(f => f.idfacture.Contains(recouvrements.periode)).ToList();

            var encaissementss = db.encaissements.ToList();
            var encaissements = new List<encaissements>();
            foreach(var enc in encaissementss)
            {
                if (factures.Find(f => f.id == enc.idfacture) != null)
                    encaissements.Add(enc);
            }


            var facturespayees = 0;
            var facturesimpayees = 0;
            double montant = 0;

            foreach(var fac in factures)
            {
                if(fac.Paiement == 1)
                {
                    facturespayees++;
                    var index = encaissements.FindIndex(re => re.idfacture == fac.id);
                    if (index != -1)
                        //var enc = db.encaissements.Where(e => e.idfacture == fac.IdFacture).ToList();
                        //if(enc.Count > 0)
                        montant = montant + (double)encaissements[index].montantencaisee;
                } else
                {
                    facturesimpayees++;
                }
            }

            return Json(new { facturespayee = facturespayees, facturesimpayee = facturesimpayees, montants = montant , factures = facturespayees  + facturesimpayees
            , progresspayees = facturespayees + facturesimpayees == 0 ? 0 : (int)(200 * facturespayees)/ (facturespayees + facturesimpayees)
            ,
                progressimpayees = facturespayees + facturesimpayees == 0 ? 0 : (int)(200 * facturesimpayees) / (facturespayees + facturesimpayees)
            });
        }

        // POST: recouvrements/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,opendate,closedate,periode,utilisateur,montantRecouvre,nbrFactures,nbrFacturesPayees")] recouvrements recouvrements)
        {
            if (ModelState.IsValid)
            {
                db.Entry(recouvrements).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(recouvrements);
        }

        // GET: recouvrements/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            recouvrements recouvrements = db.recouvrements.Find(id);
            if (recouvrements == null)
            {
                return HttpNotFound();
            }
            return View(recouvrements);
        }

        // POST: recouvrements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            recouvrements recouvrements = db.recouvrements.Find(id);
            db.recouvrements.Remove(recouvrements);
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
