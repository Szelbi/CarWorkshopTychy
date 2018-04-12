using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarWorkshop.Helpers
{
    public class StatusHelper
    {
        public static string StatusToString(Database.OrderState state)
        {
            switch (state) {
                case Database.OrderState.NEW: return "Nowe";
                case Database.OrderState.CLOSED: return "Zamknięte";
                case Database.OrderState.PAYD: return "Zapłacone";
                case Database.OrderState.SENDED: return "Wysłane";
            }
            return "";
        }
    }
}