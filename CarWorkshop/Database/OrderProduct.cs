using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarWorkshop.Database
{
    public class OrderProduct
    {
        public int OrderProductId { get; set; }


        [Required]
        public virtual Product Product { get; set; }

        [Required]
        public decimal Price { get; set; }
        public int Count { get; set; }

        public decimal GetTotalFromProduct()
        {
            return Product.Price * Count;
        }

        public decimal GetTotal()
        {
            return this.Price * this.Count;
        }

    }
}