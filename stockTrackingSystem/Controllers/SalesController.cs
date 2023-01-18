using stockTrackingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace stockTrackingSystem.Controllers
{
    public class SalesController : Controller
    {
        // GET: Sales
        TRACKING_SYSTEMEntities db = new TRACKING_SYSTEMEntities();
        [Authorize(Roles = "User")]

        public ActionResult Index(int page = 1)
        {
            if (User.Identity.IsAuthenticated)
            {
                var username = User.Identity.Name;
                var user = db.UserInfo.FirstOrDefault(x => x.eMail == username);
                var model = db.Sales.Where(x => x.UserInfoId == user.Id).ToList().ToPagedList(page, 5);
                return View(model);
            }
            return HttpNotFound();

        }
        public ActionResult Purchase(int id)
        {
            var model = db.ShoppingCard.FirstOrDefault(x => x.Id == id);
            return View(model);
        }
        [HttpPost]

        public ActionResult Purchase_2(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var model = db.ShoppingCard.FirstOrDefault(x => x.Id == id);
                    var sales = new Sales
                    {
                        UserInfoId = model.UserInfoId,
                        ProductsId = model.ProductsId,
                        Piece = model.Piece,
                        Price = model.Price,
                        Image = model.Image,
                        Date = model.Date,
                    };
                    db.ShoppingCard.Remove(model);
                    db.Sales.Add(sales);
                    db.SaveChanges();
                    ViewBag.process = "The purchase  successful.";
                }
            }
            catch (Exception)
            {
                ViewBag.process = "The purchase not successful.";
            }
            return View("process");
        }

        public ActionResult PurchaseAll(decimal? Amount)
        {
            if (User.Identity.IsAuthenticated)
            {
                var username = User.Identity.Name;
                var user = db.UserInfo.FirstOrDefault(x => x.eMail == username);
                var model = db.ShoppingCard.Where(x => x.UserInfoId == user.Id).ToList();
                var uid = db.ShoppingCard.FirstOrDefault(x => x.UserInfoId == user.Id);
                if (model != null)
                {
                    if (uid == null)
                    {
                        ViewBag.Tutar = "There is no product in your shopping card";
                    }
                    else if (uid != null)
                    {
                        Amount = db.ShoppingCard.Where(x => x.UserInfoId == uid.UserInfoId).Sum(x => x.Products.Price * x.Piece);
                        ViewBag.Tutar = "Total Amount = $" + Amount;
                    }
                    return View(model);
                }
                return View();
             }
            return HttpNotFound();
        }



        public ActionResult AllPurchase_2()
        {
            var username = User.Identity.Name;
            var user = db.UserInfo.FirstOrDefault(x => x.eMail == username);
            var model = db.ShoppingCard.Where(x => x.UserInfoId == user.Id).ToList();
            int line = 0;
            foreach (var item in model)
            {
                var sales = new Sales
                {
                    UserInfoId = model[line].UserInfoId,
                    ProductsId = model[line].ProductsId,
                    Piece = model[line].Piece,
                    Price = model[line].Price,
                    Image = model[line].Products.Image,
                    Date = DateTime.Now
                };
                db.Sales.Add(sales);
                db.SaveChanges();
                line++;
            }
            db.ShoppingCard.RemoveRange(model);
            db.SaveChanges();



            return RedirectToAction("Index", "ShoppingCard");
        }

    }
}