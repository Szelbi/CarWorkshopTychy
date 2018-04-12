using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarWorkshop.Database;
using Microsoft.AspNet.Identity;

namespace CarWorkshop.Controllers
{
    
    public class OrdersController : Controller
    {
        DatabaseContext db = new DatabaseContext();
        OrderRepository _or;
        ProductRepository _pr;

        public OrdersController()
        {
            _or = new OrderRepository(db);
            _pr = new ProductRepository(db);
        }

        [Authorize]
        public ActionResult Index()
        {
            var o =_or.UserOrders(User.Identity.GetUserId());
            o.Reverse();
            return View(o);
        }

        public ActionResult OrderConfirmation(string order = "")
        {
            return View(order as object);
        }

        [Authorize]
        public ActionResult Details(int id)
        {
            var or = _or.UserOrders(User.Identity.GetUserId());
            var order = or.FirstOrDefault(o => o.OrderId.Equals(id));
            if(order != null)
            {
                return View(order);
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Address()
        {
            var address = _or.GetDatabaseAddressForUser(User.Identity.GetUserId());
            return View(address);
        }

        [HttpPost, Authorize]
        public ActionResult  Address(Address model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var a = _or.SaveAddress(User.Identity.GetUserId(), model);
            return RedirectToAction("Index");
        }

    }
}