using stockTrackingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace stockTrackingSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        // GET: Category
        TRACKING_SYSTEMEntities db = new TRACKING_SYSTEMEntities();
        
        public ActionResult Index()
        {
            return View(db.Categories.Where(x=>x.Case==true).ToList());
        }

        
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        
        public ActionResult Add(Categories data)
        {
            db.Categories.Add(data);
            data.Case = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
       
        public ActionResult Delete(int id)
        {
            var category = db.Categories.Where(x => x.Id == id).FirstOrDefault();
            db.Categories.Remove(category);
            category.Case = false;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Update(Categories data)
        {
            var update=db.Categories.Where(x => x.Id == data.Id).FirstOrDefault();
            update.Name = data.Name;
            update.Statement = data.Statement;
            db.SaveChanges();
            return View();
            
        }
    }
}