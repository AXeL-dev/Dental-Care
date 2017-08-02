using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dental_Care.Models;
using Dental_Care.Helpers;

namespace Dental_Care.Controllers
{
    public class OrdonanceController : Controller
    {
        // attr.
        private DentalCareDbContext dbContext = new DentalCareDbContext();

        // Actions

        //
        // GET: /Ordonance/
        public ActionResult Index()
        {
            // if user not logged in, go to login page
            if (!UserHelper.isLoggedIn()) return RedirectToAction("Login", "User");

            if (UserHelper.isAdmin() || UserHelper.isDentist(true))
            {
                List<Ordonance> ordonances = dbContext.Ordonances.ToList();
                return View(ordonances);
            }
            else
            {
                return RedirectToAction("Profil", "User", new { id = UserHelper.getId() });
            }
        }

        //
        // GET: /Ordonance/Create?consultationId=
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
        // POST: /Ordonance/Create
        [HttpPost]
        public ActionResult Create(Ordonance ordonance)
        {
            if (ModelState.IsValid) // all model fields are ok
            {
                try
                {
                    // generate ref
                    List<Ordonance> ordonances = dbContext.Ordonances.ToList();
                    int count = ordonances.Count;
                    ordonance.Ref = "OR-" + DateTime.Now.ToString("ddMM") + "-" + ++count;
                    // add consultation
                    dbContext.Ordonances.Add(ordonance);
                    dbContext.SaveChanges();
                    Session["Message"] = "Ordonance créée !";
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
            ViewBag.consultationId = ordonance.ConsultationId;

            return View();
        }

        //
        // GET: /Ordonance/Edit/Id
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

            Ordonance ordonance = dbContext.Ordonances.SingleOrDefault(o => o.Id == id);
            if (ordonance == null)
            {
                return HttpNotFound();
            }

            return View(ordonance);
        }

        //
        // POST: /Ordonance/Edit/Id
        [HttpPost]
        public ActionResult Edit(Ordonance ordonance)
        {
            Ordonance checkOrdonance = dbContext.Ordonances.SingleOrDefault(o => o.Id == ordonance.Id);
            if (checkOrdonance == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                checkOrdonance.Date = ordonance.Date;
                checkOrdonance.Contenu = ordonance.Contenu;
                dbContext.SaveChanges();
                Session["Message"] = "Modification effectuée!";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Warning = "Veuillez remplir tout les champs.";
            }

            // set ConsultationId & Ref (because they are not posted)
            ordonance.ConsultationId = checkOrdonance.ConsultationId;
            ordonance.Ref = checkOrdonance.Ref;

            return View(ordonance);
        }

        //
        // GET: /Ordonance/Details/Id
        public ActionResult Details(int id = -1)
        {
            // if user not logged in, go to login page
            if (!UserHelper.isLoggedIn()) return RedirectToAction("Login", "User");

            Ordonance ordonance = dbContext.Ordonances.SingleOrDefault(o => o.Id == id);
            if (ordonance == null)
            {
                return HttpNotFound();
            }
            else
            {
                // check if user has access to requested informations
                if (!UserHelper.isDentist() && !UserHelper.hasAccessTo(ordonance.Consultation.PatientForm.UserId))
                {
                    return RedirectToAction("Profil", "User", new { id = UserHelper.getId() });
                }
            }

            return View(ordonance);
        }

        //
        // GET: /Ordonance/Delete/Id
        public ActionResult Delete(int id = -1)
        {
            // if user not logged in, go to login page
            if (!UserHelper.isLoggedIn()) return RedirectToAction("Login", "User");

            Ordonance ordonance = dbContext.Ordonances.SingleOrDefault(o => o.Id == id);
            if (ordonance == null)
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

            return View(ordonance);
        }

        //
        // POST: /Ordonance/Delete/Id
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id) // action function name changed to bypass build error
        {
            Ordonance checkOrdonance = dbContext.Ordonances.SingleOrDefault(o => o.Id == id);

            if (checkOrdonance == null)
            {
                return HttpNotFound();
            }
            else
            {
                dbContext.Ordonances.Remove(checkOrdonance);
                dbContext.SaveChanges();
                Session["Message"] = "<b>" + checkOrdonance.Ref + "</b> a été supprimée !";
                return RedirectToAction("Index");
            }
        }
	}
}