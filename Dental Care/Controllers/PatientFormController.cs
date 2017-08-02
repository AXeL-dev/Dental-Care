using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dental_Care.Helpers;
using Dental_Care.Models;
using System.IO;

namespace Dental_Care.Controllers
{
    public class PatientFormController : Controller
    {
        // attr.
        private DentalCareDbContext dbContext = new DentalCareDbContext();
        private string[] imageTypes = {    
                                        "image/gif",
                                        "image/jpeg",
                                        "image/pjpeg",
                                        "image/png"
                                    };

        // Méthodes
        private List<User> getUsersList()
        {
            return dbContext.Users.ToList();
        }

        // Actions

        //
        // GET: /PatientForm/Index/userId
        public ActionResult Index(int userId = -1)
        {
            // if user not logged in, go to login page
            if (!UserHelper.isLoggedIn()) return RedirectToAction("Login", "User");

            if (UserHelper.isAdmin() || UserHelper.isDentist(true))
            {
                List<PatientForm> patientForms;
                if (userId > 0) // filter by userId
                {
                    patientForms = dbContext.PatientForms.Where(pf => pf.UserId == userId).ToList();
                }
                else
                {
                    patientForms = dbContext.PatientForms.ToList();
                }
                return View(patientForms);
            }
            else
            {
                return RedirectToAction("Profil", "User", new { id = UserHelper.getId() });
            }
        }

        //
        // GET: /PatientForm/Create
        public ActionResult Create()
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

            // get all users & send them to view
            ViewBag.Users = getUsersList();

            return View();
        }

        //
        // POST: /PatientForm/Create
        [HttpPost]
        public ActionResult Create(PatientForm pf, HttpPostedFileBase ImageUpload)
        {
            if (ImageUpload == null || ImageUpload.ContentLength == 0)
            {
                Session["Warning"] = "Veuillez choisir une image pour les dents !";
            }
            else if (!imageTypes.Contains(ImageUpload.ContentType))
            {
                Session["Warning"] = "Merci de choisir une image au format JPG, PNG ou GIF !";
            }
            else if (ModelState.IsValid) // all model fields are ok
            {
                try
                {
                    // Save image to database (base 64)
                    using (var binaryReader = new BinaryReader(ImageUpload.InputStream))
                        pf.Dents.Image = binaryReader.ReadBytes(ImageUpload.ContentLength);
                    // set date
                    pf.CreationDate = DateTime.Now;
                    // add Patient Form
                    dbContext.PatientForms.Add(pf);
                    dbContext.SaveChanges();
                    Session["Message"] = "Fiche patient créée !";
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

            // get all users & send them to view
            ViewBag.Users = getUsersList();

            return View();
        }

        //
        // GET: /PatientForm/Edit/Id
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

            PatientForm pf = dbContext.PatientForms.SingleOrDefault(f => f.Id == id);
            if (pf == null)
            {
                return HttpNotFound();
            }

            // get all users & send them to view
            ViewBag.Users = getUsersList();

            return View(pf);
        }

        //
        // POST: /PatientForm/Edit/Id
        [HttpPost]
        public ActionResult Edit(PatientForm pf, HttpPostedFileBase ImageUpload)
        {
            PatientForm checkPf = dbContext.PatientForms.SingleOrDefault(f => f.Id == pf.Id);
            if (checkPf == null)
            {
                return HttpNotFound();
            }

            if (ImageUpload != null && ImageUpload.ContentLength != 0 && !imageTypes.Contains(ImageUpload.ContentType))
            {
                Session["Warning"] = "Merci de choisir une image au format JPG, PNG ou GIF !";
            }
            else if (ModelState.IsValid)
            {
                checkPf.Name = pf.Name;
                checkPf.Sexe = pf.Sexe;
                checkPf.Tel = pf.Tel;
                checkPf.Age = pf.Age;
                // Save image to database (base 64)
                if (ImageUpload != null && ImageUpload.ContentLength != 0)
                {
                    using (var binaryReader = new BinaryReader(ImageUpload.InputStream))
                        checkPf.Dents.Image = binaryReader.ReadBytes(ImageUpload.ContentLength);
                }
                checkPf.Dents.Note = pf.Dents.Note;
                checkPf.UserId = pf.UserId;
                dbContext.SaveChanges();
                Session["Message"] = "Modification effectuée!";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Warning = "Veuillez remplir tout les champs.";
            }

            // reset image or it will be null (because she's not auto posted)
            pf.Dents.Image = checkPf.Dents.Image;

            // get all users & send them to view
            ViewBag.Users = getUsersList();

            return View(pf);
        }

        //
        // GET: /PatientForm/Details/Id
        public ActionResult Details(int id = -1)
        {
            // if user not logged in, go to login page
            if (!UserHelper.isLoggedIn()) return RedirectToAction("Login", "User");

            PatientForm pf = dbContext.PatientForms.SingleOrDefault(f => f.Id == id);
            if (pf == null)
            {
                return HttpNotFound();
            }
            else
            {
                // check if user has access to requested informations
                if (!UserHelper.isDentist() && !UserHelper.hasAccessTo(pf.UserId))
                {
                    return RedirectToAction("Profil", "User", new { id = UserHelper.getId() });
                }
            }

            return View(pf);
        }

        //
        // GET: /PatientForm/Delete/Id
        public ActionResult Delete(int id = -1)
        {
            // if user not logged in, go to login page
            if (!UserHelper.isLoggedIn()) return RedirectToAction("Login", "User");

            PatientForm pf = dbContext.PatientForms.SingleOrDefault(f => f.Id == id);
            if (pf == null)
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

            return View(pf);
        }

        //
        // POST: /PatientForm/Delete/Id
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id) // action function name changed to bypass build error
        {
            PatientForm checkPf = dbContext.PatientForms.SingleOrDefault(f => f.Id == id);

            if (checkPf == null)
            {
                return HttpNotFound();
            }
            else
            {
                Dents dents = checkPf.Dents;
                dbContext.PatientForms.Remove(checkPf);
                dbContext.Dents.Remove(dents); // don't forget to remove dents
                dbContext.SaveChanges();
                Session["Message"] = "Fiche patient supprimée !";
                return RedirectToAction("Index");
            }
        }
	}
}