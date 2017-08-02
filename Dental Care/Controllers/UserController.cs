using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dental_Care.Models;
using System.Web.Helpers;
using System.Web.Security;
using Dental_Care.Helpers;

namespace Dental_Care.Controllers
{
    public class UserController : Controller
    {
        // attr.
        private DentalCareDbContext dbContext = new DentalCareDbContext();

        // Actions

        //
        // GET: /User/Index
        public ActionResult Index()
        {
            // if user not logged in, go to login page
            if (!UserHelper.isLoggedIn()) return RedirectToAction("Login");

            if (UserHelper.isAdmin(true))
            {
                List<User> users = dbContext.Users.ToList();
                return View(users);
            }
            else
            {
                return RedirectToAction("Profil", "User", new { id = UserHelper.getId() });
            }
        }

        //
        // GET: /User/Profil/Id
        public ActionResult Profil(int id = -1)
        {
            //if (id <= 0) return RedirectToAction("Index", "Home");

            // if user not logged in, go to login page
            if (!UserHelper.isLoggedIn()) return RedirectToAction("Login");

            User user = dbContext.Users.SingleOrDefault(u => u.Id == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            else
            {
                // check if user has access to requested informations
                if (!UserHelper.hasAccessTo(id))
                {
                    return RedirectToAction("Profil", "User", new { id = UserHelper.getId() });
                }
            }

            return View(user);
        }

        //
        // GET: /User/Edit/Id
        public ActionResult Edit(int id = -1)
        {
            //if (id <= 0) return RedirectToAction("Index", "Home");

            // if user not logged in, go to login page
            if (!UserHelper.isLoggedIn()) return RedirectToAction("Login");

            User user = dbContext.Users.SingleOrDefault(u => u.Id == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            else
            {
                // check if user has access to requested informations
                if (!UserHelper.hasAccessTo(id))
                {
                    return RedirectToAction("Profil", "User", new { id = UserHelper.getId() });
                }
            }

            return View(user);
        }

        //
        // POST: /User/Edit/Id
        [HttpPost]
        public ActionResult Edit(User user)
        {
            User checkUser = dbContext.Users.SingleOrDefault(u => u.Id == user.Id);
            if (checkUser == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                // don't forget to check if email is unique
                User checkEmail = dbContext.Users.SingleOrDefault(u => u.Id != checkUser.Id && u.Email == user.Email);
                if (checkEmail != null)
                {
                    ViewBag.Error = "Cet email est déjà utilisé.";
                    //user.Email = checkUser.Email;
                }
                else
                {
                    checkUser.Username = user.Username;
                    checkUser.Email = user.Email;
                    checkUser.Pwd = Crypto.Hash(user.Pwd); // hash the new password
                    checkUser.Tel = user.Tel;
                    checkUser.Address = user.Address;
                    // only admin have right to change isAdmin & isDentist attribute
                    if (UserHelper.isAdmin())
                    {
                        checkUser.isAdmin = user.isAdmin;
                        checkUser.isDentist = user.isDentist;
                    }
                    // if current user has change his informations, apply to session vars too
                    bool shouldUpdateSession = UserHelper.isCurrentSessionUser(user.Id);
                    if (shouldUpdateSession)
                    {
                        Session["Username"] = checkUser.Username;
                        Session["isAdmin"] = checkUser.isAdmin;
                        Session["isDentist"] = checkUser.isDentist;
                    }

                    dbContext.SaveChanges();
                    Session["Message"] = "Modification effectuée!";
                    return RedirectToAction("Profil", "User", new { id = checkUser.Id });
                }
            }
            else
            {
                ViewBag.Warning = "Veuillez remplir tout les champs.";
            }
            
            return View(user);
        }

        //
        // GET: /User/Delete/Id
        public ActionResult Delete(int id = -1)
        {
            //if (id <= 0) return RedirectToAction("Index", "Home");

            // if user not logged in, go to login page
            if (!UserHelper.isLoggedIn()) return RedirectToAction("Login");

            User user = dbContext.Users.SingleOrDefault(u => u.Id == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            else
            {
                // check if user has access to requested informations
                if (!UserHelper.isAdmin(true))//!hasAccessTo(id))
                {
                    return RedirectToAction("Profil", "User", new { id = UserHelper.getId() });
                }
            }

            return View(user);
        }

        //
        // POST: /User/Delete/Id
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id) // action function name changed to bypass build error
        {
            User checkUser = dbContext.Users.SingleOrDefault(u => u.Id == id);

            if (checkUser == null)
            {
                return HttpNotFound();
            }
            else
            {
                dbContext.Users.Remove(checkUser);
                dbContext.SaveChanges();
                // if current user has been deleted, we clear session
                bool shouldClearSession = UserHelper.isCurrentSessionUser(checkUser.Id);
                if (shouldClearSession)
                {
                    Session.Clear();
                }
                Session["Message"] = "<b>" + checkUser.Username + "</b> a été supprimé !";
                return RedirectToAction("Index");
            }
        }

        //
        // GET: /User/Login
        public ActionResult Login()
        {
            // if user already logged in, go to home page
            if (UserHelper.isLoggedIn()) return RedirectToAction("Index", "Home");
            
            return View();
        }

        //
        // POST: /User/Login
        [HttpPost]
        public ActionResult Login(User user)
        {
            string hashedPwd = Crypto.Hash(user.Pwd);
            User checkUser = dbContext.Users.SingleOrDefault(u => u.Email == user.Email && u.Pwd == hashedPwd);

            if (checkUser != null) // if user check is okay, set session vars & go to home page
            {
                Session["Id"] = checkUser.Id;
                Session["Username"] = checkUser.Username;
                if (checkUser.isAdmin) Session["isAdmin"] = checkUser.isAdmin;
                else if (checkUser.isDentist) Session["isDentist"] = checkUser.isDentist;
                FormsAuthentication.SetAuthCookie(user.Email, false);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Error = "Email ou mot de passe incorrect !";
                return View();
            }
        }

        //
        // GET: /User/Logout
        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        //
        // GET: /User/Register
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /User/Register
        [HttpPost]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid) // all model fields are ok
            {
                try
                {
                    // first we check if email is unique
                    User checkEmail = dbContext.Users.SingleOrDefault(u => u.Email == user.Email);
                    if (checkEmail != null)
                    {
                        ViewBag.Error = "Cet email est déjà utilisé.";
                    }
                    else // if it's okay
                    {
                        user.Pwd = Crypto.Hash(user.Pwd); // hash the password
                        dbContext.Users.Add(user);
                        dbContext.SaveChanges();
                        Session["Message"] = "Vous êtes maintenant inscrit ! Vous pouvez vous connecter.";
                        return RedirectToAction("Login");
                    }
                }
                catch(Exception ex)
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
	}
}