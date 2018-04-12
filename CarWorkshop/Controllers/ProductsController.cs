using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarWorkshop.Database;
using Newtonsoft.Json;

namespace CarWorkshop.Controllers
{
    public class ProductsController : Controller
    {
        DatabaseContext db = new DatabaseContext();
        ProductRepository _productRepository;

        public ProductsController()
        {
            _productRepository = new ProductRepository(db);
        }

        // GET: Products
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Category(int id)
        {
            List<Product> list = new List<Product>();
            if(id == 0)
            {
                return RedirectToAction("Index", "Home");
            } else
            {
                TempData["categoryId"] = id;
                list = _productRepository.ListCategory(id);
                return View("~/Views/Home/Index.cshtml", list);
            }           
        }

        public ActionResult Details(int id)
        {
            var product = _productRepository.Get(id);
            if(product == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(product);
        }

        public ActionResult GetImage(int id, int? maxWidth, int? maxHeight)
        {
            var product = _productRepository.Get(id);
            if (product == null || string.IsNullOrWhiteSpace(product.Image))
            {
                return base.File("~/Content/images/no_image.jpg", "image/jpg");
            }

            var dir = Server.MapPath("~/App_Data/Images");
            var filePath = Path.Combine(dir, product.Image);
            if (System.IO.File.Exists(filePath))
            {
                if(maxWidth.HasValue && maxHeight.HasValue)
                {
                    var image = Helpers.Images.ScaleImage(filePath, maxWidth.Value, maxHeight.Value);
                    return base.File(Helpers.Images.ToStream(image), MimeMapping.GetMimeMapping(filePath));
                }

                
                return base.File(filePath, MimeMapping.GetMimeMapping(filePath));
            }
            else
            {
                return base.File("~/Content/images/no_image.jpg", "image/jpg");
            }
        }


        public ActionResult Search(string q)
        {
            List<Product> searchResult = _productRepository.Search(q);

            var retObj = searchResult.Select(a => new { Name = a.ProductName, Id = a.ProductId }).ToList();

            return Content(JsonConvert.SerializeObject(retObj), "application/json");
        }
    }
}