using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dental_Care.Helpers
{
    public class RdvState
    {
        // const.
        public const int Canceled = 0;
        public const int Default = 1;
        public const int Confirmed = 2;
        public const int Closed = 3;

        public static string[] Label = {"Annulé", "En attente", "Confirmé", "Fermé"};

        public static string[] ClassName = { "label rdv-canceled", "label rdv-default", "label rdv-confirmed", "label rdv-closed" };
    }

    public static class RdvHelper
    {
        public static bool hasAccessTo(int userId, string rdvRef, int rdvState) // return if user has or not access to informations of specified user id
        {
            if (rdvState == RdvState.Canceled)
            {
                HttpContext.Current.Session["Warning"] = "<b>" + rdvRef + "</b> est en état annulé!";
                return false;
            }
            else if (!UserHelper.isDentist() && rdvState == RdvState.Confirmed) // Dentist should be able to change confirmed state to closed (but a dentist cannot edit a rdv)
            {
                HttpContext.Current.Session["Warning"] = "<b>" + rdvRef + "</b> est en état confirmé!";
                return false;
            }
            else if (rdvState == RdvState.Closed)
            {
                HttpContext.Current.Session["Warning"] = "<b>" + rdvRef + "</b> est en état fermé!";
                return false;
            }

            return UserHelper.isDentist() || UserHelper.hasAccessTo(userId);
        }
    }
}