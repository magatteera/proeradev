
	   
 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using NetTopologySuite.GeometriesGraph;
using Newtonsoft.Json;
using proera;

namespace proera.Controllers
{


	public class myJson
	{
		public int nivpuissance { get; set; }
		public string calibre { get; set; }
		public string typeelec { get; set; }
		public int typebranchement { get; set; }

		public string villages { get; set; }

	}


	public class clientsController : Controller
	{
		private ERADEVEntities3 db = new ERADEVEntities3();

		// GET: clients
		public ActionResult Index(string message = "")
		{
			//var clients = db.clients.Include(c => c.etatclient).Include(c => c.villages);
			var regions = db.regions.ToList();
			ViewBag.regions = new SelectList(regions, "id", "nom_region");
			var departement = db.departements.ToList();
			var deptfilt = new List<departements>();
			foreach (departements dept in departement)
			{
				if (dept.idregion == regions[0].id)
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
			ViewBag.codevillage = new SelectList(villagefilt, "code_village", "village");

			ViewBag.ajout = false;

			if (message.IsNullOrWhiteSpace())
				return View();
			else return View(message);


		}

		// GET: clients/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			clients clients = db.clients.Find(id);
			if (clients == null)
			{
				return HttpNotFound();
			}

			clients.Village = db.villages.Find(clients.codevillage).village;
			if(clients.Etat_Client != null)
			ViewBag.etatclient = db.etatclient.Find(clients.Etat_Client).etat;
			if (clients.Contrat != null)
				ViewBag.contrat = db.contrats.Find(clients.Contrat).contrat;
			if(clients.NivPuissance!=null)
			ViewBag.nivpui = db.hnivpuissances.Find(clients.NivPuissance).NivPuissance;
			return View(clients);
		}

		public RedirectToRouteResult recherche(int reference)
		{
			clients clients = db.clients.Find(reference);
			if (clients == null)
				return RedirectToAction("Index", new { message = "aucun objet trouvé." });
			//clients client = db.clients
			//this.Details(clients.ID);
			//clients. = db.hnivpuissances.Find(clients.NivPuissance).NivPuissance;
			return RedirectToAction("Details", new { id = reference });
		}

		

		public ActionResult rechercheParNom([Bind(Include = "Nom1")] clients client)
		{
			string nom = client.Nom1;
			var clients = db.clients.Where(c => c.Nom1.Contains(nom)).ToList();

			//foreach(var cl in clients)
			//{
			//    clients[clients.FindIndex(c => c.Reference_Contrat == cl.Reference_Contrat)].NivPuissance = db.hnivpuissances.Find(cl.NivPuissance).NivPuissance;
			//}
			return View("ResultatRecherche", clients);
		}

		public ActionResult rechercheEnAttenteBordereau([Bind(Include = "numero")] bordereaux bor)
		{
			var bord = db.bordereaux.Where(b => b.numero == bor.numero).ToList()[0].id;
			var clients = db.clients.Where(c => (c.Bordereau == bord ) && (c.Etat_Client == 5)).ToList();

			var villages = db.villages.ToList();
			//foreach(var cl in clients)
			//{
			//    clients[clients.FindIndex(c => c.Reference_Contrat == cl.Reference_Contrat)].NivPuissance = db.hnivpuissances.Find(cl.NivPuissance).NivPuissance;
			//}

			List<clients> cliss = new List<clients>();
			foreach (var cl in clients)
			{
				cl.Village = villages[villages.FindIndex(v => v.code_village == cl.codevillage)].village;
				cliss.Add(cl);
			}

			ViewBag.message = "Liste des clients en attente sur le Bordereau: "+bor.numero;
			return View("ResultatRecherche", cliss);
		}
		public ActionResult rechercheEnAttente([Bind(Include = "Nom1")] clients client)
		{

			return View("enattente");
		}


		

			  public ActionResult rechercheenAttente2([Bind(Include = "Village")] clients clients)
		{
			var clis = db.clients.Where(c => (c.Village == clients.Village) && (c.Etat_Client == 5)).ToList();
			var villages = db.villages.ToList();

			List<clients> cliss = new List<clients>();
			foreach (var cl in clis)
			{

				cl.Village = villages[villages.FindIndex(v => v.code_village == cl.codevillage)].village;
				cliss.Add(cl);
			}

			return View("ResultatRecherche", cliss);
		}


		// GET: clients/CreateProductif
		[Authorize(Roles = "Proera_COM")]
		public ActionResult CreateProductif(string id)
		{
			var baremes = db.hbaremes.Where(n => (n.Usage == 2) || (n.Usage == 3)).ToList();
			var nivpui = db.hnivpuissances.Where(n => (n.Usage == 2) || (n.Usage == 3)).ToList();
			var calibres = db.hcalibres.ToList();
			var calibresfilt = new List<hcalibres>();
			foreach (hcalibres cal in calibres)
			{
				bool existe = false;
				foreach (hbaremes b in baremes)
				{
					if (b.Calibre2 == cal.Id && b.NiveauPuissance == nivpui[0].Id)
						existe = true;
				}
				if (existe)

					calibresfilt.Add(cal);
			}
			var typeelec = db.typeelec.ToList();
			var typebranche = db.hbranches.ToList();
			var branchesfilt = new List<hbranches>();
			foreach (hbranches br in typebranche)
			{
				bool existe = false;
				foreach (hbaremes b in baremes)
				{
					if (b.branch == br.Id)
						existe = true;
				}
				if (existe)
					branchesfilt.Add(br);
			}

			var bord = db.bordereaux.Where(b => b.ouvert == 1).ToList();
			ViewBag.NivPuissance = new SelectList(nivpui, "Id", "NivPuissance");
			ViewBag.activite = new SelectList(db.raisonsociale.ToList(), "id", "raison");
			ViewBag.calibres = new SelectList(calibresfilt, "calibre", "calibre");
			ViewBag.typeelec = new SelectList(typeelec, "type", "type");
			ViewBag.branchements = new SelectList(branchesfilt, "id", "branch");
			IEnumerable<SelectListItem> selectListbord = from s in bord
														 select new SelectListItem
														 {
															 Value = s.id + "",
															 Text = s.numero 
														 };
			ViewBag.bordereau = new SelectList(selectListbord, "Value", "Text");
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
			ViewBag.communes = new SelectList(communefilt, "code_com", "nom");
			ViewBag.codevillage = new SelectList(villagefilt, "code_village", "village");

			ViewBag.ajout = false;
			if (id != null)
			{
				if (Int32.Parse(id) == 1)
					ViewBag.ajout = true;
			}
			return View();
		}

		// GET: clients/CreateDomestique
		
	  //<roleManager enabled = "true" />
		//[Authorize(Roles = "Proera_COM, Administrateurs")]
		//[Authorize(Users = "administrateur")]
		public ActionResult CreateDomestique(string id)
		{
			var baremes = db.hbaremes.Where(b => b.Usage == 1).ToList();
			var nivpui = db.hnivpuissances.Where(n => n.Usage == 1).ToList();
			var calibres = db.hcalibres.ToList();
			var calibresfilt = new List<hcalibres>();
			foreach (hcalibres cal in calibres)
			{
				bool existe = false;
				foreach (hbaremes b in baremes)
				{
					if (b.Calibre2 == cal.Id && b.NiveauPuissance == nivpui[0].Id)
						existe = true;
				}
				if (existe)

					calibresfilt.Add(cal);
			}
			var typeelec = db.typeelec.ToList();
			var typebranche = db.hbranches.ToList();
			var branchesfilt = new List<hbranches>();
			foreach (hbranches br in typebranche)
			{
				bool existe = false;
				foreach (hbaremes b in baremes)
				{
					if (b.branch == br.Id)
						existe = true;
				}
				if (existe)
					branchesfilt.Add(br);
			}
			var bord = db.bordereaux.Where(b => (b.ouvert == 1) && (b.utilisateur == User.Identity.Name)).ToList();
			ViewBag.NivPuissance = new SelectList(nivpui, "Id", "NivPuissance");
			ViewBag.calibres = new SelectList(calibresfilt, "calibre", "calibre");
			ViewBag.typeelec = new SelectList(typeelec, "type", "type");
			ViewBag.branchements = new SelectList(branchesfilt, "id", "branch");
			IEnumerable<SelectListItem> selectListbord = from s in bord
														 select new SelectListItem
														 {
															 Value = s.id + "",
															 Text = s.numero 
														 };
			ViewBag.bordereau = new SelectList(selectListbord, "Value", "Text");
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
			ViewBag.communes = new SelectList(communefilt, "code_com", "nom");
			ViewBag.codevillage = new SelectList(villagefilt, "code_village", "village");

			ViewBag.modePaiement = new SelectList(db.typepaiement.ToList(), "id", "Type");

			ViewBag.ajout = false;
			if (id != null)
			{
				if (Int32.Parse(id) == 1)
					ViewBag.ajout = true;
			}

			return View();
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


		public string changementcommuneindex([Bind(Include = "code_com")] communes com)
		{

			var villagess = db.villages.Where(v => v.idLocalite == com.code_com).ToList();
			string villagesselect = "<option value=''>Selectionnez le village</option>";
			foreach (villages v in villagess)
			{

				villagesselect += "<option value='" + v.code_village + "'>" + v.village + "</option>";

			}

			return villagesselect;
		}



		// POST: clients/Create
		// Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
		// plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]

		public ActionResult Create([Bind(Include = "numeropaiement,ID,Nom1,Prenom,Num_ID,Tel,Raison_Social,Date_Abonnement,Village,Niv_Service,Type_Elect,calibre,Reference_Contrat,Montant_Encaisse,Etat_Client,Commentaire,Date_Mise_en_Service,Date_Resiliation,X_GPS,Y_GPS,Bordereau,Contrat,interface,Ancien_indexe,SoldeTotal,IDPayment,Prev_Bill,Last_Bill,NbrEP,Modifié,Créé,Créé_par,periodeFacturee,dateCoupure,dateAbonn,dateMeS,idlastbill,NivPuissance,activite,sqlstate,refclient,codevillage,modePaiement")] clients clients)
		{
			if (ModelState.IsValid)
			{
				clients.Contrat = 3;
				clients.Etat_Client = 5;
				clients.NivPuissance = db.hnivpuissances.Where(n => n.Id == clients.NivPuissance).ToList()[0].Id;
				clients.Créé_par = User.Identity.Name;
				db.clients.Add(clients);
				db.SaveChanges();
				var cl = db.clients.Where(c => c.ID == db.clients.Max(u => u.ID)).ToList();
				return RedirectToAction("Details", new { id = cl[0].Reference_Contrat });
			}

			//ViewBag.Etat_Client = new SelectList(db.etatclient, "id", "etat", clients.Etat_Client);
			//ViewBag.codevillage = new SelectList(db.villages, "idLocalite", "LOCALITE", clients.codevillage);
			return RedirectToAction("Index");
		}





		public string appliquerMisEnVigueur([Bind(Include = "bordereau, commentaire")] forcermiseenvigueur force)
		{
			var clientss = db.clients.Where(c => c.Bordereau == force.bordereau).ToList();

			double montant = 0;

			foreach (clients cli in clientss)
			{
				if(cli.Montant_Encaisse != null)
				montant += (double)cli.Montant_Encaisse;
				cli.Contrat = 1;
				db.Entry(cli).State = EntityState.Modified;
				db.SaveChanges();
			}
			var idbord = (int)force.bordereau;
			var bordereau = db.bordereaux.Where(b => b.id == idbord).ToList();
			bordereaux bd = bordereau[0];
			bd.ouvert = 0;
			db.Entry(bd).State = EntityState.Modified;
			db.SaveChanges();

			force.date = DateTime.Now;
			force.montantbordereau = bd.montant;
			force.montantencaisse = montant;
			force.utilisateur = User.Identity.Name;


			db.forcermiseenvigueur.Add(force);
			db.SaveChanges();
			//string role = User.IsInRole("Administrateurs");
			 
			return "Mise en vigueur sur le bordereau "+force.bordereau + " faite avec succés!";
		}




		public ActionResult getClientByBordereau([Bind(Include = "Bordereau")] clients clients)
		{
			var clientss = db.clients.Where(c => c.Bordereau == clients.Bordereau).ToList();

			string selectclients = "";
			double montant = 0;

			double especes = 0;
			double cheques = 0;
			double coupons = 0;

			foreach (clients cli in clientss)
			{
				selectclients += "<tr>" +
					"<td>" + cli.Nom1 + "</td>" +
					"<td>" + cli.Prenom + "</td>" +
					"<td>" + db.villages.Find(cli.codevillage).village + "</td>" +
					"<td>" + cli.Reference_Contrat + "</td>" +
					"<td>" + cli.Montant_Encaisse + "</td>" +
					"</tr>";
				if (cli.Montant_Encaisse != null)
				{
					montant += (double)cli.Montant_Encaisse;

					//if(cli.modePaiement == )
					if (cli.typepaiement.type == "especes")
						especes += (double)cli.Montant_Encaisse;
					if (cli.typepaiement.type == "coupon")
						coupons += (double)cli.Montant_Encaisse;
					if (cli.typepaiement.type == "cheque")
						cheques += (double)cli.Montant_Encaisse;
				}
				//"<td><a href='/clients/mettreEnVigueurValide/"+ cli.Bordereau +"'>Edit</a>" +
			}
			if (clientss.Count >= 1)
			{
				int idbordereau = (int)clientss[0].Bordereau;
				var bordereau = db.bordereaux.Where(b => b.id == idbordereau).ToList();
				return Json(new
				{
					selectclient = selectclients,
					montantbordereau = bordereau[0].montant,
					totalencaisse = montant,
					nombreclients = clientss.Count,
					montantespeces = especes,
					montantcheques = cheques,
					montantcoupons = coupons
				});
			}
			return Json(new
			{
				selectclient = selectclients,
				montantbordereau = 0,
				totalencaisse = montant,
				nombreclients = clientss.Count,
				montantespeces = 0,
				montantcheques = 0,
				montantcoupons = 0
			});
		}

		public ActionResult rechercheParVillage([Bind(Include = "codevillage")] clients clients)
		{
			var clis = db.clients.Where(c => c.codevillage == clients.codevillage).ToList();
			List<clients> cliss = new List<clients>();
			var villages = db.villages.ToList();
			foreach(var cl in clis)
			{
				cl.Village = villages[villages.FindIndex(v => v.code_village == cl.codevillage)].village;
				cliss.Add(cl);
			}

			return View("ResultatRecherche", cliss);
		}


		public ActionResult bondecoupure()
		{
			//var clients = db.clients.Where().ToList();

			return View("ResultatRecherche");
		}

		public ActionResult couper()
		{
			//var facts = db.factures.Where(f => (f.Paiement == false) && (f.penalite > 0)).ToList();
			//var clients = new List<clients>();

			//foreach(var f in facts)
			//{
			//    clients.Add(db.clients.Find(f.RefClient));
			//}

			var recouvs = db.recouvrements.Where(r => (r.active == 0) && (r.utilisateur == User.Identity.Name)).ToList();
			ViewBag.periode = new SelectList(recouvs, "periode", "periode");

			return View("coupure");
		}


		public ActionResult validerCoupure([Bind(Include = "REFERENCE,DATE,MOTIF,REMARQUE,PERIODE,index")] SAV sav)
		{
			try
			{
				var client = db.clients.Find(sav.REFERENCE);
				sav.ACTION = "COUPE";
				sav.OLDETAT = "EN SERVICE";
				sav.NEWETAT = "COUPE";
				sav.NOM = client.Nom1;
				sav.PRENOM = client.Prenom;
				sav.VILLAGE = client.Village;
				sav.createby = User.Identity.Name;
				sav.createdate = DateTime.Now;
				sav.periodedue = sav.PERIODE;


				db.SAV.Add(sav);
				db.SaveChanges();

				string liste = "";

				//var facts = db.factures.Where(f => (f.Paiement == false) && (f.penalite > 0) && (f.PeriodeFacturee == rec.periode)).ToList();
				var facts = db.factures.Where(f => (f.Paiement == 0) && (f.PeriodeFacturee == sav.PERIODE)).ToList();
				var clients = new List<clients>();

				foreach (var f in facts)
				{
					clients.Add(db.clients.Find(f.RefClient));
				}

				foreach (var cli in clients)
				{


					try
					{
						if (cli.NivPuissance != null)
							liste = liste + "<tr>" +
							"<td>" + (cli.Nom1 == null ? "" : cli.Nom1) + "</td>" +
							"<td>" + (cli.Prenom == null ? "" : cli.Prenom) + "</td>" +
							"<td>" + (cli.Village == null ? "" : cli.Village) + "</td>" +
							"<td>" + cli.Reference_Contrat + "</td>" +
							"<td>" + db.hnivpuissances.Find(cli.NivPuissance).NivPuissance + "</td>" +
							"<td>" + cli.Etat_Client + "</td>" +
							"<td><button class='btn btn-danger' ref='" + cli.Reference_Contrat + "'" +
							" nom='" + cli.Nom1 + "'" +
							" prenom='" + cli.Prenom + "'" +
							" village='" + db.villages.Where(v => v.code_village == cli.codevillage).ToList()[0].village + "'" +
							" id='btncouper" + cli.Reference_Contrat + "'>Couper</button></td>" +
							"</tr>";

						else liste = liste + "<tr>" +
						"<td>" + (cli.Nom1 == null ? "" : cli.Nom1) + "</td>" +
						"<td>" + (cli.Prenom == null ? "" : cli.Prenom) + "</td>" +
						"<td>" + (cli.Village == null ? "" : cli.Village) + "</td>" +
						"<td>" + cli.Reference_Contrat + "</td>" +
						"<td>non defini</td>" +
						"<td>" + cli.Etat_Client + "</td>" +

						"<td><button class='btn btn-danger' ref='" + cli.Reference_Contrat + "'" +
						" nom='" + cli.Nom1 + "'" +
						" prenom='" + cli.Prenom + "'" +
						" village='" + db.villages.Where(v => v.code_village == cli.codevillage).ToList()[0].village + "'" +
						" id='btncouper" + cli.Reference_Contrat + "'>Couper</button></td>" +
						"</tr>";
					}
					catch (Exception e)
					{

					}
				}

				return Json(new { ok = true, cli = liste });
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return Json(new { ok = false });
			}

			return Json(new { ok = false });
		}


		public ActionResult changementRefRetablissement([Bind(Include = "Reference_Contrat")] clients clients)
		{
			//clients
			var cli = db.clients.Find(clients.Reference_Contrat);
			if (cli != null)
			{

				var facs = db.factures.Where(f => (f.Paiement == 0) && (f.RefClient == cli.Reference_Contrat)).ToList();
				String tabfacs = "";
				foreach(var fac in facs)
				{
					tabfacs += "<tr>" +
						"<td>" + fac.dateReleve + "</td>" +
						"<td>" + fac.netPayer + "</td>" +
						 "</tr>";
				}

				return Json(new
				{
					message = "trouve",
					nom = cli.Nom1,
					prenom = cli.Prenom,
					tel = cli.Tel,
					factures = tabfacs
				});
			}


			return Json(new { message = "absent" });
		}
		public ActionResult validerRetablissement([Bind(Include = "Reference_Contrat")] clients clients)
		{
			//clients
			try
			{
				var cli = db.clients.Find(clients.Reference_Contrat);

				cli.Etat_Client = 1;
				db.Entry(cli).State = EntityState.Modified;
				db.SaveChanges();

				var facs = db.factures.Where(f => f.RefClient == cli.Reference_Contrat).ToList();
				foreach(var fac in facs)
				{
					fac.Paiement = 1;
					db.Entry(fac).State = EntityState.Modified;
					db.SaveChanges();
				}

				return Json(new { message = "Client retabli avec succes." });
			}
			catch (Exception e)
			{
				return Json(new { message = "Une erreur est survenue !!" });
			}


		}


		public string changementPeriodeCoupure([Bind(Include = "periode")] recouvrements rec)
		{
			string liste = "";

			//var facts = db.factures.Where(f => (f.Paiement == false) && (f.penalite > 0) && (f.PeriodeFacturee == rec.periode)).ToList();
			var facts = db.factures.Where(f => (f.Paiement == 0) && (f.PeriodeFacturee == rec.periode)).ToList();
			var clients = new List<clients>();

			foreach (var f in facts)
			{
				clients.Add(db.clients.Find(f.RefClient));
			}

			var villages = db.villages.ToList();
			var nivpuis = db.hnivpuissances.ToList();


			foreach (var cli in clients)
			{


				try
				{
					if(cli.NivPuissance != null)
					liste = liste + "<tr>" +
					"<td>" + (cli.Nom1 == null ? "" : cli.Nom1) + "</td>" +
					"<td>" + (cli.Prenom == null ? "" : cli.Prenom) + "</td>" +
					"<td>" + (cli.Village == null ? "" : cli.Village) + "</td>" +
					"<td>" + cli.Reference_Contrat + "</td>" +
					"<td>" + nivpuis[nivpuis.FindIndex(v => v.Id == cli.NivPuissance)].NivPuissance    + "</td>" +
					"<td>" + cli.Etat_Client + "</td>" +
					"<td><button class='btn btn-danger' ref='" + cli.Reference_Contrat + "'" +
					" nom='"+cli.Nom1+"'" +
					" prenom='" + cli.Prenom + "'" +
					" village='" + villages[villages.FindIndex(v => v.code_village == cli.codevillage)].village + "'" +
					" id='btncouper" + cli.Reference_Contrat + "'>Couper</button></td>" +
					"</tr>";

					else liste = liste + "<tr>" +
					"<td>" + (cli.Nom1 == null ? "" : cli.Nom1) + "</td>" +
					"<td>" + (cli.Prenom == null ? "" : cli.Prenom) + "</td>" +
					"<td>" + (cli.Village == null ? "" : cli.Village) + "</td>" +
					"<td>" + cli.Reference_Contrat + "</td>" +
					"<td>non defini</td>" +
					"<td>" + cli.Etat_Client + "</td>" +

					"<td><button class='btn btn-danger' ref='" + cli.Reference_Contrat + "'" +
					" nom='" + cli.Nom1 + "'" +
					" prenom='" + cli.Prenom + "'" +
					" village='" + villages[villages.FindIndex(v => v.code_village == cli.codevillage)].village + "'" +
					" id='btncouper" + cli.Reference_Contrat + "'>Couper</button></td>" +
					"</tr>";

				}
				catch (Exception e)
				{

				}
			}

			return liste;
		}

		public ActionResult retablir()
		{
			//var clients = db.clients.Where().ToList();

			return View("Retablir");
		}

		public ActionResult changerinterface()
		{
			//var clients = db.clients.Where().ToList();

			return View("changerinterface");
		}

		public ActionResult changementRefInterface([Bind(Include = "Reference_Contrat")] clients clients)
		{
			//clients
			var cli = db.clients.Find(clients.Reference_Contrat);
			if (cli != null)
			{
				return Json(new
				{
					message = "trouve",
					nom = cli.Nom1,
					oldInterface = cli.numcompteur,
				});
			}

			return Json(new { message = "absent" });
		}


		public ActionResult validerChangementInterface([Bind(Include = "oldinterface, newinterface, indexdepose, indexpose, refclient")] changementinterface changementinterface)
		{
			try
			{
				changementinterface.utilisateur = User.Identity.Name;
				db.changementinterface.Add(changementinterface);
				db.SaveChanges();


				return Json(new { message = "Changement Fait avec succes." });
			}
			catch (Exception e)
			{
				return Json(new { message = "Une erreur s'est produite!!" });
			}

		}


		//public ActionResult reclamation()
		//{
		//    //var clients = db.clients.Where().ToList();

		//    return View("ResultatRecherche");
		//}
		//public ActionResult resilier()
		//{
		//    //var clients = db.clients.Where().ToList();

		//    return View("ResultatRecherche");
		//}


		//[Authorize(Roles = "REC")]
		public ActionResult MigrerClient(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			clients clients = db.clients.Find(id);
			if (clients == null)
			{
				return HttpNotFound();
			}

			var baremes = db.hbaremes.ToList();
			var nivpui = db.hnivpuissances.ToList();
			var calibres = db.hcalibres.ToList();
			var calibresfilt = new List<hcalibres>();
			foreach (hcalibres cal in calibres)
			{
				bool existe = false;
				foreach (hbaremes b in baremes)
				{
					if (b.Calibre2 == cal.Id && b.NiveauPuissance == nivpui[0].Id)
						existe = true;
				}
				if (existe)

					calibresfilt.Add(cal);
			}
			var typeelec = db.typeelec.ToList();
			var typebranche = db.hbranches.ToList();
			var branchesfilt = new List<hbranches>();
			foreach (hbranches br in typebranche)
			{
				bool existe = false;
				foreach (hbaremes b in baremes)
				{
					if (b.branch == br.Id)
						existe = true;
				}
				if (existe)
					branchesfilt.Add(br);
			}
			var bord = db.bordereaux.Where(b => b.ouvert == 1).ToList();
			ViewBag.NivPuissance = new SelectList(nivpui, "NivPuissance", "NivPuissance");
			ViewBag.calibres = new SelectList(calibresfilt, "calibre", "calibre");
			ViewBag.typeelec = new SelectList(typeelec, "type", "type");
			ViewBag.branchements = new SelectList(branchesfilt, "id", "branch");
			IEnumerable<SelectListItem> selectListbord = from s in bord
														 select new SelectListItem
														 {
															 Value = s.id + "",
															 Text = s.numero 
														 };
			ViewBag.bordereau = new SelectList(selectListbord, "Value", "Text");


			return View("~/Views/clients/Migrer.cshtml", clients);
		}


		// GET: clients/Edit/5
		//[Authorize(Roles = "COM")]
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			clients clients = db.clients.Find(id);
			if (clients == null)
			{
				return HttpNotFound();
			}

			ViewBag.Etat_Client = clients.Etat_Client;
			ViewBag.Contrat = clients.Contrat;

			if (clients.Contrat == 3)
			{
				if (clients.Raison_Social == null)
					RedirectToAction("Index");

				if(clients.Raison_Social.Contains("PARTICULIER"))
				{
					var baremes = db.hbaremes.Where(b => b.Usage == 1).ToList();
					var nivpui = db.hnivpuissances.Where(n => n.Usage == 1).ToList();
					var calibres = db.hcalibres.ToList();
					var calibresfilt = new List<hcalibres>();
					foreach (hcalibres cal in calibres)
					{
						bool existe = false;
						foreach (hbaremes b in baremes)
						{
							if (b.Calibre2 == cal.Id && b.NiveauPuissance == nivpui[0].Id)
								existe = true;
						}
						if (existe)

							calibresfilt.Add(cal);
					}
					var typeelec = db.typeelec.ToList();
					var typebranche = db.hbranches.ToList();
					var branchesfilt = new List<hbranches>();
					foreach (hbranches br in typebranche)
					{
						bool existe = false;
						foreach (hbaremes b in baremes)
						{
							if (b.branch == br.Id)
								existe = true;
						}
						if (existe)
							branchesfilt.Add(br);
					}
					var bord = db.bordereaux.Where(b => (b.ouvert == 1) && (b.utilisateur == User.Identity.Name)).ToList();
					ViewBag.NivPuissance = new SelectList(nivpui, "Id", "NivPuissance");
					ViewBag.calibres = new SelectList(calibresfilt, "calibre", "calibre");
					ViewBag.typeelec = new SelectList(typeelec, "type", "type");
					ViewBag.branchements = new SelectList(branchesfilt, "id", "branch");
					IEnumerable<SelectListItem> selectListbord = from s in bord
																 select new SelectListItem
																 {
																	 Value = s.id + "",
																	 Text = s.numero
																 };
					ViewBag.bordereau = new SelectList(selectListbord, "Value", "Text");
					var region1 = db.regions.ToList();
					ViewBag.regions = new SelectList(region1, "id", "nom_region");
					var departement1 = db.departements.ToList();
					var deptfilt1 = new List<departements>();
					foreach (departements dept in departement1)
					{
						if (dept.idregion == region1[0].id)
							deptfilt1.Add(dept);
					}
					var commune1 = db.communes.ToList();
					var communefilt1 = new List<communes>();
					foreach (communes com in commune1)
					{
						if (com.iddepartement == deptfilt1[0].code_departement)
							communefilt1.Add(com);
					}
					var village1 = db.villages.ToList();
					var villagefilt1 = new List<villages>();
					foreach (villages v in village1)
					{
						if (v.idLocalite == communefilt1[0].code_com)
							villagefilt1.Add(v);
					}
					ViewBag.idmodePaiement = clients.modePaiement;


					ViewBag.departements = new SelectList(deptfilt1, "code_departement", "nom");
					ViewBag.communes = new SelectList(communefilt1, "code_com", "nom");
					ViewBag.codevillage = new SelectList(villagefilt1, "code_village", "village");

					ViewBag.modePaiement = new SelectList(db.typepaiement.ToList(), "id", "Type");

					ViewBag.ajout = false;

					return View("updateDomNonEnvigueur", clients);
				} else
				{
					var baremes = db.hbaremes.Where(n => (n.Usage == 2) || (n.Usage == 3)).ToList();
					var nivpui = db.hnivpuissances.Where(n => (n.Usage == 2) || (n.Usage == 3)).ToList();
					var calibres = db.hcalibres.ToList();
					var calibresfilt = new List<hcalibres>();
					foreach (hcalibres cal in calibres)
					{
						bool existe = false;
						foreach (hbaremes b in baremes)
						{
							if (b.Calibre2 == cal.Id && b.NiveauPuissance == nivpui[0].Id)
								existe = true;
						}
						if (existe)

							calibresfilt.Add(cal);
					}
					var typeelec = db.typeelec.ToList();
					var typebranche = db.hbranches.ToList();
					var branchesfilt = new List<hbranches>();
					foreach (hbranches br in typebranche)
					{
						bool existe = false;
						foreach (hbaremes b in baremes)
						{
							if (b.branch == br.Id)
								existe = true;
						}
						if (existe)
							branchesfilt.Add(br);
					}

					var bord = db.bordereaux.Where(b => b.ouvert == 1).ToList();
					ViewBag.NivPuissance = new SelectList(nivpui, "Id", "NivPuissance");
					ViewBag.Raison_Social = new SelectList(db.raisonsociale.ToList(), "id", "raison");
					ViewBag.calibres = new SelectList(calibresfilt, "calibre", "calibre");
					ViewBag.typeelec = new SelectList(typeelec, "type", "type");
					ViewBag.branchements = new SelectList(branchesfilt, "id", "branch");
					IEnumerable<SelectListItem> selectListbord = from s in bord
																 select new SelectListItem
																 {
																	 Value = s.id + "",
																	 Text = s.numero
																 };
					ViewBag.bordereau = new SelectList(selectListbord, "Value", "Text");
					var region2 = db.regions.ToList();
					ViewBag.regions = new SelectList(region2, "id", "nom_region");
					var departement2 = db.departements.ToList();
					var deptfilt2 = new List<departements>();
					foreach (departements dept in departement2)
					{
						if (dept.idregion == region2[0].id)
							deptfilt2.Add(dept);
					}
					var commune2 = db.communes.ToList();
					var communefilt2 = new List<communes>();
					foreach (communes com in commune2)
					{
						if (com.iddepartement == deptfilt2[0].code_departement)
							communefilt2.Add(com);
					}
					var village2 = db.villages.ToList();
					var villagefilt2 = new List<villages>();
					foreach (villages v in village2)
					{
						if (v.idLocalite == communefilt2[0].code_com)
							villagefilt2.Add(v);
					}

					ViewBag.departements = new SelectList(deptfilt2, "code_departement", "nom");
					ViewBag.communes = new SelectList(communefilt2, "code_com", "nom");
					ViewBag.codevillage = new SelectList(villagefilt2, "code_village", "village");

					ViewBag.ajout = false;
					return View("updateProdNonEnvigueur", clients);
				}
			}

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
			ViewBag.communes = new SelectList(communefilt, "code_com", "nom");
			ViewBag.codevillage = new SelectList(villagefilt, "code_village", "village");

			ViewBag.Etat_Client = new SelectList(db.etatclient, "id", "etat", clients.Etat_Client);
			ViewBag.codevillage = new SelectList(db.villages, "code_village", "village", clients.codevillage);

			if (clients.Contrat != 1)
			{

			}

			return View(clients);
		}

		//[Authorize(Roles = "COM")]
		public ActionResult mettreEnVigueur()
		{

			ViewBag.bordereaux = new SelectList(db.bordereaux.Where(b => b.ouvert == 1), "id", "numero");
			//ViewBag.role = System.Web.Security.Roles.n
			return View("MettreEnVigueur");
		}

		//[Authorize(Roles = "TECH")]
		public ActionResult mettreEnService()
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
			ViewBag.villages = new SelectList(villagefilt, "code_village", "village");
			//ViewBag.role = System.Web.Security.Roles.n
			return View("MettreEnService");
		}

		public ActionResult mettreEnServiceNonRac()
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
			ViewBag.villages = new SelectList(villagefilt, "code_village", "village");
			//ViewBag.role = System.Web.Security.Roles.n
			return View("mettreEnServiceNonRac");
		}


		//[Authorize(Roles = "TECH")]
		public string appliquerMiseEnService([Bind(Include = "numcompteur,indexDePose,dateMiseEnService,referenceClient,statut,commentaire")] miseenserviceclient miseenserviceclient)
		{
			miseenserviceclient.utilisateur = User.Identity.Name;
			var client = db.clients.Find(miseenserviceclient.referenceClient);
			if (!miseenserviceclient.statut.Contains("NON RACCORDABLE"))
			{
				client.Etat_Client = 1;
				client.Date_Mise_en_Service = miseenserviceclient.dateMiseEnService + "";
				client.numcompteur = miseenserviceclient.numcompteur;
				client.Ancien_indexe = miseenserviceclient.indexDePose+"";
			}
			else
			{
				client.Etat_Client = 6;
			}

			db.Entry(client).State = EntityState.Modified;
			db.SaveChanges();

			db.miseenserviceclient.Add(miseenserviceclient);
			db.SaveChanges();

			return "Fait Avec Succes";
		}

		public ActionResult clientsacouper()
        {
			return View("clientsacouper");
        }


		public String changementvillagemes([Bind(Include = "code_village")] villages vil)
		{
			var clientss = db.clients.Where(c => (c.codevillage == vil.code_village) && (c.Contrat == 1) && (c.Etat_Client == 5)).ToList();
			var niv = db.hnivpuissances.ToList();

			string selectclients = "";

			foreach (clients cli in clientss)
			{
				//if (cli.etatclient.id == 5 && cli.Contrat == 5)
				if(cli.NivPuissance != null)
				selectclients += "<tr>" +
					"<td>" + cli.Nom1 + "</td>" +
					"<td>" + cli.Prenom + "</td>" +
					"<td>" + cli.Tel + "</td>" +
					"<td>" + cli.Reference_Contrat + "</td>" +
					"<td>" + niv[niv.FindIndex(v => v.Id == cli.NivPuissance)].NivPuissance + "</td>" +
					"<td><button class='btn btn-info btn-sm btn-round' value='" + cli.Reference_Contrat + "' id='btnmettreenService-" + cli.Reference_Contrat + "' type='button'>Raccorder</button></td>" +
					"</tr>";

				else
					selectclients += "<tr>" +
						"<td>" + cli.Nom1 + "</td>" +
						"<td>" + cli.Prenom + "</td>" +
						"<td>" + cli.Village + "</td>" +
						"<td>" + cli.Reference_Contrat + "</td>" +
						"<td>non defini</td>" +
						"<td><button class='btn btn-info btn-sm btn-round' value='" + cli.Reference_Contrat + "' id='btnmettreenService-" + cli.Reference_Contrat + "' type='button'>Raccorder</button></td>" +
						"</tr>";

				//"<td><a href='/clients/mettreEnVigueurValide/"+ cli.Bordereau +"'>Edit</a>" +
			}

			return selectclients;
		}

		

		public String changementvillagemesNonRac([Bind(Include = "code_village")] villages vil)
		{
			var clientss = db.clients.Where(c => (c.codevillage == vil.code_village) && (c.Contrat == 1) && (c.Etat_Client == 6)).ToList();
			var niv = db.hnivpuissances.ToList();

			string selectclients = "";
			foreach (clients cli in clientss)
			{
				//if (cli.etatclient.id == 5 && cli.Contrat == 5)
				if (cli.NivPuissance != null)
					selectclients += "<tr>" +
						"<td>" + cli.Nom1 + "</td>" +
						"<td>" + cli.Prenom + "</td>" +
						"<td>" + cli.Village + "</td>" +
						"<td>" + cli.Reference_Contrat + "</td>" +
						"<td>" + niv[niv.FindIndex(v => v.Id == cli.NivPuissance)].NivPuissance + "</td>" +
						"<td><button class='btn btn-info btn-sm btn-round' value='" + cli.Reference_Contrat + "' id='btnmettreenService-" + cli.Reference_Contrat + "' type='button'>Raccorder</button></td>" +
						"</tr>";

				else
					selectclients += "<tr>" +
						"<td>" + cli.Nom1 + "</td>" +
						"<td>" + cli.Prenom + "</td>" +
						"<td>" + cli.Village + "</td>" +
						"<td>" + cli.Reference_Contrat + "</td>" +
						"<td>non defini</td>" +
						"<td><button class='btn btn-info btn-sm btn-round' value='" + cli.Reference_Contrat + "' id='btnmettreenService-" + cli.Reference_Contrat + "' type='button'>Raccorder</button></td>" +
						"</tr>";

				//"<td><a href='/clients/mettreEnVigueurValide/"+ cli.Bordereau +"'>Edit</a>" +
			}

			return selectclients;
		}

		

		public ActionResult changementregionmes([Bind(Include = "id")] regions reg)
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
		public ActionResult changementdepartementmes([Bind(Include = "code_departement")] departements dept)
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
		public string changementcommunemes([Bind(Include = "code_com")] communes com)
		{

			var villages = db.villages.Where(v => v.idLocalite == com.code_com).ToList();
			string selectvillages = "<option value=''>Selectionnez le Village</option>";
			foreach (villages vil in villages)
			{

				selectvillages += "<option value='" + vil.code_village + "'>" + vil.village + "</option>";

			}


			return selectvillages;
		}

		// GET: clients/Edit/5
		public String changement([Bind(Include = "nivpuissance, calibre, typeelec, typebranchement")] myJson json)
		{
			//var objet = JsonConvert.DeserializeObject<myJson>(json);
			var baremes = db.hbaremes.ToList();
			var baremeschanges = new List<hbaremes>();
			foreach (hbaremes bar in baremes)
			{
				if (bar.hnivpuissances.Id != json.nivpuissance ||
					bar.hcalibres.Calibre != json.calibre ||
					bar.Usage != 1 ||
					bar.branch != json.typebranchement)
				{

				}
				else
					baremeschanges.Add(bar);
			}

			string baremes2 = "";
			foreach (hbaremes bar in baremeschanges)
			{
				baremes2 = "<option value='" + bar.Montant + "'>" + bar.Montant + "</option>";
			}
			//return "<option value='" + 12 + "'>" + 20000 + "</option>";
			return baremes2;
		}

		public String changementProductif([Bind(Include = "nivpuissance, calibre, typeelec, typebranchement")] myJson json)
		{
			//var objet = JsonConvert.DeserializeObject<myJson>(json);
			var baremes = db.hbaremes.ToList();
			var baremeschanges = new List<hbaremes>();
			foreach (hbaremes bar in baremes)
			{
				if (bar.hnivpuissances.Id != json.nivpuissance ||
					bar.hcalibres.Calibre != json.calibre ||
					bar.Usage == 1 ||
					bar.branch != json.typebranchement)
				{

				}
				else
					baremeschanges.Add(bar);
			}

			string baremes2 = "";
			foreach (hbaremes bar in baremeschanges)
			{
				baremes2 += "<option value='" + bar.Montant + "'>" + bar.Montant + "</option>";
			}
			//return "<option value='" + 12 + "'>" + 20000 + "</option>";
			return baremes2;
		}




		public String savebordereau([Bind(Include = "numero,montant,type")] bordereaux bordereaux)
		{
			bordereaux.utilisateur = User.Identity.Name;
			bordereaux.ouvert = 1;
			bordereaux.datecreation = DateTime.Now;
			db.bordereaux.Add(bordereaux);
			db.SaveChanges();


			var bord1 = db.bordereaux.Where(b => b.ouvert == 1).ToList();
			var bord = bord1.OrderByDescending(b => b.id);

			string bords = "";
			foreach (bordereaux b in bord)
			{
				bords += "<option value='" + b.id + "'>" + b.numero + "</option>";
			}
			//return "<option value='" + 12 + "'>" + 20000 + "</option>";
			return bords;
		}

		// GET: clients/Edit/5
		public ActionResult changementnivpuissanceProductif([Bind(Include = "nivpuissance, calibre, typeelec, typebranchement")] myJson json)
		{
			//var objet = JsonConvert.DeserializeObject<myJson>(json);
			var baremes = db.hbaremes.ToList();
			var branches = db.hbranches.ToList();
			var calibres = db.hcalibres.ToList();
			var branchechanges = new List<hbranches>();
			var calibrechanges = new List<hcalibres>();
			var baremeschanges = new List<hbaremes>();
			string selectbranches = "";
			string selectcalibres = "";
			string baremes2 = "";


			foreach (hcalibres cal in calibres)
			{
				bool existe = false;
				foreach (hbaremes b in baremes)
				{
					if (b.Calibre2 == cal.Id && b.hnivpuissances.Id == json.nivpuissance)
						existe = true;
				}
				if (existe)
				{
					calibrechanges.Add(cal);
					selectcalibres += "<option value='" + cal.Id + "'>" + cal.Calibre + "</option>";
				}

			}


			foreach (hbranches branch in branches)
			{
				bool existe = false;
				foreach (hbaremes b in baremes)
				{
					if (b.branch == branch.Id && b.hnivpuissances.Id == json.nivpuissance)
						existe = true;
				}
				if (existe)
				{
					branchechanges.Add(branch);
					selectbranches += "<option value='" + branch.Id + "'>" + branch.Branch + "</option>";
				}

			}

			foreach (hbaremes bar in baremes)
			{

				if (bar.hnivpuissances.Id != json.nivpuissance ||
					bar.Calibre2 != calibrechanges[0].Id ||
					bar.Usage == 1 ||
					bar.branch != branchechanges[0].Id)
				{

				}
				else
				{
					baremeschanges.Add(bar);
					baremes2 = "<option value='" + bar.Montant + "'>" + bar.Montant + "</option>";
				}
			}

			//return "<option value='" + 12 + "'>" + 20000 + "</option>";
			return Json(new { baremes = baremes2, branches = selectbranches, calibres = selectcalibres });
		}

		public ActionResult changementnivpuissance([Bind(Include = "nivpuissance, calibre, typeelec, typebranchement")] myJson json)
		{
			//var objet = JsonConvert.DeserializeObject<myJson>(json);
			var baremes = db.hbaremes.ToList();
			var branches = db.hbranches.ToList();
			var calibres = db.hcalibres.ToList();
			var branchechanges = new List<hbranches>();
			var calibrechanges = new List<hcalibres>();
			var baremeschanges = new List<hbaremes>();
			string selectbranches = "";
			string selectcalibres = "";
			string baremes2 = "";


			foreach (hcalibres cal in calibres)
			{
				bool existe = false;
				foreach (hbaremes b in baremes)
				{
					if (b.Calibre2 == cal.Id && b.hnivpuissances.Id == json.nivpuissance)
						existe = true;
				}
				if (existe)
				{
					calibrechanges.Add(cal);
					selectcalibres += "<option value='" + cal.Id + "'>" + cal.Calibre + "</option>";
				}

			}


			foreach (hbranches branch in branches)
			{
				bool existe = false;
				foreach (hbaremes b in baremes)
				{
					if (b.branch == branch.Id && b.hnivpuissances.Id == json.nivpuissance)
						existe = true;
				}
				if (existe)
				{
					branchechanges.Add(branch);
					selectbranches += "<option value='" + branch.Id + "'>" + branch.Branch + "</option>";
				}

			}

			foreach (hbaremes bar in baremes)
			{

				if (bar.hnivpuissances.Id != json.nivpuissance ||
					bar.Calibre2 != calibrechanges[0].Id ||
					bar.Usage != 1 ||
					bar.branch != branchechanges[0].Id)
				{

				}
				else
				{

					baremeschanges.Add(bar);
					baremes2 = "<option value='" + bar.Montant + "'>" + bar.Montant + "</option>";
				}
			}

			//return "<option value='" + 12 + "'>" + 20000 + "</option>";
			return Json(new { baremes = baremes2, branches = selectbranches, calibres = selectcalibres });
		}



		// POST: clients/Edit/5
		// Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
		// plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		//[Authorize(Roles = "COM")]
		public ActionResult Edit([Bind(Include = "numeropaiement,Nom1,Prenom,Num_ID,Tel,Raison_Social,Date_Abonnement,Village,Niv_Service,Type_Elect,calibre,Reference_Contrat,Montant_Encaisse,Commentaire,Bordereau,SoldeTotal,IDPayment,Modifié,Créé,Créé_par,periodeFacturee,dateCoupure,dateAbonn,dateMeS,idlastbill,NivPuissance,activite,sqlstate,refclient,codevillage,modePaiement")] clients clients)
		{
			if (ModelState.IsValid)
			{
				clients.Etat_Client = 5;
				clients.Contrat = 3;
				db.Entry(clients).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			ViewBag.Etat_Client = new SelectList(db.etatclient, "id", "etat", clients.Etat_Client);
			ViewBag.codevillage = new SelectList(db.villages, "code_village", "village", clients.codevillage);
			return View(clients);
		}

		// GET: clients/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			clients clients = db.clients.Find(id);
			if (clients == null)
			{
				return HttpNotFound();
			}
			return View(clients);
		}



		// POST: clients/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			clients clients = db.clients.Find(id);
			db.clients.Remove(clients);
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
