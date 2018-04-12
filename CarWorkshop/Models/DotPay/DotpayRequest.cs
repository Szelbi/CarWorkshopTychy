using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarWorkshop.Models.DotPay
{
    public class DotpayRequest
    {
        public string id { get; set; } 
        public decimal amount { get; set; }
        public string currency { get; set; }
        public string description { get; set; }
        public string URLC { get; set; }
        public string URL { get; set; }
        public string Email { get; set; }

        public DotpayRequest(decimal amount, string description, string email)
        {
            string baseUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            if(!string.IsNullOrWhiteSpace(System.Configuration.ConfigurationManager.AppSettings["BaseUrl"]))
            {
                baseUrl = System.Configuration.ConfigurationManager.AppSettings["BaseUrl"].ToString();
            }

            this.id = System.Configuration.ConfigurationManager.AppSettings["DotPay_ShopID"].ToString();
            this.amount = amount;
            this.description = description;
            this.URLC =  baseUrl + "/dotpay/response";
            this.URL = baseUrl + "/orders/orderconfirmation?order=" + description;
            this.currency = "PLN";
            this.Email = email;
        }

        public override string ToString()
        {                   
            string dotpayurl = System.Configuration.ConfigurationManager.AppSettings["DotPay_Url"].ToString();
            return $"{dotpayurl}?id={id}&amount={amount}&currency={currency}&description={description}&urlc={URLC}&url={URL}&email={Email}&type=0";
        }
    }
}