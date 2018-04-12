using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CarWorkshop.Database;

namespace CarWorkshop.Models
{
    public class SessionOrder
    {
        private static string key = "_sessionOrder";

        public Order order { get; set; }

        public SessionOrder(Order order)
        {
            this.order = order;
            this.Save();
        }

        public static SessionOrder Get()
        {
            var cart = HttpContext.Current.Session[key] as SessionOrder;
            return cart;
        }

        public void Save()
        {
            HttpContext.Current.Session[key] = this;
        }
    }
}