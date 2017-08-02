using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dental_Care.Models;
using Dental_Care.Helpers;

namespace Dental_Care.Controllers
{
    public class HomeController : Controller
    {
        // attr.
        private DentalCareDbContext dbContext = new DentalCareDbContext();

        // Actions

        //
        // GET: /Home/Index
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Home/Contact
        public ActionResult Contact()
        {
            return View();
        }

        //
        // POST: /Home/Contact
        [HttpPost]
        public ActionResult Contact(Contact contact)
        {
            if (ModelState.IsValid) // all model fields are ok
            {
                try
                {
                    // set date
                    contact.Date = DateTime.Now;
                    // add Contact Message
                    dbContext.Contacts.Add(contact);
                    dbContext.SaveChanges();
                    Session["Message"] = "<strong>Merci</strong> . Votre message a été envoyé.";
                    //ModelState.Clear(); // delete form data after submit
                    return RedirectToAction("Contact"); // by this way request data are cleared
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
        // GET : /Home/About
        public ActionResult About()
        {
            return View();
        }
	}
}