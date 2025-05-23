using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCare_system.Models
{
    public class SpaService
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SpaServiceId { get; set; }

        [Required]
        public string Name { get; set; } // e.g., "Basic Grooming", "Luxury Spa"

        [Required]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Required]
        public int DurationMinutes { get; set; } // e.g., 30, 60, 90 mins
    }
}