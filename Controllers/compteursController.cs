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
    public class compteursController : Controller
    {
        private PROERAEntities db = new PROERAEntities();

        // GET: compteurs
        public ActionResult Index()
        {
            return View();
        }

        // GET: compteurs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            compteurs compteurs = db.compteurs.Find(id);
            if (compteurs == null)
            {
                return HttpNotFound();
            }
            return View(compteurs);
        }

        // GET: compteurs/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult importer(HttpPostedFileBase file)
        {


            string path = null;
            List<compteurs> compteurstoadd = new List<compteurs>();
            try
            {
                if (file.ContentLength > 0)
                {
                    var filename = Path.GetFileName(file.FileName);

                    //file.SaveAs(path);

                    path = Path.Combine(Server.MapPath("~/upload"), filename);
                    //path = "C:\\Users\\m.war\\source\\repos\\proera\\App_Data\\upload\\" + filename;
                    file.SaveAs(path);

                    var csv = new CsvReader(new StreamReader(path), CultureInfo.CurrentCulture);

                    //ViewBag.test2 = csv.GetRecord<>.ToString();
                    var compteurslist = csv.GetRecords<Compts>().ToList();
                   

                    foreach (var c in compteurslist)
                    {
                        compteurs co = new compteurs();
                        co.datereception = DateTime.Parse(c.datereception);
                        co.entrepot = c.entrepot;
                        co.fabricant = c.fabricant;
                        co.libre = 1;
                        co.utilisateur = User.Identity.Name;
                        co.type = c.type;
                        db.compteurs.Add(co);
                        db.SaveChanges();
                    }

                    return View("Index");
                }
            }
            catch (Exception e)
            {

            }


            //ViewBag.opendate = recouvrements.opendate;
            //ViewBag.filename = file.FileName;
            return View("~/Views/recouvrements/test.cshtml");
        }
        public ActionResult recherche([Bind(Include = "numerointerface")] compteurs compteurs)
        {
            var comps = db.compteurs.Where(c => c.numerointerface == compteurs.numerointerface).ToList();
            return View("liste", comps);
        }

        public ActionResult listedisponibles()
        {
            var comps = db.compteurs.Where(c => c.libre == 1).ToList();
            return View("liste", comps);
        }


        

        // POST: compteurs/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,numerointerface,datereception,utilisateur,libre,type,entrepot,fabricant")] compteurs compteurs)
        {
            if (ModelState.IsValid)
            {
                db.compteurs.Add(compteurs);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(compteurs);
        }

        // GET: compteurs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            compteurs compteurs = db.compteurs.Find(id);
            if (compteurs == null)
            {
                return HttpNotFound();
            }
            return View(compteurs);
        }

        // POST: compteurs/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,numerointerface,datereception,utilisateur,libre,type,entrepot,fabricant")] compteurs compteurs)
        {
            if (ModelState.IsValid)
            {
                db.Entry(compteurs).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(compteurs);
        }

        // GET: compteurs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            compteurs compteurs = db.compteurs.Find(id);
            if (compteurs == null)
            {
                return HttpNotFound();
            }
            return View(compteurs);
        }

        // POST: compteurs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            compteurs compteurs = db.compteurs.Find(id);
            db.compteurs.Remove(compteurs);
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
