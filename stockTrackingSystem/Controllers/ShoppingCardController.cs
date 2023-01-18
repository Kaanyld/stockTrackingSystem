using Microsoft.VisualBasic.ApplicationServices;
using stockTrackingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace stockTrackingSystem.Controllers
{
    public class ShoppingCardController : Controller
    {
        // GET: ShoppingCard
        TRACKING_SYSTEMEntities db = new TRACKING_SYSTEMEntities();

        public ActionResult Index(decimal? Amount)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userName = User.Identity.Name;
                var user = db.UserInfo.FirstOrDefault(x => x.eMail == userName);
                var model = db.ShoppingCard.Where(x => x.UserInfoId == user.Id).ToList();
                var uid = db.ShoppingCard.FirstOrDefault(x => x.UserInfoId == user.Id);

                //If the user has not added a product to the shopping card.
                if (model != null)
                {
                    if (uid == null)
                    {
                        ViewBag.Amount = "There are no products in your card.";
                    }
                    else if (uid != null)
                    {
                        Amount = db.ShoppingCard.Where(x => x.UserInfoId == uid.UserInfoId).Sum(x => x.Products.Price * x.Piece);
                        ViewBag.Amount = "Total Amount= $" + Amount;
                    }
                    return View(model);
                }
            }
            return HttpNotFound();
        }


        public ActionResult AddShoppingCard(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userName = User.Identity.Name;
                var model = db.UserInfo.FirstOrDefault(x => x.eMail == userName);
                var p = db.Products.Find(id);
                var shoppingcard = db.ShoppingCard.FirstOrDefault(x => x.UserInfoId == model.Id && x.ProductsId == id);
                if (model != null)
                {
                    if (shoppingcard != null)
                    {
                        shoppingcard.Piece++;
                        shoppingcard.Price = p.Price * shoppingcard.Piece;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    var s = new ShoppingCard
                    {
                        UserInfoId = model.Id,
                        ProductsId = p.Id,
                        Piece = 1,
                        Price = p.Price,
                        Date = DateTime.Now
                    };
                    db.ShoppingCard.Add(s);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View();
            }
            return HttpNotFound();

        }
        public ActionResult ShoppingCardCount(int? count)
        {
            if (User.Identity.IsAuthenticated)
            {
                var model = db.UserInfo.FirstOrDefault(x => x.eMail == User.Identity.Name);
                count = db.ShoppingCard.Where(x => x.UserInfoId == model.Id).Count();
                ViewBag.count = count;
                if (count == 0)
                {
                    ViewBag.count = "";
                }
                return PartialView();

            }
            return HttpNotFound();
        }

        public ActionResult increase(int id)
        {
            var model = db.ShoppingCard.Find(id);
            model.Piece++;
            model.Price = model.Price * model.Piece;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult decrease(int id)
        {
            var model = db.ShoppingCard.Find(id);
            if(model.Piece == 1)
            {
                db.ShoppingCard.Remove(model);
                db.SaveChanges();
            }
            model.Piece--;
            model.Price = model.Price * model.Piece;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public void writePiece(int id, int quantity)
        {
            var model = db.ShoppingCard.Find(id);
            model.Piece = quantity;
            model.Price=model.Price* model.Piece;
            db.SaveChanges();
        }

        public ActionResult Delete (int id)
        {
            var delete=db.ShoppingCard.Find(id);
            db.ShoppingCard.Remove(delete);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DeleteAll()
        {
            if(User.Identity.IsAuthenticated)
            {
                var username=User.Identity.Name;
                var model = db.UserInfo.FirstOrDefault(x => x.eMail == username);
                var delete = db.ShoppingCard.Where(x => x.UserInfoId == model.Id);
                db.ShoppingCard.RemoveRange(delete);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return HttpNotFound();
           
        }


    }

}
