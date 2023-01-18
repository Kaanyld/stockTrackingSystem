using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using stockTrackingSystem.Models;

namespace stockTrackingSystem.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Product
        TRACKING_SYSTEMEntities db = new TRACKING_SYSTEMEntities();
        [Authorize]

        public ActionResult Index(string search)
        {
            var productsList = db.Products.ToList();
            if(!string.IsNullOrEmpty(search))
            {
                productsList = productsList.Where(x => x.Name.Contains(search) || x.Statement.Contains(search)).ToList();
            }
            return View(productsList);
        }

        [Authorize(Roles ="Admin")]
        public ActionResult Add()
        {
            List<SelectListItem> value1 = (from x in db.Categories.ToList()

                                           select new SelectListItem
                                           {
                                               Text = x.Name,
                                               Value = x.Id.ToString()

                                           }).ToList();
            ViewBag.ctgr = value1;

            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Add(Products Data, HttpPostedFileBase File)
        {
            string path = Path.Combine("~/Content/Images" + File.FileName);
            File.SaveAs(Server.MapPath(path));
            Data.Image = File.FileName.ToString();
            db.Products.Add(Data);
            db.SaveChanges();
            return RedirectToAction("Index");

        }

        [Authorize(Roles = "Admin")]

        public ActionResult Delete(int id)
        {
            var product=db.Products.Where(x => x.Id == id).FirstOrDefault();
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]

        public ActionResult Update(int id)
        {
           var update= db.Products.Where(x => x.Id == id).FirstOrDefault();
            List<SelectListItem> value1 = (from x in db.Categories.ToList()

                                           select new SelectListItem
                                           {
                                               Text = x.Name,
                                               Value = x.Id.ToString()

                                           }).ToList();
            ViewBag.ctgr = value1;
            return View(update);
        }
        [Authorize(Roles ="Admin")]
        [HttpPost]

        public ActionResult Update(Products model, HttpPostedFileBase File)
        {
            var product = db.Products.Find(model.Id);

            if(File==null)
            { 
                product.Name = model.Name;
                product.Statement = model.Statement;
                product.Price = model.Price;
                product.Stock = model.Stock;
                product.Popular = model.Popular;
                product.CategoriesId = model.CategoriesId;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                product.Image = File.FileName.ToString();
                product.Name = model.Name;
                product.Statement = model.Statement;
                product.Price = model.Price;
                product.Stock = model.Stock;
                product.Popular = model.Popular;
                product.CategoriesId = model.CategoriesId;

                db.SaveChanges();
                return RedirectToAction("Index");

            }


        }

        [Authorize(Roles ="Admin")]

        public ActionResult CriticalStock()
        {
            var critical=db.Products.Where(x=>x.Stock <=50).ToList();
            return View(critical);
        }
        public PartialViewResult StockCount()
        {
            if (User.Identity.IsAuthenticated)
            {
                var count=db.Products.Where(x=>x.Stock<50).Count();
                ViewBag.count = count;
                var decreasing=db.Products.Where(x=>x.Stock==50).Count();
                ViewBag.decreasing = decreasing;
            }
            return PartialView();
        }
        public ActionResult StockGraphic()
        {
            ArrayList value1 = new ArrayList();
            ArrayList value2 = new ArrayList();
            var dataStockGraphic = db.Products.ToList();
            dataStockGraphic.ToList().ForEach(x=>value1.Add(x.Name));
            dataStockGraphic.ToList().ForEach(x => value2.Add(x.Stock));
            var graphic = new Chart(width: 500, height: 500).AddTitle("Product-Stock Graphic").AddSeries(chartType:"Column", name:"Name", xValue: value1, yValues: value2);
            return File(graphic.ToWebImage().GetBytes(), "image/jpeg");
        }


    }

}