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
        private PROERAEntities db = new PROERAEntities();

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
            ViewBag.code_prog = new SelectList(db.programmes, "ID", "nom");

            var region = db.regions.ToList();
            var idregion = region[0].id;
            ViewBag.region = new SelectList(region, "id", "nom_region");
            var depts = db.departements.Where(d => d.idregion == idregion).ToList();
            ViewBag.departement = new SelectList(depts, "code_departement", "nom");
            var iddepts = depts[0].code_departement;
            var coms = db.communes.Where(c => c.iddepartement == iddepts).ToList();

            ViewBag.technologie = new SelectList(db.Technologies, "id", "technologie");

            ViewBag.idLocalite = new SelectList(coms, "code_com", "nom");
            return View();
        }

        // POST: villages/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "region,departement,commune,village,population,menage,education,sante,forage,antenne_bts,moulin,longitude,latitude,code_prog,idLocalite,etattravaux,technologie,commercialise,nbrclients,codemt,neardist,source")] villages villages)
        {
            if (ModelState.IsValid)
            {
                villages.menage = villages.population / 8;
                villages.code_village = 1;
                villages.commune = db.communes.Find(villages.idLocalite).nom;
                db.villages.Add(villages);


                db.SaveChanges();

                villages villages2 = db.villages.Find(1);
                db.villages.Remove(villages2);

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idLocalite = new SelectList(db.communes, "code_com", "nom", villages.idLocalite);
            ViewBag.code_prog = new SelectList(db.programmes, "ID", "nom", villages.code_prog);
            return View(villages);
        }

        public ActionResult changementvillage([Bind(Include = "code_village")] villages village)
        {
            village = db.villages.Find(village.code_village);
            return Json(new
            {
                nomvillage = village.village,
                pop = village.population,
                educ = village.education,
                sant = village.sante,
                forages = village.forage,
                antbts = village.antenne_bts,
                moul = village.moulin,
                longi = village.longitude,
                lati = village.latitude,
                etattra = village.etattravaux,
                code = village.code_prog,
                techprog = village.programmes.tech,
                techno = village.technologie,
                nbrclient = village.nbrclients,
                source = village.source
            });

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

            var iddepts = db.communes.Where(c => c.code_com == villages.idLocalite).ToList()[0].iddepartement;
            var idregion = db.departements.Where(c => c.code_departement == iddepts).ToList()[0].idregion;

            var region = db.regions.ToList();
            //var idregion = (villages.idLocalite + "").Substring(1, 1);
            ViewBag.region = new SelectList(region, "id", "nom_region", idregion);
            var depts = db.departements.Where(d => d.idregion == idregion).ToList();
            //var iddepts = (villages.idLocalite + "").Substring(1, 2);
            ViewBag.departement = new SelectList(depts, "code_departement", "nom",iddepts);

            var coms = db.communes.Where(c => c.iddepartement == iddepts).ToList();
            var idcom = villages.idLocalite;
            ViewBag.idLocalite = new SelectList(db.communes, "code_com", "nom", villages.idLocalite);

            var progs = db.programmes.ToList();
            if (villages.code_prog != null)
                ViewBag.code_prog = progs.Select(x => new SelectListItem() { Value = x.ID+"", Text = x.nom, Selected = villages.code_prog+"" == x.ID+"" }).ToList();

            else
                ViewBag.code_prog = new SelectList(db.programmes, "ID", "nom");

            if (villages.technologie != null)
                ViewBag.technologie = new SelectList(db.Technologies, "id", "technologie", villages.Technologies.id);
            else
                ViewBag.technologie = new SelectList(db.Technologies, "id", "technologie");
            return View(villages);
        }



        // GET: villages/Edit/5
        public ActionResult Edit2()
        {
            var region = db.regions.ToList();
            var idregion = region[0].id;
            ViewBag.region = new SelectList(region, "id", "nom_region");
            var depts = db.departements.Where(d => d.idregion  == idregion).ToList();
            ViewBag.departement = new SelectList(depts, "code_departement", "nom");
            var iddept = depts[0].code_departement;
            var coms = db.communes.Where(c => c.iddepartement  == iddept).ToList();
            ViewBag.idLocalite = new SelectList(db.communes, "code_com", "nom");
            ViewBag.code_prog = new SelectList(db.programmes, "ID", "nom");

            ViewBag.technologie = new SelectList(db.Technologies, "ID", "technologie");

            return View("~/Views/villages/Edit2.cshtml");
        }


        // POST: villages/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,region,departement,commune,village,population,menage,education,sante,forage,antenne_bts,moulin,longitude,latitude,code_village,code_prog,idLocalite,etattravaux,technologie,commercialise,nbrclients,codemt,neardist,source")] villages villages)
        {
            if (ModelState.IsValid)
            {
                villages.menage = villages.population / 8;
                villages.commune = db.communes.Find(villages.idLocalite).nom;
                villages.departement = db.departements.Find(db.communes.Find(villages.idLocalite).iddepartement).nom;
                villages.region = db.regions.Find(db.departements.Find(db.communes.Find(villages.idLocalite).iddepartement).idregion).nom_region;
                db.Entry(villages).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idLocalite = new SelectList(db.communes, "code_com", "nom", villages.idLocalite);
            ViewBag.code_prog = new SelectList(db.programmes, "ID", "nom", villages.code_prog);
            return View(villages);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit2([Bind(Include = "id,region,departement,commune,village,population,menage,education,sante,forage,antenne_bts,moulin,longitude,latitude,code_village,code_prog,idLocalite,etattravaux,technologie,commercialise,nbrclients,codemt,neardist,source")] villages villages)
        {
            //if (ModelState.IsValid)
            //{
                var v = db.villages.Find(villages.code_village);
                v.village = villages.village;
                v.population = villages.population;
                v.education = villages.education;
                v.sante = villages.sante;
                v.forage = villages.forage;
                v.antenne_bts = villages.antenne_bts;
                v.moulin = villages.moulin;
                v.longitude = villages.longitude;
                v.latitude = villages.latitude;
                v.etattravaux = villages.etattravaux;
                v.code_prog = villages.code_prog;
                v.technologie = villages.technologie; 
                v.source = villages.source;
            villages.commune = db.communes.Find(villages.idLocalite).nom;
            //villages.menage = villages.population / 8;
            db.Entry(v).State = EntityState.Modified;
                db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            var region = db.regions.ToList();
            var idregion = region[0].id;
            ViewBag.region = new SelectList(region, "id", "nom_region");
            var depts = db.departements.Where(d => d.idregion == idregion).ToList();
            ViewBag.departement = new SelectList(depts, "code_departement", "nom");
            var iddept = depts[0].code_departement;
            var coms = db.communes.Where(c => c.iddepartement == iddept).ToList();
            ViewBag.idLocalite = new SelectList(db.communes, "code_com", "nom");
            ViewBag.code_prog = new SelectList(db.programmes, "ID", "nom");

            ViewBag.technologie = new SelectList(db.Technologies, "ID", "technologie");

            return View("~/Views/villages/Edit2.cshtml");
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
