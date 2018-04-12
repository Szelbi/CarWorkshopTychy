using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarWorkshop.Database;
using CarWorkshop.Models.DotPay;

namespace CarWorkshop.Controllers
{
    public class DotpayController : Controller
    {
        DatabaseContext db = new DatabaseContext();
        OrderRepository _or;

        public DotpayController()
        {
            _or = new OrderRepository(db);
        }

        // GET: Dotpay
        public ActionResult Index()
        {
            return View();
        }

        public async System.Threading.Tasks.Task<ActionResult> Response(DotPayResponse response)
        {
            var address = Request.UserHostAddress;
            var order = _or.SetDotpayData(response.description, response.operation_number);
            if (order.State == OrderState.PAYD && !string.IsNullOrWhiteSpace(order.Email))
            {
                await Helpers.EmailService.SendEmail(order.Email, "Zmiana statusu zamówienia", $"Twoje zamówienie o numerze {order.OrderNumber} zmieniło status na <b>ZAPŁACONE</b>");
            }
            return Content("ok");
        }
    }
}