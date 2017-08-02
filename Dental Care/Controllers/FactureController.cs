using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dental_Care.Models;
using Dental_Care.Helpers;

namespace Dental_Care.Controllers
{
    public class FactureController : Controller
    {
        // attr.
        private DentalCareDbContext dbContext = new DentalCareDbContext();

        // Actions

        //
        // GET: /Facture/
        public ActionResult Index()
        {
            // if user not logged in, go to login page
            if (!UserHelper.isLoggedIn()) return RedirectToAction("Login", "User");

            if (UserHelper.isAdmin() || UserHelper.isDentist(true))
            {
                List<Facture> factures = dbContext.Factures.ToList();
                return View(factures);
            }
            else
            {
                return RedirectToAction("Profil", "User", new { id = UserHelper.getId() });
            }
        }

        //
        // GET: /Facture/Create?consultationId=
        public ActionResult Create(int consultationId)
        {
            // if user not logged in, go to login page
            if (!UserHelper.isLoggedIn())
            {
                return RedirectToAction("Login", "User");
            }
            else if (!UserHelper.isAdmin() && !UserHelper.isDentist(true))
            {
                return RedirectToAction("Profil", "User", new { id = UserHelper.getId() });
            }

            // pass consultationId to view
            ViewBag.consultationId = consultationId;

            return View();
        }

        //
        // POST: /Facture/Create
        [HttpPost]
        public ActionResult Create(Facture facture)
        {
            // check if montant is > 0
            if (facture.Montant == 0)
            {
                ViewBag.Warning = "Le montant doit être supérieur à 0.";
            }
            else if (ModelState.IsValid) // all model fields are ok
            {
                try
                {
                    // generate ref
                    List<Facture> factures = dbContext.Factures.ToList();
                    int count = factures.Count;
                    facture.Ref = "FA-" + DateTime.Now.ToString("ddMM") + "-" + ++count;
                    // add facture
                    dbContext.Factures.Add(facture);
                    dbContext.SaveChanges();
                    Session["Message"] = "Facture créée !";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Une erreur s'est produite.";
                    ViewBag.ErrorDetails = ex.Message;
                }
            }
            else
            {
                ViewBag.Warning = "Veuillez remplir tout les champs.";
            }

            // pass consultationId to view
            ViewBag.consultationId = facture.ConsultationId;

            return View();
        }

        //
        // GET: /Facture/Edit/Id
        public ActionResult Edit(int id = -1)
        {
            // if user not logged in, go to login page
            if (!UserHelper.isLoggedIn())
            {
                return RedirectToAction("Login", "User");
            }
            else if (!UserHelper.isAdmin() && !UserHelper.isDentist(true))
            {
                return RedirectToAction("Profil", "User", new { id = UserHelper.getId() });
            }

            Facture facture = dbContext.Factures.SingleOrDefault(f => f.Id == id);
            if (facture == null)
            {
                return HttpNotFound();
            }

            return View(facture);
        }

        //
        // POST: /Facture/Edit/Id
        [HttpPost]
        public ActionResult Edit(Facture facture)
        {
            Facture checkFacture = dbContext.Factures.SingleOrDefault(f => f.Id == facture.Id);
            if (checkFacture == null)
            {
                return HttpNotFound();
            }

            // check if montant is > 0
            if (facture.Montant == 0)
            {
                ViewBag.Warning = "Le montant doit être supérieur à 0.";
            }
            else if (ModelState.IsValid)
            {
                checkFacture.Date = facture.Date;
                checkFacture.Montant = facture.Montant;
                dbContext.SaveChanges();
                Session["Message"] = "Modification effectuée!";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Warning = "Veuillez remplir tout les champs.";
            }

            // set ConsultationId & Ref (because they are not posted)
            facture.ConsultationId = checkFacture.ConsultationId;
            facture.Ref = checkFacture.Ref;

            return View(facture);
        }

        //
        // GET: /Facture/Details/Id
        public ActionResult Details(int id = -1)
        {
            // if user not logged in, go to login page
            if (!UserHelper.isLoggedIn()) return RedirectToAction("Login", "User");

            Facture facture = dbContext.Factures.SingleOrDefault(f => f.Id == id);
            if (facture == null)
            {
                return HttpNotFound();
            }
            else
            {
                // check if user has access to requested informations
                if (!UserHelper.isDentist() && !UserHelper.hasAccessTo(facture.Consultation.PatientForm.UserId))
                {
                    return RedirectToAction("Profil", "User", new { id = UserHelper.getId() });
                }
            }

            return View(facture);
        }

        //
        // GET: /Facture/Delete/Id
        public ActionResult Delete(int id = -1)
        {
            // if user not logged in, go to login page
            if (!UserHelper.isLoggedIn()) return RedirectToAction("Login", "User");

            Facture facture = dbContext.Factures.SingleOrDefault(f => f.Id == id);
            if (facture == null)
            {
                return HttpNotFound();
            }
            else
            {
                // check if user has access to requested informations
                if (!UserHelper.isAdmin() && !UserHelper.isDentist(true))
                {
                    return RedirectToAction("Profil", "User", new { id = UserHelper.getId() });
                }
            }

            return View(facture);
        }

        //
        // POST: /Facture/Delete/Id
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id) // action function name changed to bypass build error
        {
            Facture checkFacture = dbContext.Factures.SingleOrDefault(f => f.Id == id);

            if (checkFacture == null)
            {
                return HttpNotFound();
            }
            else
            {
                dbContext.Factures.Remove(checkFacture);
                dbContext.SaveChanges();
                Session["Message"] = "<b>" + checkFacture.Ref + "</b> a été supprimée !";
                return RedirectToAction("Index");
            }
        }
	}
}