using PagedList;
using PagedList.Mvc;
using stockTrackingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace stockTrackingSystem.Controllers
{
    public class AllSalesController : Controller
    {
        // GET: AllSales
        TRACKING_SYSTEMEntities db = new TRACKING_SYSTEMEntities();
       [ Authorize(Roles= "Admin")]
        public ActionResult Index(int page =1)
        {
            return View(db.Sales.ToList().ToPagedList(page,5));
        }
    }
}