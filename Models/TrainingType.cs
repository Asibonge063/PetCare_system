using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PetCare_system.Models
{
    public class TrainingType
    {
        [Key]
        public int TrainingTypeId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        public int DefaultDuration { get; set; } // Duration in minutes

        public decimal DefaultCost { get; set; } // Cost for the training session

        // Optional: A list to represent all training sessions of this type
        public virtual ICollection<Training> Trainings { get; set; }

        public TrainingType()
        {
            Trainings = new HashSet<Training>();
        }
    }
}
