using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dental_Care.Helpers
{
    public static class UserHelper
    {
        // Méthodes statiques
        public static bool isLoggedIn()
        {
            return HttpContext.Current.Session["Id"] != null && HttpContext.Current.Session["Username"] != null;
        }

        public static bool isAdmin(bool showSessionWarning = false)
        {
            bool yes = HttpContext.Current.Session["isAdmin"] != null && (bool)HttpContext.Current.Session["isAdmin"] == true;
            if (!yes && showSessionWarning)
            {
                HttpContext.Current.Session["Warning"] = "Accès non autorisé!";
            }
            return yes;
        }

        public static bool isDentist(bool showSessionWarning = false)
        {
            bool yes = HttpContext.Current.Session["isDentist"] != null && (bool)HttpContext.Current.Session["isDentist"] == true;
            if (!yes && showSessionWarning)
            {
                HttpContext.Current.Session["Warning"] = "Accès non autorisé!";
            }
            return yes;
        }

        public static bool isCurrentSessionUser(int userId)
        {
            return HttpContext.Current.Session["Id"] != null && (int)HttpContext.Current.Session["Id"] == userId;
        }

        public static bool hasAccessTo(int id) // return if user has or not access to informations of specified user id
        {
            bool ok = isCurrentSessionUser(id) || isAdmin();
            if (!ok)
            {
                HttpContext.Current.Session["Warning"] = "Accès non autorisé!";
            }
            return ok;
        }

        public static int getId()
        {
            return HttpContext.Current.Session["Id"] != null ? (int)HttpContext.Current.Session["Id"] : -1;
        }
    }
}