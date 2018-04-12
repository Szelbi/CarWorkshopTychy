using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarWorkshop.Database
{
    public class Address
    {
        public int AddressId { get; set; }

        [Required, DisplayName("Adresat")]
        public string Name { get; set; }

        [Required, DisplayName("Adres")]
        public string Street { get; set; }

        [Required, DisplayName("Kod pocztowy")]
        public string PostCode { get; set; }

        [Required, DisplayName("Miasto")]
        public string City { get; set; }

        [Required, DisplayName("Email do zamówienia")]
        public string Email { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}