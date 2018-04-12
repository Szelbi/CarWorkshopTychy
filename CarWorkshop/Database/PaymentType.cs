using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarWorkshop.Database
{
    public class PaymentType
    {
        public int PaymentTypeId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}