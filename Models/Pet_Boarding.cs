using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace PetCare_system.Models
{
    public class Pet_Boarding
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int board_Id { get; set; }

        // Owner and pet information
        [Required]
        public string OwnerName { get; set; }
        [Required]
        public string PetName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, Phone]
        public string Phone { get; set; }
        [Required]
        public string PetType { get; set; }
        public string PetBreed { get; set; }

        // Dates
        [Required, DataType(DataType.Date)]
        public DateTime CheckInDate { get; set; }
        [Required, DataType(DataType.Date)]
        public DateTime CheckOutDate { get; set; }
        public DateTime BookingDate { get; set; } = DateTime.Now;

        // Package information
        [Required]
        public string Package { get; set; }

        [NotMapped]
        public decimal PackagePrice
        {
            get
            {
                if (Package.Contains("Standard")) return 350;
                if (Package.Contains("Deluxe")) return 500;
                if (Package.Contains("Executive")) return 750;
                return 0;
            }
        }

        public string SpecialNeeds { get; set; }
        [Required]
        public bool Agreement { get; set; }
        public string Status { get; set; } = "Pending"; // Default status set to "Pending"
        public string Check_Status { get; set; } = "null";


        // Navigation property
        public virtual ICollection<PaymentForBoard> Payments { get; set; }
    }
}