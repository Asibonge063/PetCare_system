using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCare_system.Models
{
    public class Adoption
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AdoptionId { get; set; }

        // Application Details
        [Required]
        public string ExperienceLevel { get; set; }

        [Required]
        [StringLength(500)]
        public string HomeDescription { get; set; }

        [Required]
        [StringLength(1000)]
        public string AdoptionReason { get; set; }

        public string Status { get; set; } = "Pending";

        public DateTime SubmittedDate { get; set; } = DateTime.Now;

        // User Snapshot
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        public string AdopterFullName { get; set; }
        public string AdopterEmail { get; set; }
        public string AdopterPhone { get; set; }

        // Pet Snapshot
        public int Id { get; set; }
        [ForeignKey("Id")]
        public virtual Pet Pet { get; set; }

        public string PetName { get; set; }
        public string PetType { get; set; }
        public string PetBreed { get; set; }
        public DateTime PetDOB { get; set; }
    }
}