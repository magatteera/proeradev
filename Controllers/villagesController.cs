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

    class GFG : IComparer<string>
    {
        public int Compare(string x, string y)
        {

            if (x == null || y == null)
            {
                return 0;
            }

            // "CompareTo()" method 
            return x.CompareTo(y);

        }
    }



    //[Authorize(Roles = "REC")]
    public class villagesController : Controller
    {
        private ERADEVEntities3 db = new ERADEVEntities3();

        // GET: villages
        public ActionResult Index()
        {
            var villages = db.villages.ToList();
            return View(villages);
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

        //[Authorize(Roles = "Administrateurs")]
        // GET: villages/Create
        public ActionResult Create()
        {

            var region = db.regions.ToList();
            ViewBag.regions = new SelectList(region, "id", "nom_region");
            var departement = db.departements.ToList();
            var deptfilt = new List<departements>();
            foreach (departements dept in departement)
            {
                if (dept.idregion == region[0].id)
                    deptfilt.Add(dept);
            }
            var commune = db.communes.ToList();
            var communefilt = new List<communes>();
            foreach (communes com in commune)
            {
                if (com.iddepartement == deptfilt[0].code_departement)
                    communefilt.Add(com);
            }
            var village = db.villages.ToList();
            var villagefilt = new List<villages>();
            foreach (villages v in village)
            {
                if (v.idLocalite == communefilt[0].code_com)
                    villagefilt.Add(v);
            }

            ViewBag.departements = new SelectList(deptfilt, "code_departement", "nom");
            ViewBag.COMMUNE = new SelectList(communefilt, "code_com", "nom");
            return View();
        }

        // POST: villages/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,region,departement,commune,village,population,menage,education,sante,forage,antenne_bts,moulin,long,lat,code_village,code_prog,idLocalite")] villages villages)
        {
            if (ModelState.IsValid)
            {
                db.villages.Add(villages);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(villages);
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
            return View(villages);
        }

        // POST: villages/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,region,departement,commune,village,population,menage,education,sante,forage,antenne_bts,moulin,long,lat,code_village,code_prog,idLocalite")] villages villages)
        {
            if (ModelState.IsValid)
            {
                db.Entry(villages).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(villages);
        }

        public string nouvellecommune([Bind(Include = "nom, iddepartement")] communes com)
        {

            string listecoms = "";

            var codedept = db.communes.Where(c => c.iddepartement == com.iddepartement).ToList();
            var code = com.iddepartement + "" + (codedept.Count() + 1);
            com.code_com = Int32.Parse(code);
            db.communes.Add(com);
            db.SaveChanges();
            var communes = db.communes.Where(c => c.iddepartement == com.iddepartement).ToList();
            var commune = communes.OrderByDescending(b => b.id);
            foreach (communes coms in commune)
            {

                listecoms += "<option value='" + coms.code_com + "'>" + coms.nom + "</option>";

            }
            return listecoms;
        }


        public string nouveaudepartement([Bind(Include = "nom,idregion")] departements departements)
        {

            string listedepts = "";

            departements.code_departement = 0;
            db.departements.Add(departements);
            db.SaveChanges();
            var depts = db.departements.Where(d => d.idregion == departements.idregion).ToList();
            var deps = depts.OrderByDescending(b => b.code_departement).ToList();
            foreach (departements de in deps)
            {

                listedepts += "<option value='" + de.code_departement + "'>" + de.nom + "</option>";

            }
            return listedepts;
        }


        [Authorize(Roles = "Administrateurs")]
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