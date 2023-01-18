using stockTrackingSystem.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using stockTrackingSystem.Common;

namespace stockTrackingSystem.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        TRACKING_SYSTEMEntities db = new TRACKING_SYSTEMEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public ActionResult Login(UserInfo p)
        {

            var informations = db.UserInfo.FirstOrDefault(x => x.eMail == p.eMail && x.Password == p.Password);
            if (informations != null)
            {
                FormsAuthentication.SetAuthCookie(informations.eMail, false);
                Session["eMail"] = informations.eMail.ToString();
                Session["Name"] = informations.Name.ToString();
                Session["LastName"] = informations.LastName.ToString();

                return RedirectToAction("Index", "Home");

            }
            else
            {
                ViewBag.warning = "Username or password is incorrect.";
            }
            return View();

        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Register(UserInfo data)
        {
            db.UserInfo.Add(data);
            data.Role = "User";
            db.SaveChanges();
            return RedirectToAction("Login","Account");
        }

        public ActionResult Update()
        {
            var users = (string)Session["eMail"];
            var values = db.UserInfo.FirstOrDefault(x => x.eMail == users);
            return View(values);
        }

        [HttpPost]

        public ActionResult Update(UserInfo data)
        {
            var users = (string)Session[Constant.eMail];
            var user = db.UserInfo.Where(x => x.eMail == users).FirstOrDefault();
            user.Name=data.Name;
            user.LastName = data.LastName;
            user.eMail = data.eMail;
            user.UserName = data.UserName;
            user.Password = data.Password;
            user.PasswordAgain = data.PasswordAgain;

            db.SaveChanges();
            return RedirectToAction("Index","Home");
        }
    }
}