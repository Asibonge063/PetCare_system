using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PetCare_system.Models
{
    public class PaymentForSpar
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int BookingId { get; set; }

        public BookingSpar Booking { get; set; } // Navigation property

        [Required]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; } = DateTime.Now;

        [Required]
        public string PaymentMethod { get; set; } // "Credit Card", "PayPal", etc.

        public string TransactionId { get; set; } // For payment gateways
    }
}