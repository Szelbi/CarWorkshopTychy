using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarWorkshop.Database;

namespace CarWorkshop.Controllers
{
    public class HomeController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        private ProductRepository _pr;
        public HomeController()
        {
            this._pr = new ProductRepository(db);
        }

        public ActionResult Index()
        {
            var products = _pr.List();
            products.Reverse();
            return View(products.Take(7).ToList());
        }

        public ActionResult TermsAndConditions()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Informacje o sklepie";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Strona kontaktowa sklepu CarWorkshop";

            return View();
        }


        public ActionResult Search(string search)
        {
            List<Product> searchResult = _pr.Search(search);
            ViewData["search"] = true;

            return View("Index", searchResult);
        }
    }
}