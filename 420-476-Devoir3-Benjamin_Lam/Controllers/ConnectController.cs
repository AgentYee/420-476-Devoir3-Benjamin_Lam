using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _420_476_Devoir3_Benjamin_Lam.Controllers
{
    public class ConnectController : Controller
    {
        private NorthwindEntities db = new NorthwindEntities();
        // GET: Connect
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string login, string password)
        {
            foreach (User u in db.Users)
            {
                if ((u.Login == login) && (u.Password == password))
                {
                    Session["ConnectedUser"] = u.FirstName + " " + u.LastName;
                    return RedirectToAction("Index", "Products");
                }
            }
            ViewBag.LoginValidation = "fail";
            return View();
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login");
        }
    }
}