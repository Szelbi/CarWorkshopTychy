using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;

namespace CarWorkshop.Database
{
    public class Order
    {
        public int OrderId { get; set; }

        [Required]
        public string OrderNumber { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        public OrderState State { get; set; } = OrderState.NEW;

        [Required]
        public string PaymentIdentifier { get; set; }

        public string DotpayPaymentIdentifier { get; set; }
        public DateTime? DotpayPaymentDate { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual List<OrderProduct> OrderProducts { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string PostCode { get; set; }

        [Required]
        public string City { get; set; }

        public string Email { get; set; }

        public DateTime Date { get; set; }

        [Required]
        public int PaymentTypeId { get; set; }
        public virtual PaymentType PaymentType { get; set; }

        [Required]
        public int DeliverTypeId { get; set; }
        public virtual DeliverType DeliverType { get; set; }


        public bool Deleted { get; set; }

        public decimal GetTotal()
        {
            decimal total = 0;
            foreach (var item in this.OrderProducts)
            {
                total += item.GetTotal();
            }

            total += this.GetPaymentTypePrice();
            total += this.GetDeliverTypePrice();


            return total;
        }

        public string GetDotpayRequestString()
        {
            var dotpayRequest = new Models.DotPay.DotpayRequest(this.GetTotal(), this.PaymentIdentifier, this.Email);
            return dotpayRequest.ToString();
        }

        public string PaymentToString()
        {
            if (PaymentTypeId == 1)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Dotpay ");
                if(!string.IsNullOrWhiteSpace(this.DotpayPaymentIdentifier))
                {
                    sb.Append(this.DotpayPaymentIdentifier);
                    sb.Append(" ");
                    sb.Append($"({this.DotpayPaymentDate.ToString()})");
                }
                return sb.ToString();
            }
            else
            {
                return PaymentType.Name;
            }
        }

        public void CalulateOrderProducts()
        {
            foreach (var product in this.OrderProducts)
            {
                product.Price = product.Product.Price;
            }
        }


        public decimal GetDeliverTypePrice()
        {
            decimal price = 0;
            if (this.DeliverType == null)
            {
                var deliver = TypesRepository.GetDeliverType(this.DeliverTypeId);
                if (deliver != null)
                {
                    price = deliver.Price;
                }
            }
            else
            {
                price = this.DeliverType.Price;
            }

            return price;
        }

        public decimal GetPaymentTypePrice()
        {
            decimal price = 0;
            if (this.PaymentType == null)
            {
                var p = TypesRepository.GetPaymentType(this.PaymentTypeId);
                if (p != null)
                {
                    price = p.Price;
                }
            }
            else
            {
                price = this.PaymentType.Price;
            }

            return price;
        }

        public bool CheckProductChanges(DatabaseContext db)
        {
            bool changes = false;
            foreach (var p in this.OrderProducts)
            {
                var dbProduct = db.Products.FirstOrDefault(a => a.ProductId.Equals(p.Product.ProductId));
                if (dbProduct == null || dbProduct.Deleted)
                {
                    changes = true;
                }
                else
                {
                    if (!dbProduct.Price.Equals(p.Product.Price) || !dbProduct.ProductName.Equals(p.Product.ProductName))
                    {
                        changes = true;
                    }
                }
            }
            return changes;
        }

        public static string GenerateNumber()
        {
            Random generator = new Random();
            String r = generator.Next(0, 100000).ToString("D5");
            string date = DateTime.Now.ToString("yyMMdd");
            return $"{date}-{r}";
        }
    }

    public enum OrderState { NEW, PAYD, SENDED, CLOSED }
}