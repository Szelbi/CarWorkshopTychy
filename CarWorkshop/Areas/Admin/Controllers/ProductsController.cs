using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarWorkshop.Database;

namespace CarWorkshop.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class ProductsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        private ProductRepository _productRepository;
        private CategoryRepository _categoryRepository;

        public ProductsController()
        {
            _productRepository = new ProductRepository(db);
            _categoryRepository = new CategoryRepository(db);
        }

        // GET: Admin/Products
        public ActionResult Index()
        {
            return View(_productRepository.List());
        }


        // GET: Admin/Products/Create
        public ActionResult Create()
        {
            ViewData["categories"] = _categoryRepository.List();
            return View();
        }

        // POST: Admin/Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductName,Description,Price")] Product product, int? productCategory, HttpPostedFileBase file)
        {
            string fileName = "";
            if (file != null && file.ContentLength > 0)
            {
                var fileExtension = Path.GetExtension(file.FileName);
                fileName = Guid.NewGuid().ToString().Replace("-", "") + fileExtension;
                var path = Path.Combine(Server.MapPath("~/App_Data/Images/"), fileName);
                file.SaveAs(path);
            }

            if (ModelState.IsValid)
            {
                if (productCategory.HasValue && _categoryRepository.Exists(productCategory.Value))
                {
                    product.CategoryId = productCategory.Value;
                }

                product.Image = fileName;
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewData["categories"] = new CategoryRepository(db).List();
            return View(product);
        }

        // GET: Admin/Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            ViewData["categories"] = new CategoryRepository(db).List();

            return View(product);
        }

        // POST: Admin/Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind(Include = "ProductName,Description,Price")] Product product, int? productCategory, HttpPostedFileBase file)
        {
            string fileName = "";
            if (file != null && file.ContentLength > 0)
            {
                var fileExtension = Path.GetExtension(file.FileName);
                fileName = Guid.NewGuid().ToString().Replace("-", "") + fileExtension;
                var path = Path.Combine(Server.MapPath("~/App_Data/Images/"), fileName);
                file.SaveAs(path);
            }

            if (ModelState.IsValid)
            {
                var dbProduct = db.Products.FirstOrDefault(p => p.ProductId.Equals(id));

                if(dbProduct == null)
                {
                    return RedirectToAction("Index");
                }

                if (productCategory.HasValue && _categoryRepository.Exists(productCategory.Value))
                {
                    dbProduct.CategoryId = productCategory.Value;
                }
                else
                {
                    dbProduct.Category = null;
                }

                if(fileName != null)
                {
                    dbProduct.Image = fileName;
                }

                dbProduct.ProductName = product.ProductName;
                dbProduct.Description = product.Description;
                dbProduct.Price = product.Price;
               
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewData["categories"] = new CategoryRepository(db).List();
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            product.Deleted = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Search(string search)
        {
            List<Product> searchResult = _productRepository.Search(search);
            ViewData["search"] = true;

            return View("Index", searchResult);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
