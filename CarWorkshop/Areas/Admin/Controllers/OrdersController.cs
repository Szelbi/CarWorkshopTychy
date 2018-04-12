using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarWorkshop.Database;

namespace CarWorkshop.Areas.Admin.Controllers
{
    [Authorize(Roles="admin")]
    public class OrdersController : Controller
    {
        DatabaseContext db = new DatabaseContext();
        OrderRepository _or;

        public OrdersController()
        {
            _or = new OrderRepository(db);
        }
        // GET: Admin/Order
        public ActionResult Index()
        {
            var list = _or.List().OrderByDescending(a => a.Date).ToList();
            return View(list);
        }

        public ActionResult Details(int id)
        {
            var order = _or.Get(id);
            if(order == null)
            {
                return RedirectToAction("Index");
            }

            return View(order);
        }

        public async System.Threading.Tasks.Task<ActionResult> SendOrder(int id)
        {
            var order = _or.ChangeOrderStatus(id, OrderState.SENDED);
            if (order == null)
            {
                return RedirectToAction("Index");
            } else {
                //wysłanie emaila
                if(!string.IsNullOrWhiteSpace(order.Email))
                {
                   await  Helpers.EmailService.SendEmail(order.Email, "Zmiana statusu zamówienia", $"Twoje zamówienie o numerze {order.OrderNumber} zmieniło status na <b>WYSŁANE</b>");
                }
            }
            
            return RedirectToAction("Details", new { id = id });
        }

        public async System.Threading.Tasks.Task<ActionResult> CloseOrder(int id)
        {
            var order = _or.ChangeOrderStatus(id, OrderState.CLOSED);
            if (order == null)
            {
                return RedirectToAction("Index");
            } else
            {
                //wysłanie emaila
                if (!string.IsNullOrWhiteSpace(order.Email))
                {
                    await Helpers.EmailService.SendEmail(order.Email, "Zmiana statusu zamówienia", $"Twoje zamówienie o numerze {order.OrderNumber} zmieniło status na <b>ZAMKNIĘTE</b>");
                }
            }

            return RedirectToAction("Details", new { id = id });
        }
    }
}