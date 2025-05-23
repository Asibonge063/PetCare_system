using PetCare_system.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;


namespace PetCare_system.Models
{
    public class PaymentForBoard
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Payment_boardId { get; set; }

        // Relationship to boarding reservation
        [Required]
        [ForeignKey("Boarding")]
        [Display(Name = "Boarding Reservation")]
        public int BoardingId { get; set; }
        public virtual Pet_Boarding Boarding { get; set; }

        // Payment amount (linked to boarding package)
        [Required]
        [Display(Name = "Amount Paid")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal AmountPaid { get; set; }

        [Required]
        [Display(Name = "Payment Date")]
        public DateTime PaymentDate { get; set; } = DateTime.Now;

        // Payment method information
        [Required]
        [StringLength(50)]
        [Display(Name = "Payment Method")]
        public string PaymentMethod { get; set; }

        // Bank Type (e.g., Visa, MasterCard, etc.)
        [Required]
        [StringLength(50)]
        [Display(Name = "Bank Type")]
        public string BankType { get; set; } // Visa, MasterCard, etc.


        // Card details
        [StringLength(100)]
        [Display(Name = "Cardholder Name")]
        public string CardHolderName { get; set; }
        // Credit Card Details
        [Required(ErrorMessage = "Card number is required")]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "Card number must be 16 digits")]
        [RegularExpression(@"^[0-9]{16}$", ErrorMessage = "Card number must contain only digits")]
        [Display(Name = "Account Number")]
        public string AccountNumber { get; set; }

        [StringLength(3, ErrorMessage = "CVV must be 3 digits.")]
        [Display(Name = "CVV")]
        public string CVV { get; set; }

        [Display(Name = "Expiration Date")]
        [DataType(DataType.Date)]
        public DateTime? ExpiryDate { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Payment Status")]
        public string Status { get; set; } = "Pending";

        [StringLength(100)]
        [Display(Name = "Transaction ID")]
        public string TransactionId { get; set; }

        // User who made the payment
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        // Helper method to set amount based on package
        public void SetAmountFromPackage(string package)
        {
            if (package.Contains("Standard")) AmountPaid = 350;
            else if (package.Contains("Deluxe")) AmountPaid = 500;
            else if (package.Contains("Executive")) AmountPaid = 750;
        }
    }
}