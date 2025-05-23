using System;
using System.ComponentModel.DataAnnotations;

namespace PetCare_system.Models
{
    public class MembershipViewModel
    {
        [Required]
        public int PetId { get; set; }

        [Required]
        [Display(Name = "Membership Type")]
        public string MembershipType { get; set; }

        [Required]
        [Display(Name = "Amount Paid")]
        public decimal AmountPaid { get; set; }

        [Required]
        [Display(Name = "Payment Date")]
        public DateTime PaymentDate { get; set; } = DateTime.Now;

        [Required]
        [StringLength(50)]
        [Display(Name = "Payment Method")]
        public string PaymentMethod { get; set; }

        // Credit Card Details
        [StringLength(16, MinimumLength = 13, ErrorMessage = "Card number must be between 13 and 16 digits.")]
        [Display(Name = "Account Number")]
        public string AccountNumber { get; set; }

        [StringLength(3, ErrorMessage = "CVV must be 3 digits.")]
        [Display(Name = "CVV")]
        public string CVV { get; set; }

        [Display(Name = "Expiration Date")]
        [DataType(DataType.Date)]
        public DateTime? ExpiryDate { get; set; }

        [StringLength(100)]
        [Display(Name = "Account Holder")]
        public string AccountHolder { get; set; }

        [StringLength(50)]
        [Display(Name = "Bank Type")]
        public string BankType { get; set; }
    }
}