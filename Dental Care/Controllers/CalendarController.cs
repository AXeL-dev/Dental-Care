using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dental_Care.Helpers;
using Dental_Care.Models;

namespace Dental_Care.Controllers
{
    public class CalendarController : Controller
    {
        // attr.
        private DentalCareDbContext dbContext = new DentalCareDbContext();

        // Actions

        //
        // GET: /Calendar/
        public ActionResult Index()
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

            return View();
        }

        //
        // GET: /Calendar/GetEvents
        public JsonResult GetEvents()
        {
            // if user not logged in, go to login page
            if (!UserHelper.isLoggedIn())
            {
                return null;
            }
            else if (!UserHelper.isAdmin() && !UserHelper.isDentist(true))
            {
                return null;
            }

            // get rdvs
            List<Rdv> rdvs = dbContext.Rdvs.ToList();
            var rdvList = from r in rdvs
                            select new
                            {
                                id = r.Id,
                                title = r.Ref,
                                start = r.Date.ToString("s"),
                                //end = r.Date.ToString("s"),
                                url = Url.Action("Details", "Rdv", new { id = r.Id }),
                                className = RdvState.ClassName[r.State],
                                tooltip = RdvState.Label[r.State],
                                allDay = false
                            };

            var rows = rdvList.ToArray();
            return Json(rows, JsonRequestBehavior.AllowGet);
        }
	}
}