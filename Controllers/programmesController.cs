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

    [Authorize(Roles = "Proera_SIG,Proera_Admin")]
    public class programmesController : Controller
    {
        private PROERAEntities db = new PROERAEntities();

        // GET: programmes
        public ActionResult Index()
        {
            ViewBag.programmes = new SelectList(db.programmes, "id", "nom");
            return View();
        }

        public ActionResult liste()
        {
            return View(db.programmes.ToList());
        }

        // GET: programmes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            programmes programmes = db.programmes.Find(id);
            if (programmes == null)
            {
                return HttpNotFound();
            }
            return View(programmes);
        }

        // GET: programmes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: programmes/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "nom,ID,tech,DateDebut,delai,budget,MTavance,niveau_decaissement,avenant,DateFin,descriptProg,remarque,operateur")] programmes programmes)
        {
            if (ModelState.IsValid)
            {
                db.programmes.Add(programmes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(programmes);
        }

        public ActionResult listevillages([Bind(Include = "ID")] programmes p)
        {
            var village = db.villages.Where(v => v.code_prog != p.ID).ToList();

            var villagein = db.villages.Where(v => v.code_prog == p.ID).ToList();

            string listebr = "";

            var coms = db.communes.ToList();

            foreach (var v in village)
            {
                listebr += 
                    "<tr>" + "<td>" + v.village + "</td>" +
                    "<td>" + coms[coms.FindIndex(re => re.code_com == v.idLocalite)].nom + "</td>" +
                    //"<td>" + db.communes.Where(c => c.code_com+"" == v.idLocalite+"").ToList()[0].nom +"</td>" +
                    "<td>" + v.code_village + "</td>" +
                    "<td>" + "<button class='btn btn-warning btn-sm' id='btnmodifier-" + v.id + "' code='" + v.code_village + "'>Attribuer</button>" + "</td>" +
                    "</tr>";
            }

            string listein = "";

            foreach (var v in villagein)
            {
                listein +=
                    "<tr>" + "<td>" + v.village + "</td>" +
                    "<td>" + coms[coms.FindIndex(re => re.code_com == v.idLocalite)].nom + "</td>" +
                    //"<td>" + db.communes.Where(c => c.code_com+"" == v.idLocalite+"").ToList()[0].nom +"</td>" +
                    "<td>" + v.code_village + "</td>" +
                    "</tr>";
            }

            return Json(new
            {
                villages = listebr, id = p.ID, villagesin = listein
            });
        }

        public ActionResult changerprogramme([Bind(Include = "code_prog, code_village")] villages vil)
        {
            var village = db.villages.Where(v => v.code_village == vil.code_village).ToList()[0];

            try
            {
                village.code_prog = vil.code_prog;
                db.Entry(village).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new
                {
                    message = "Programme attribué avec succés"
                });
            }
            catch (Exception e)
            {
                return Json(new
                {
                    message = "erreur"
                });
            }
        }


        // GET: programmes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            programmes programmes = db.programmes.Find(id);
            if (programmes == null)
            {
                return HttpNotFound();
            }
            return View(programmes);
        }

        // POST: programmes/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "nom,ID,tech,DateDebut,delai,budget,MTavance,niveau_decaissement,avenant,DateFin,descriptProg,remarque,operateur")] programmes programmes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(programmes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(programmes);
        }

        // GET: programmes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            programmes programmes = db.programmes.Find(id);
            if (programmes == null)
            {
                return HttpNotFound();
            }
            return View(programmes);
        }

        // POST: programmes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            programmes programmes = db.programmes.Find(id);
            db.programmes.Remove(programmes);
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
