using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using stockTrackingSystem.Models;

namespace stockTrackingSystem.Controllers
{
    public class UserInfoController : Controller
    {
        // GET: UserInfo
        TRACKING_SYSTEMEntities db = new TRACKING_SYSTEMEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PasswordReset()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PasswordReset(string email)
        {
            var mail = db.UserInfo.Where(x => x.eMail == email).SingleOrDefault();
            if (mail != null)
            {
                Random rnd = new Random();
                int newPassword = rnd.Next();
                UserInfo password = new UserInfo();
                mail.Password = Crypto.Hash(Convert.ToString(newPassword), "MD5");
                db.SaveChanges();
                WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.EnableSsl = true;
                WebMail.UserName = "kaanyldrm98@gmail.com";
                WebMail.Password = "kjutbvsjdcswueek";
                WebMail.SmtpPort = 587;
                WebMail.Send(email, "Login Password", "Password" + newPassword);

            }
            else
            {
            }
            return View();
        }
    }
}