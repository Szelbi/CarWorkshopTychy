using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CarWorkshop.Database;

namespace CarWorkshop.Models
{
    public class ShoppingCart
    {
        public List<OrderProduct> products { get; set; }

        public DeliverType deliverType { get; set; }

        public PaymentType paymentType { get; set; }

        private static string _cartKey = "shoppingCart";

        public ShoppingCart()
        {
            this.products = new List<OrderProduct>();
        }

        public static ShoppingCart GetCart()
        {
            var cart = HttpContext.Current.Session[_cartKey] as ShoppingCart;
            if (cart == null)
            {
                cart = new ShoppingCart();
                HttpContext.Current.Session[_cartKey] = cart;
            }
            return cart;
        }

        public void Save()
        {
            HttpContext.Current.Session[_cartKey] = this;
        }

        public void RemoveFromCart(int id, bool onlyCount = false)
        {
            var orderProduct = this.products.FirstOrDefault(p => p.Product != null && p.Product.ProductId.Equals(id));
            if (orderProduct != null)
            {
                if (onlyCount)
                {
                    orderProduct.Count--;
                    if(orderProduct.Count == 0)
                    {
                        this.products.Remove(orderProduct);
                    }
                }
                else
                {
                    this.products.Remove(orderProduct);
                }


            }
            this.Save();
        }

        public void AddToCart(Product product)
        {
            var orderProduct = this.products.FirstOrDefault(p => p.Product != null && p.Product.ProductId.Equals(product.ProductId));
            if (orderProduct != null)
            {
                orderProduct.Count++;
            }
            else
            {
                orderProduct = new OrderProduct()
                {
                    Product = product,
                    Count = 1
                };
                this.products.Add(orderProduct);

            }
            this.Save();
        }

        public decimal GetTotal()
        {
            decimal total = 0;
            foreach (var p in this.products)
            {
                total += p.GetTotalFromProduct();
            }
            return total;
        }

        public int GetItemsCount()
        {
            int count = 0;
            this.products.ForEach(p => { count += p.Count; });
            return count;
        }

        public static int GetItemCount()
        {
            var cart = ShoppingCart.GetCart();
            return cart.GetItemsCount();
        }

        public bool CheckProductChanges(DatabaseContext db)
        {
            bool changes = false;
            foreach (var p in this.products)
            {
                var dbProduct = db.Products.FirstOrDefault(a => a.ProductId.Equals(p.Product.ProductId));
                if(dbProduct == null || dbProduct.Deleted)
                {
                    this.products.Remove(p);
                    changes = true;
                }
                else
                {
                    if (!dbProduct.Price.Equals(p.Product.Price) || !dbProduct.ProductName.Equals(p.Product.ProductName))
                    {
                        p.Product = dbProduct;
                        changes = true;
                    }
                }
            }
            if(changes)
            {
                this.Save();
            }
            
            return changes;
        }

        public void Clear()
        {
            HttpContext.Current.Session[_cartKey] = new ShoppingCart();
        }

    }
}