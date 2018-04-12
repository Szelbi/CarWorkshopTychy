using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarWorkshop.Database
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required, DisplayName("Nazwa produktu")]
        public string ProductName { get; set; }

        [DisplayName("Opis produktu"), DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public bool Deleted { get; set; }

        [DisplayName("Cena produktu"), Required(ErrorMessage ="Niepoprawna wartość ceny"), DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [DisplayName("Zdjęcie produktu")]
        public string Image { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}