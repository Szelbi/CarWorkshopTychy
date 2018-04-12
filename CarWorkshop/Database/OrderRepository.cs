using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using CarWorkshop.Models;

namespace CarWorkshop.Database
{
    public class OrderRepository
    {
        private DatabaseContext db;

        public OrderRepository(DatabaseContext db)
        {
            this.db = db;
        }

        public Order Create(Order order, string userId = "")
        {
            order.Date = DateTime.Now;
            order.OrderNumber = Order.GenerateNumber();
            order.PaymentIdentifier = Order.GenerateNumber();

            if (!string.IsNullOrWhiteSpace(userId))
            {
                ApplicationUser user = db.Users.FirstOrDefault(u => u.Id.Equals(userId));
                order.User = user;
            }

            foreach (var op in order.OrderProducts)
            {
                var dbProduct = db.Products.FirstOrDefault(c => c.ProductId.Equals(op.Product.ProductId));
                op.Product = dbProduct;
                op.Price = dbProduct.Price;
            }

            db.Orders.Add(order);
            db.SaveChanges();

            return order;
        }

        public List<Order> UserOrders(string userId)
        {
            List<Order> list = db.Orders.Where(o => o.User.Id.Equals(userId)).ToList();
            return list;
        }

        public List<Order> List()
        {
            List<Order> list = db.Orders.Where(o => !o.Deleted).ToList();
            return list;
        }

        public Order Get(int id)
        {
            return db.Orders.FirstOrDefault(o => o.OrderId.Equals(id));

        }

        public Order ChangeOrderStatus(int id, OrderState status)
        {
            var order = db.Orders.FirstOrDefault(o => o.OrderId.Equals(id));
            if (order != null)
            {
                try
                {
                    order.State = status;
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    var a = e;
                }
            }

            return order;
        }

        public Address SaveAddress(string userId, Address address)
        {
            var dbAddress = db.Addresses.FirstOrDefault(a => a.User.Id.Equals(userId));
            if(dbAddress == null)
            {
                dbAddress = new Address()
                {
                    City = address.City,
                    Email = address.Email,
                    Name = address.Name,
                    PostCode = address.PostCode,
                    Street = address.Street
                };

                var user = db.Users.First(u => u.Id.Equals(userId));
                dbAddress.User = user;
                db.Addresses.Add(dbAddress);
            }
            else
            {
                dbAddress.City = address.City;
                dbAddress.Email = address.Email;
                dbAddress.Name = address.Name;
                dbAddress.PostCode = address.PostCode;
                dbAddress.Street = address.Street;
            }
            db.SaveChanges();
            return dbAddress;
        }

        public Address GetDatabaseAddressForUser(string id)
        {
            var user = db.Users.First(u => u.Id.Equals(id));
            var dbAddress = db.Addresses.FirstOrDefault(a => a.User.Id.Equals(id));
            if (dbAddress == null)
            {
                dbAddress = new Address();
                dbAddress.Email = user.Email;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(dbAddress.Email))
                {
                    dbAddress.Email = user.Email;
                }
            }

            return dbAddress;
        }

        public Models.Address GetAddressForUser(string id)
        {
            CarWorkshop.Models.Address address = new Models.Address();
            var user = db.Users.FirstOrDefault(u => u.Id.Equals(id));
            if(user != null)
            {
                var dbAddress = db.Addresses.FirstOrDefault(a => a.User.Id.Equals(id));
                if (dbAddress == null)
                {
                    dbAddress = new Address();
                    dbAddress.Email = user.Email;
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(dbAddress.Email))
                    {
                        dbAddress.Email = user.Email;
                    }
                }
                address.City = dbAddress.City;
                address.Email = dbAddress.Email;
                address.Name = dbAddress.Name;
                address.PostCode = dbAddress.PostCode;
                address.Street = dbAddress.Street;
            }



            return address;
        }

        public Order SetDotpayData(string paymentId, string dotpaypaymentid)
        {
            var order = db.Orders.FirstOrDefault(o => o.PaymentIdentifier.Equals(paymentId));
            if(order != null)
            {
                order.DotpayPaymentIdentifier = dotpaypaymentid;
                order.DotpayPaymentDate = DateTime.Now;
                
                if(order.State != OrderState.CLOSED)
                {
                    order.State = OrderState.PAYD;
                }

                db.SaveChanges();
            }
            return order;
        }
    }
}