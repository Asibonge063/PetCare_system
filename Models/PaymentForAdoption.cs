using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCare_system.Models
{
    public class PaymentForAdoption
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Payment_Id { get; set; }

        [Required]
        [ForeignKey("Adoption")]
        public int AdoptionId { get; set; }
        public virtual Pet_Adoption Adoption { get; set; }

        [Required]
        [Display(Name = "Amount")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Display(Name = "Pet Name")]
        public string PetName { get; set; }

        [Required]
        [Display(Name = "Payment Date")]
        public DateTime PaymentDate { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Payment Method")]
        public string PaymentMethod { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Bank Type")]
        public string BankType { get; set; }

        [StringLength(100)]
        [Display(Name = "Cardholder Name")]
        public string CardHolderName { get; set; }

        [Required(ErrorMessage = "Card number is required")]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "Card number must be 16 digits")]
        [RegularExpression(@"^[0-9]{16}$", ErrorMessage = "Card number must contain only digits")]
        [Display(Name = "Account Number")]
        public string AccountNumber { get; set; }

        [StringLength(3, ErrorMessage = "CVV must be 3 digits")]
        [Display(Name = "CVV")]
        public string CVV { get; set; }

        [Display(Name = "Expiration Date")]
        [DataType(DataType.Date)]
        public DateTime? ExpiryDate { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Payment Status")]
        public string Status { get; set; }

        [StringLength(100)]
        [Display(Name = "Transaction ID")]
        public string TransactionId { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}