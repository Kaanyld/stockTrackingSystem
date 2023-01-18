using stockTrackingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace stockTrackingSystem.Controllers
{
    public class StatisticsController : Controller
    {
        // GET: Statistics
        TRACKING_SYSTEMEntities db = new TRACKING_SYSTEMEntities();

        public ActionResult Index()
        {
            var sales = db.Sales.Count();
            ViewBag.sales = sales;
            var products = db.Products.Count();
            ViewBag.products = products;
            var categories = db.Categories.Count();
            ViewBag.categories = categories;    
            var shoppingcart = db.ShoppingCard.Count();
            ViewBag.shoppingcart = shoppingcart;
            var userinfo = db.UserInfo.Count();
            ViewBag.userinfo = userinfo;
            return View();
        }
    }
}