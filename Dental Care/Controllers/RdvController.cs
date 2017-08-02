using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dental_Care.Models;
using Dental_Care.Helpers;

namespace Dental_Care.Controllers
{
    public class RdvController : Controller
    {
        // attr.
        private DentalCareDbContext dbContext = new DentalCareDbContext();

        // Actions

        //
        // GET: /Rdv/Index
        public ActionResult Index()
        {
            // if user not logged in, go to login page
            if (!UserHelper.isLoggedIn()) return RedirectToAction("Login", "User");

            if (UserHelper.isAdmin() || UserHelper.isDentist(true))
            {
                List<Rdv> rdvs = dbContext.Rdvs.ToList();
                return View(rdvs);
            }
            else
            {
                return RedirectToAction("Profil", "User", new { id = UserHelper.getId() });
            }
        }

        //
        // GET: /Rdv/Create
        public ActionResult Create()
        {
            // if user not logged in, go to login page
            if (!UserHelper.isLoggedIn())
            {
                Session["Warning"] = "Vous devez vous connecter pour prendre un rendez-vous.";
                return RedirectToAction("Login", "User");
            }
            else if (UserHelper.isDentist())
            {
                Session["Warning"] = "Cet espace est réservé aux patients.";
                return RedirectToAction("Index");
            }

            return View();
        }

        //
        // POST: /Rdv/Create
        [HttpPost]
        public ActionResult Create(Rdv rdv)
        {
            // if user not logged in, go to login page
            // double check (at post too) because we have a form on home page who use this post action only
            if (!UserHelper.isLoggedIn())
            {
                Session["Warning"] = "Vous devez vous connecter pour prendre un rendez-vous.";
                return RedirectToAction("Login", "User");
            }

            if (ModelState.IsValid) // all model fields are ok
            {
                try
                {
                    // generate appointment number
                    List<Rdv> rdvs = dbContext.Rdvs.ToList();
                    int count = rdvs.Count;
                    rdv.Ref = "RDV-" + DateTime.Now.ToString("ddMM") + "-" + ++count;
                    // associate to current user
                    int currentUserId = UserHelper.getId();
                    rdv.UserId = currentUserId;
                    // set state
                    rdv.State = RdvState.Default;
                    // add Rdv
                    dbContext.Rdvs.Add(rdv);
                    dbContext.SaveChanges();
                    Session["Message"] = "Votre rendez-vous a été créé !";
                    return RedirectToAction("Profil", "User", new { id = currentUserId });
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

            return View();
        }

        //
        // GET: /Rdv/Details/Id
        public ActionResult Details(int id = -1)
        {
            // if user not logged in, go to login page
            if (!UserHelper.isLoggedIn()) return RedirectToAction("Login", "User");

            Rdv rdv = dbContext.Rdvs.SingleOrDefault(r => r.Id == id);
            if (rdv == null)
            {
                return HttpNotFound();
            }
            else
            {
                // check if user has access to requested informations
                if (!RdvHelper.hasAccessTo(rdv.UserId, rdv.Ref, RdvState.Default))//, rdv.State)) // force access to details even if canceled or whatever
                {
                    return RedirectToAction("Profil", "User", new { id = UserHelper.getId() });
                }
            }

            return View(rdv);
        }

        //
        // GET: /Rdv/Edit/Id
        public ActionResult Edit(int id = -1)
        {
            // if user not logged in, go to login page
            if (!UserHelper.isLoggedIn()) return RedirectToAction("Login", "User");

            Rdv rdv = dbContext.Rdvs.SingleOrDefault(r => r.Id == id);
            if (rdv == null)
            {
                return HttpNotFound();
            }
            else
            {
                // check if user has access to requested informations
                if (UserHelper.isDentist())
                {
                    Session["Warning"] = "Accès non autorisé!";
                    return RedirectToAction("Index");
                }
                else if (!RdvHelper.hasAccessTo(rdv.UserId, rdv.Ref, rdv.State))
                {
                    return RedirectToAction("Profil", "User", new { id = UserHelper.getId() });
                }
            }

            return View(rdv);
        }

        //
        // POST: /Rdv/Edit/Id
        [HttpPost]
        public ActionResult Edit(Rdv rdv)
        {
            Rdv checkRdv = dbContext.Rdvs.SingleOrDefault(r => r.Id == rdv.Id);
            if (checkRdv == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                checkRdv.Name = rdv.Name;
                checkRdv.Sexe = rdv.Sexe;
                checkRdv.Tel = rdv.Tel;
                checkRdv.Email = rdv.Email;
                checkRdv.Message = rdv.Message;
                checkRdv.Date = rdv.Date;
                dbContext.SaveChanges();
                Session["Message"] = "Modification effectuée!";
                return RedirectToAction("Profil", "User", new { id = checkRdv.User.Id });
            }
            else
            {
                ViewBag.Warning = "Veuillez remplir tout les champs.";
            }

            return View(rdv);
        }

        //
        // GET: /Rdv/Delete/Id
        public ActionResult Delete(int id = -1)
        {
            // if user not logged in, go to login page
            if (!UserHelper.isLoggedIn()) return RedirectToAction("Login", "User");

            Rdv rdv = dbContext.Rdvs.SingleOrDefault(r => r.Id == id);
            if (rdv == null)
            {
                return HttpNotFound();
            }
            else
            {
                // check if user has access to requested informations
                if (!UserHelper.isAdmin(true))
                {
                    return UserHelper.isDentist() ? RedirectToAction("Index") : RedirectToAction("Profil", "User", new { id = UserHelper.getId() });
                }
            }

            return View(rdv);
        }

        //
        // POST: /Rdv/Delete/Id
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id) // action function name changed to bypass build error
        {
            Rdv checkRdv = dbContext.Rdvs.SingleOrDefault(r => r.Id == id);

            if (checkRdv == null)
            {
                return HttpNotFound();
            }
            else
            {
                dbContext.Rdvs.Remove(checkRdv);
                dbContext.SaveChanges();
                Session["Message"] = "<b>" + checkRdv.Ref + "</b> a été supprimé !";
                return RedirectToAction("Index");
            }
        }

        //
        // GET: /Rdv/Cancel/Id
        public ActionResult Cancel(int id = -1)
        {
            // if user not logged in, go to login page
            if (!UserHelper.isLoggedIn()) return RedirectToAction("Login", "User");

            Rdv rdv = dbContext.Rdvs.SingleOrDefault(r => r.Id == id);
            if (rdv == null)
            {
                return HttpNotFound();
            }
            else
            {
                // check if user has access to requested informations
                if (!RdvHelper.hasAccessTo(rdv.UserId, rdv.Ref, rdv.State))
                {
                    return RedirectToAction("Profil", "User", new { id = UserHelper.getId() });
                }
            }

            return View(rdv);
        }

        //
        // POST: /Rdv/Cancel/Id
        [HttpPost, ActionName("Cancel")]
        public ActionResult CancelConfirmed(int id) // action function name changed to bypass build error
        {
            Rdv checkRdv = dbContext.Rdvs.SingleOrDefault(r => r.Id == id);

            if (checkRdv == null)
            {
                return HttpNotFound();
            }
            else
            {
                checkRdv.State = RdvState.Canceled;
                dbContext.SaveChanges();
                Session["Message"] = "<b>" + checkRdv.Ref + "</b> a été annulé !";
                return UserHelper.isDentist() ? RedirectToAction("Index") : RedirectToAction("Profil", "User", new { id = UserHelper.getId() });
            }
        }

        //
        // GET: /Rdv/Confirm/Id
        public ActionResult Confirm(int id = -1)
        {
            // if user not logged in, go to login page
            if (!UserHelper.isLoggedIn()) return RedirectToAction("Login", "User");

            Rdv rdv = dbContext.Rdvs.SingleOrDefault(r => r.Id == id);
            if (rdv == null)
            {
                return HttpNotFound();
            }
            else
            {
                // check if user has access to requested informations
                if (!RdvHelper.hasAccessTo(rdv.UserId, rdv.Ref, rdv.State))
                {
                    return RedirectToAction("Profil", "User", new { id = UserHelper.getId() });
                }
            }

            return View(rdv);
        }

        //
        // POST: /Rdv/Confirm/Id
        [HttpPost, ActionName("Confirm")]
        public ActionResult ConfirmConfirmed(int id) // action function name changed to bypass build error
        {
            Rdv checkRdv = dbContext.Rdvs.SingleOrDefault(r => r.Id == id);

            if (checkRdv == null)
            {
                return HttpNotFound();
            }
            else
            {
                checkRdv.State = RdvState.Confirmed;
                dbContext.SaveChanges();
                Session["Message"] = "<b>" + checkRdv.Ref + "</b> a été confirmé !";
                return UserHelper.isDentist() ? RedirectToAction("Index") : RedirectToAction("Profil", "User", new { id = UserHelper.getId() });
            }
        }

        //
        // GET: /Rdv/Close/Id
        public ActionResult Close(int id = -1)
        {
            // if user not logged in, go to login page
            if (!UserHelper.isLoggedIn()) return RedirectToAction("Login", "User");

            Rdv rdv = dbContext.Rdvs.SingleOrDefault(r => r.Id == id);
            if (rdv == null)
            {
                return HttpNotFound();
            }
            else
            {
                // check if user has access to requested informations
                if (!RdvHelper.hasAccessTo(rdv.UserId, rdv.Ref, rdv.State))
                {
                    return RedirectToAction("Profil", "User", new { id = UserHelper.getId() });
                }
            }

            return View(rdv);
        }

        //
        // POST: /Rdv/Close/Id
        [HttpPost, ActionName("Close")]
        public ActionResult CloseConfirmed(int id) // action function name changed to bypass build error
        {
            Rdv checkRdv = dbContext.Rdvs.SingleOrDefault(r => r.Id == id);

            if (checkRdv == null)
            {
                return HttpNotFound();
            }
            else
            {
                checkRdv.State = RdvState.Closed;
                dbContext.SaveChanges();
                Session["Message"] = "<b>" + checkRdv.Ref + "</b> a été fermé !";
                return UserHelper.isDentist() ? RedirectToAction("Index") : RedirectToAction("Profil", "User", new { id = UserHelper.getId() });
            }
        }
	}
}