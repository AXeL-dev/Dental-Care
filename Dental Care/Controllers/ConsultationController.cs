using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dental_Care.Models;
using Dental_Care.Helpers;

namespace Dental_Care.Controllers
{
    public class ConsultationController : Controller
    {
        // attr.
        private DentalCareDbContext dbContext = new DentalCareDbContext();

        // Actions

        //
        // GET: /Consultation/
        public ActionResult Index(int patientFormId = -1)
        {
            // if user not logged in, go to login page
            if (!UserHelper.isLoggedIn()) return RedirectToAction("Login", "User");

            if (UserHelper.isAdmin() || UserHelper.isDentist(true))
            {
                List<Consultation> consultations;
                if (patientFormId > 0) // filter by patientFormId
                {
                    consultations = dbContext.Consultations.Where(c => c.PatientFormId == patientFormId).ToList();
                }
                else
                {
                    consultations = dbContext.Consultations.ToList();
                }
                return View(consultations);
            }
            else
            {
                return RedirectToAction("Profil", "User", new { id = UserHelper.getId() });
            }
        }

        //
        // GET: /Consultation/Create?patientFormId=
        public ActionResult Create(int patientFormId)
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

            // pass patientFormId to view
            ViewBag.patientFormId = patientFormId;

            return View();
        }

        //
        // POST: /Consultation/Create
        [HttpPost]
        public ActionResult Create(Consultation consultation)
        {
            if (ModelState.IsValid) // all model fields are ok
            {
                try
                {
                    // generate ref
                    List<Consultation> consultations = dbContext.Consultations.ToList();
                    int count = consultations.Count;
                    consultation.Ref = "CO-" + DateTime.Now.ToString("ddMM") + "-" + ++count;
                    // add consultation
                    dbContext.Consultations.Add(consultation);
                    dbContext.SaveChanges();
                    Session["Message"] = "Consultation créée !";
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

            // pass patientFormId to view
            ViewBag.patientFormId = consultation.PatientFormId;

            return View();
        }

        //
        // GET: /Consultation/Edit/Id
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

            Consultation consultation = dbContext.Consultations.SingleOrDefault(c => c.Id == id);
            if (consultation == null)
            {
                return HttpNotFound();
            }

            return View(consultation);
        }

        //
        // POST: /Consultation/Edit/Id
        [HttpPost]
        public ActionResult Edit(Consultation consultation)
        {
            Consultation checkConsultation = dbContext.Consultations.SingleOrDefault(c => c.Id == consultation.Id);
            if (checkConsultation == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                checkConsultation.Date = consultation.Date;
                checkConsultation.Contenu = consultation.Contenu;
                checkConsultation.Antecedant = consultation.Antecedant;
                checkConsultation.Traitement = consultation.Traitement;
                dbContext.SaveChanges();
                Session["Message"] = "Modification effectuée!";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Warning = "Veuillez remplir tout les champs.";
            }

            // set PatientFormId & Ref (because they are not posted)
            consultation.PatientFormId = checkConsultation.PatientFormId;
            consultation.Ref = checkConsultation.Ref;

            return View(consultation);
        }

        //
        // GET: /Consultation/Details/Id
        public ActionResult Details(int id = -1)
        {
            // if user not logged in, go to login page
            if (!UserHelper.isLoggedIn()) return RedirectToAction("Login", "User");

            Consultation consultation = dbContext.Consultations.SingleOrDefault(c => c.Id == id);
            if (consultation == null)
            {
                return HttpNotFound();
            }
            else
            {
                // check if user has access to requested informations
                if (!UserHelper.isDentist() && !UserHelper.hasAccessTo(consultation.PatientForm.UserId))
                {
                    return RedirectToAction("Profil", "User", new { id = UserHelper.getId() });
                }
            }

            return View(consultation);
        }

        //
        // GET: /Consultation/Delete/Id
        public ActionResult Delete(int id = -1)
        {
            // if user not logged in, go to login page
            if (!UserHelper.isLoggedIn()) return RedirectToAction("Login", "User");

            Consultation consultation = dbContext.Consultations.SingleOrDefault(c => c.Id == id);
            if (consultation == null)
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

            return View(consultation);
        }

        //
        // POST: /Consultation/Delete/Id
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id) // action function name changed to bypass build error
        {
            Consultation checkConsultation = dbContext.Consultations.SingleOrDefault(c => c.Id == id);

            if (checkConsultation == null)
            {
                return HttpNotFound();
            }
            else
            {
                dbContext.Consultations.Remove(checkConsultation);
                dbContext.SaveChanges();
                Session["Message"] = "<b>" + checkConsultation.Ref + "</b> a été supprimée !";
                return RedirectToAction("Index");
            }
        }
	}
}