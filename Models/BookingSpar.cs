using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PetCare_system.Models
{
    public class BookingSpar
    {
        [Key]
        public int BookingSparId { get; set; }

        [Required]
        public string UserId { get; set; } // Links to Identity User

        [Required]
        public int SpaServiceId { get; set; }

        public SpaService SpaService { get; set; } // Navigation property

        [Required]
        public DateTime BookingDate { get; set; }

        [Required]
        public string PetName { get; set; }

        [Required]
        public string PetType { get; set; } // e.g., Dog, Cat

        public string SpecialInstructions { get; set; }

        public bool IsPaid { get; set; } = false;
    }
}