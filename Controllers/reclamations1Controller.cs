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
        private PROERAEntities db = new PROERAEntities();

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
            //var region = db.regions.ToList();
            //ViewBag.regions = new SelectList(region, "id", "nom_region");
            //var departement = db.departements.ToList();
            //var deptfilt = new List<departements>();
            //foreach (departements dept in departement)
            //{
            //    if (dept.idregion == region[0].id)
            //        deptfilt.Add(dept);
            //}
            //var commune = db.communes.ToList();
            //var communefilt = new List<communes>();
            //foreach (communes com in commune)
            //{
            //    if (com.iddepartement == deptfilt[0].code_departement)
            //        communefilt.Add(com);
            //}
            //var village = db.villages.ToList();
            //var villagefilt = new List<villages>();
            //foreach (villages v in village)
            //{
            //    if (v.idLocalite == communefilt[0].code_com)
            //        villagefilt.Add(v);
            //}


            ViewBag.type = new SelectList(db.typereclamation, "id", "type");
            return View();
        }

        // POST: reclamations1/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,refclient,date,type,priorite,commentaire,nivintervention,telephone,localite,statut,utilisateur, numcompteur, datecloture, nature")] reclamation reclamation)
        {
            if (ModelState.IsValid)
            {
                reclamation.utilisateur = User.Identity.Name;
                db.reclamation.Add(reclamation);
                db.SaveChanges();
                return Redirect("http://nodec-winserv1:8081/ReportServer/Pages/ReportViewer.aspx?%2freclamations&rs:Command=Render");
                return RedirectToAction("Index");
            }
            /*
                        ViewBag.refclient = new SelectList(db.clients, "Reference_Contrat", "Nom1", reclamation.refclient);
                        ViewBag.type = new SelectList(db.typereclamation, "id", "type", reclamation.type);*/
            //return View(reclamation);

            return RedirectToAction("Index");
        }

        public ActionResult changementregion([Bind(Include = "id")] regions reg)
        {

            var departement = db.departements.Where(d => d.idregion == reg.id).ToList();
            string depart = "<option value=''>Selectionnez le department</option>";
            foreach (departements dept in departement)
            {
                depart += "<option value='" + dept.code_departement + "'>" + dept.nom + "</option>";

            }

            string communesselect = "<option value=''>Selectionnez la commune</option>";

            string villagesselect = "<option value=''>Selectionnez le village</option>";



            return Json(new { departement = depart, commune = communesselect, village = villagesselect });

        }
        public ActionResult changementdepartement([Bind(Include = "code_departement")] departements dept)
        {

            var commune = db.communes.Where(c => c.iddepartement == dept.code_departement).ToList();
            string communesselect = "<option value=''>Selectionnez la commune</option>";
            foreach (communes com in commune)
            {

                communesselect += "<option value='" + com.code_com + "'>" + com.nom + "</option>";

            }

            string villagesselect = "<option value=''>Selectionnez le village</option>";


            return Json(new { commune = communesselect, village = villagesselect });
        }


        public ActionResult changementcommune([Bind(Include = "code_com")] communes com)
        {

            var villagess = db.villages.Where(v => v.idLocalite == com.code_com).ToList();
            string villagesselect = "<option value=''>Selectionnez le village</option>";
            foreach (villages v in villagess)
            {

                villagesselect += "<option value='" + v.code_village + "'>" + v.village + "</option>";

            }

            return Json(new { village = villagesselect });
        }



        public ActionResult verifrefclient([Bind(Include = "refclient")] reclamation reclamation)
        {
            var cls = db.clients.Where(c => c.Reference_Contrat == reclamation.refclient).ToList();
            if(cls.Count() > 0)
            {
                var vill = db.villages.Find(cls[0].codevillage);
                return Json(new
                {
                    message = "existe",
                    refclient = cls[0].Reference_Contrat,
                    nom = cls[0].Nom1,
                    prenom = cls[0].Prenom,
                    //village = 
                    village = vill.village,
                    commune = vill.communes.nom,
                    departement = vill.communes.departements.nom,
                    region = vill.communes.departements.regions.nom_region,
                    codevill = vill.code_village,
                    compteur = cls[0].numcompteur
                }) ;
            }

            return Json(new { message = "absent" });
        }
        public ActionResult verifcompteur([Bind(Include = "numcompteur")] reclamation reclamation)
        {
            var cls = db.clients.Where(c => c.numcompteur == reclamation.numcompteur).ToList();
            if (cls.Count() > 0)
            {
                var vill = db.villages.Find(cls[0].codevillage);
                return Json(new
                {
                    message = "existe",
                    refclient = cls[0].Reference_Contrat,
                    nom = cls[0].Nom1,
                    prenom = cls[0].Prenom,
                    //village = 
                    village = vill.village,
                    commune = vill.communes.nom,
                    departement = vill.communes.departements.nom,
                    region = vill.communes.departements.regions.nom_region,
                    codevill = vill.code_village,
                    compteur = cls[0].numcompteur
                });
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

            var idregion = Int32.Parse((reclamation.localite + "").Substring(0, 1));
            var iddept = Int32.Parse((reclamation.localite + "").Substring(0, 2));
            var idcom = Int32.Parse((reclamation.localite + "").Substring(0, 3));
            //ViewBag.refclient = new SelectList(db.clients, "Reference_Contrat", "Nom1");
            var region = db.regions.ToList();
            ViewBag.regions = new SelectList(region, "id", "nom_region", idregion);
            var departement = db.departements.ToList();
            var deptfilt = new List<departements>();
            foreach (departements dept in departement)
            {
                if (dept.idregion == idregion)
                    deptfilt.Add(dept);
            }
            var commune = db.communes.ToList();
            var communefilt = new List<communes>();
            foreach (communes com in commune)
            {
                if (com.iddepartement == iddept)
                    communefilt.Add(com);
            }
            var village = db.villages.ToList();
            var villagefilt = new List<villages>();
            foreach (villages v in village)
            {
                if (v.idLocalite == idcom)
                    villagefilt.Add(v);
            }

            ViewBag.departements = new SelectList(deptfilt, "code_departement", "nom", iddept);
            ViewBag.communes = new SelectList(communefilt, "code_com", "nom", idcom);
            ViewBag.localite = new SelectList(villagefilt, "code_village", "village", reclamation.localite);
            //ViewBag.refclient = new SelectList(db.clients, "Reference_Contrat", "Nom1", reclamation.refclient);
            ViewBag.type = new SelectList(db.typereclamation, "id", "type", reclamation.type);
            return View(reclamation);
        }

        // POST: reclamations1/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,refclient,date,type,priorite,commentaire,nivintervention,telephone,localite,statut,utilisateur, numcompteur, datecloture, nature")] reclamation reclamation)
        {
            if (ModelState.IsValid)
            {
                var rec = db.reclamation.Find(reclamation.id);
                //reclamation.utilisateur = User.Identity.Name;
                rec.utilisateur = User.Identity.Name;
                rec.type = reclamation.type;
                rec.nature = reclamation.nature;
                rec.priorite = reclamation.priorite;
                rec.commentaire = reclamation.commentaire;
                rec.nivintervention = reclamation.nivintervention;
                rec.telephone = reclamation.telephone;
                rec.datecloture = reclamation.datecloture;
                db.Entry(rec).State = EntityState.Modified;
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
