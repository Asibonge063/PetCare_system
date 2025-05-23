using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCare_system.Models
{
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentId { get; set; }

        // Fixed payment amount
        [Required]
        [Display(Name = "Amount Paid")]
        public decimal AmountPaid { get; set; } = 500m; // Fixed amount, you can change this value as needed

        [Required]
        [Display(Name = "Payment Date")]
        public DateTime PaymentDate { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Payment Method")]
        public string PaymentMethod { get; set; } // Credit Card, Cash, etc.

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


        [StringLength(100)]
        [Display(Name = "Account Holder")]
        public string AccountHolder { get; set; }

        // Bank Type (e.g., Visa, MasterCard, etc.)
        [Required]
        [StringLength(50)]
        [Display(Name = "Bank Type")]
        public string BankType { get; set; } // Visa, MasterCard, etc.

        // Foreign key to Vet_Consultations
        public int ConsultId { get; set; }

        [ForeignKey("ConsultId")]
        public virtual Vet_Consultations VetConsultation { get; set; }

        [Display(Name = "Payment Status")]
        public bool PaymentStatus { get; set; }

        [Display(Name = "Transaction Reference")]
        public string TransactionReference { get; set; }

        // Process the payment with a fixed amount
        public void ProcessPayment()
        {
            // In this example, the amount is fixed (500m)
            if (AmountPaid >= 500m)
            {
                PaymentStatus = true;
            }
            else
            {
                PaymentStatus = false;
            }
        }
    }
}
