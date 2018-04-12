using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarWorkshop.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class ClientsController : Controller
    {
        // GET: Admin/Clients
        public ActionResult Index()
        {
            return View();
        }
    }
}