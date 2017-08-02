using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dental_Care.Helpers;
using Dental_Care.Models;

namespace Dental_Care.Controllers
{
    public class ContactController : Controller
    {
        // attr.
        private DentalCareDbContext dbContext = new DentalCareDbContext();

        // Actions

        //
        // GET: /Contact/
        public ActionResult Index()
        {
            // if user not logged in, go to login page
            if (!UserHelper.isLoggedIn()) return RedirectToAction("Login", "User");

            if (UserHelper.isAdmin() || UserHelper.isDentist(true))
            {
                List<Contact> contacts = dbContext.Contacts.ToList();
                return View(contacts);
            }
            else
            {
                return RedirectToAction("Profil", "User", new { id = UserHelper.getId() });
            }
        }

        //
        // GET: /Contact/Details/Id
        public ActionResult Details(int id = -1)
        {
            // if user not logged in, go to login page
            if (!UserHelper.isLoggedIn()) return RedirectToAction("Login", "User");

            Contact contact = dbContext.Contacts.SingleOrDefault(c => c.Id == id);
            if (contact == null)
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

            return View(contact);
        }

        //
        // GET: /Contact/Delete/Id
        public ActionResult Delete(int id = -1)
        {
            // if user not logged in, go to login page
            if (!UserHelper.isLoggedIn()) return RedirectToAction("Login", "User");

            Contact contact = dbContext.Contacts.SingleOrDefault(c => c.Id == id);
            if (contact == null)
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

            return View(contact);
        }

        //
        // POST: /Contact/Delete/Id
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id) // action function name changed to bypass build error
        {
            Contact checkContact = dbContext.Contacts.SingleOrDefault(c => c.Id == id);

            if (checkContact == null)
            {
                return HttpNotFound();
            }
            else
            {
                dbContext.Contacts.Remove(checkContact);
                dbContext.SaveChanges();
                Session["Message"] = "Le Contact <b>" + checkContact.Name + "</b> a été supprimé !";
                return RedirectToAction("Index");
            }
        }
	}
}