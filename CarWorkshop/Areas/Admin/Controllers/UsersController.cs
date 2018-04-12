using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarWorkshop.Database;

namespace CarWorkshop.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class UsersController : Controller
    {
        DatabaseContext db = new DatabaseContext();
        UsersRepository _ur;

        public UsersController()
        {
            this._ur = new UsersRepository(db);
        }
        // GET: Admin/Users
        public ActionResult Index()
        {
            return View(_ur.List());
        }
    }
}