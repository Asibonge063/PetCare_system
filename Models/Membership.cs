using System;
using System.ComponentModel.DataAnnotations;

namespace PetCare_system.Models
{
    public class Membership
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "Pet")]
        public int PetId { get; set; }

        [Required]
        [Display(Name = "Membership Type")]
        public string MembershipType { get; set; }

        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        public string Status { get; set; } // Active, Expired, Cancelled

        [Required]
        [Display(Name = "Amount Paid")]
        public decimal PaymentAmount { get; set; }

        [Required]
        [Display(Name = "Payment Method")]
        public string PaymentMethod { get; set; }

        [Display(Name = "Payment Status")]
        public string PaymentStatus { get; set; }

        [Display(Name = "Payment Date")]
        public DateTime PaymentDate { get; set; }

        [Display(Name = "Cancellation Date")]
        public DateTime? CancellationDate { get; set; }

        [Display(Name = "Last Renewal Date")]
        public DateTime? LastRenewalDate { get; set; }

        // Credit Card Details
        [Display(Name = "Account Number")]
        public string AccountNumber { get; set; }

        [Display(Name = "CVV")]
        public string CVV { get; set; }

        [Display(Name = "Expiration Date")]
        public DateTime? ExpiryDate { get; set; }

        [Display(Name = "Account Holder")]
        public string AccountHolder { get; set; }

        [Display(Name = "Bank Type")]
        public string BankType { get; set; }

        public int? PreviousMembershipId { get; set; }

        // Navigation properties
        public virtual Pet Pet { get; set; }
    }
}