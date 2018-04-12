using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarWorkshop.Database;
using CarWorkshop.Models;

namespace CarWorkshop.Controllers
{
    public class ShoppingCartController : Controller
    {
        DatabaseContext db = new DatabaseContext();
        ProductRepository _productRepository;
        ShoppingCart cart;

        public ShoppingCartController()
        {
            _productRepository = new ProductRepository(db);
            cart = ShoppingCart.GetCart();
        }
        // GET: ShoppingCart
        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart();


            // Return the view
            return View(cart);
        }

        public ActionResult AddToCart(int id)
        {
            var product = _productRepository.Get(id);
            if(product != null)
            {
                cart.AddToCart(product);
            }
            return Content("ok");
        }

        public ActionResult RemoveFromCart(int id, bool onlyCount = false)
        {
            cart.RemoveFromCart(id, onlyCount);
            return Content("ok");
        }

        public ActionResult GetItemsCount()
        {
            return Content(cart.GetItemsCount().ToString());
        }

        public ActionResult RefreshCart()
        {
            return PartialView("~/Views/ShoppingCart/Components/_shoppingCartTable.cshtml", cart);
        }
    }
}