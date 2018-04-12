using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarWorkshop.Models.DotPay
{
    public class DotPayResponse
    {
        public string id { get; set; }
        public string operation_number { get; set; }
        public string operation_type { get; set; }
        public string operation_status { get; set; }
        public string operation_amount { get; set; }
        public string operation_currency { get; set; }
        public string is_completed { get; set; }
        public string operation_original_amount { get; set; }
        public string operation_original_currency { get; set; }
        public string description { get; set; }

    }
}