using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PetCare_system.Models
{
    public class Doctor
    {
        
        public int Id { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Specialization")]
        public string Specialization { get; set; }

        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; } // Admin will set this manually

        public string PasswordHash { get; set; } // Store hashed password

        // Change from boolean to string
        public string AvailabilityStatus { get; set; } // Example: "Available"

    }
}