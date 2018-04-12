using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarWorkshop.Database;
using CarWorkshop.Models;
using Microsoft.AspNet.Identity;

namespace CarWorkshop.Controllers
{
    public class CheckoutController : Controller
    {
        DatabaseContext db = new DatabaseContext();
        ShoppingCart cart;
        TypesRepository _tr;

        OrderRepository _or;

        public CheckoutController()
        {
            cart = ShoppingCart.GetCart();
            _or = new OrderRepository(db);
            _tr = new TypesRepository(db);
        }

        // GET: Checkout
        public ActionResult Index()
        {
            var changes = cart.CheckProductChanges(db);
            if (changes)
            {
                TempData["cart_error"] = "Nasze produkty zostały zaaktualizowane. Proszę sprawdzić swój koszyk z aktualnymi danymi produktu.";
                return RedirectToAction("Index", "ShoppingCart");
            }
            return RedirectToAction("Additional");
        }

        public ActionResult Additional()
        {
            Models.Address address = new Models.Address();
            if (User.Identity.IsAuthenticated)
            {
                address = _or.GetAddressForUser(User.Identity.GetUserId());
            }

            ViewData["paymentTypes"] = _tr.PaymentTypesList();
            ViewData["deliverTypes"] = _tr.DeliverTypesList();

            return View(address);
        }

        [HttpPost]
        public ActionResult Additional(Models.Address address)
        {
            if (!ModelState.IsValid)
            {
                ViewData["paymentTypes"] = _tr.PaymentTypesList();
                ViewData["deliverTypes"] = _tr.DeliverTypesList();
                return View(address);
            }

            var order = new Order()
            {
                Address = address.Street,
                City = address.City,
                PostCode = address.PostCode,
                Name = address.Name,
                OrderProducts = cart.products,
                Email = address.Email,
                DeliverTypeId = address.DeliverType,
                PaymentTypeId = address.PaymentType
            };
            order.CalulateOrderProducts();
            new SessionOrder(order);

            return RedirectToAction("Summary");
        }

        public ActionResult Summary()
        {
            var sessionOrder = SessionOrder.Get();
            if (sessionOrder == null)
            {
                TempData["cart_error"] = "Błąd skladania zamówienia. Prosimy złożyć zamówienie jeszcze raz.";
                return RedirectToAction("Index", "ShoppingCart");
            }
            return View(sessionOrder.order);
        }

        public ActionResult Cancel()
        {
            cart.Clear();
            return RedirectToAction("Index", "Home");
        }

        public async System.Threading.Tasks.Task<ActionResult> Payment()
        {
            var sessionOrder = SessionOrder.Get();
            if (sessionOrder == null)
            {
                TempData["cart_error"] = "Błąd skladania zamówienia. Prosimy złożyć zamówienie jeszcze raz.";
                return RedirectToAction("Index", "ShoppingCart");
            }

            if (sessionOrder.order.CheckProductChanges(db))
            {
                TempData["cart_error"] = "Błąd skladania zamówienia. Dane produktów zostały zmienione.";
                return RedirectToAction("Index", "ShoppingCart");
            }

            string userId = "";
            if (User.Identity.IsAuthenticated)
            {
                userId = User.Identity.GetUserId();
            }

            var order = new OrderRepository(db).Create(sessionOrder.order, userId);

            await Helpers.EmailService.EmailSendOrder(order.Email, "Twoje zamówienie zostało utworzone", new OrderRepository(db).Get(order.OrderId));

            cart.Clear();


            if (order.PaymentTypeId == 1)
            {
                var a = new Models.DotPay.DotpayRequest(order.GetTotal(), order.PaymentIdentifier, order.Email);
                return Redirect(a.ToString());
            }
            else
            {
                return RedirectToAction("OrderConfirmation", "Orders", new { area = "", order = order.PaymentIdentifier });
            }



        }
    }
}