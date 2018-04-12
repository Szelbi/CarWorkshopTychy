using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarWorkshop.Database
{
    public class TypesRepository
    {
        DatabaseContext db;

        public TypesRepository(DatabaseContext db)
        {
            this.db = db;
        }

        public List<DeliverType> DeliverTypesList()
        {
            return db.DeliverTypes.ToList();
        }

        public List<PaymentType> PaymentTypesList()
        {
            return db.PaymentTypes.ToList();
        }

        public static PaymentType GetPaymentType(int id)
        {
            using (var db = new DatabaseContext())
            {
                return db.PaymentTypes.FirstOrDefault(t => t.PaymentTypeId.Equals(id));
            }
        }

        public static DeliverType GetDeliverType(int id)
        {
            using (var db = new DatabaseContext())
            {
                return db.DeliverTypes.FirstOrDefault(t => t.DeliverTypeId.Equals(id));
            }
        }

        public static string GetDeliverTypeWithPrice(int id)
        {
            string name = "";
            using (var db = new DatabaseContext())
            {
                var type = db.DeliverTypes.FirstOrDefault(t => t.DeliverTypeId.Equals(id));
                if(type != null)
                {
                    name = $"{type.Name} - {type.Price} PLN";
                }
            }
            return name;
        }

        public static string GetPaymentTypeWithPrice(int id)
        {
            string name = "";
            using (var db = new DatabaseContext())
            {
                var type = db.PaymentTypes.FirstOrDefault(t => t.PaymentTypeId.Equals(id));
                if (type != null)
                {
                    name = $"{type.Name} - {type.Price} PLN";
                }
            }
            return name;
        }
    }
}