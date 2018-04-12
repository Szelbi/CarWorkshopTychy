using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarWorkshop.Database
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required, DisplayName("Nazwa kategorii")]
        public string CategoryName { get; set; }

        public bool Deleted { get; set; }


        public virtual ICollection<Product> Products { get; set; }
    }
}