using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace PetCare_system.Models
{
    public class Spar_GroomPayment
    {
        [Key]

        public int BookingId { get; set; }

        [DisplayName("Booking Total")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Amount { get; set; }

        [Required]
        [DisplayName("Card Number")]
        [CreditCard(ErrorMessage = "Invalid credit card number")]
        public string CardNumber { get; set; }

        [Required]
        [DisplayName("Expiration Date")]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/?([0-9]{2})$", ErrorMessage = "Invalid expiration date")]
        public string ExpiryDate { get; set; }

        [Required]
        [DisplayName("CVV")]
        [RegularExpression(@"^[0-9]{3,4}$", ErrorMessage = "Invalid CVV")]
        public string CVV { get; set; }

        [Required]
        [DisplayName("Name on Card")]
        public string CardName { get; set; }


    }
}