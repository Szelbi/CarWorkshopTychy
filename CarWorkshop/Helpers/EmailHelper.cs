using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarWorkshop.Helpers
{
    public class EmailHelper
    {
        public static string GetUsernameFromEmail(string email)
        {
            if(string.IsNullOrWhiteSpace(email))
            {
                return "";
            }
            var splitEmail = email.Split('@');
            return splitEmail[0];
        }
    }
}