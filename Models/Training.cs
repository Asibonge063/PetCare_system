
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PetCare_system.Models
{
    public class Training
    {
        [Key]
        public int TrainingId { get; set; }

        [Required]
        [ForeignKey("Pet")]
        public int PetId { get; set; }
        public virtual Pet Pet { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        [Required]
        [ForeignKey("TrainingType")]
        public int TrainingTypeId { get; set; }
        public virtual TrainingType TrainingType { get; set; }

        public DateTime TrainingDate { get; set; }

        public int TrainingDuration { get; set; }


        public decimal TrainingCost { get; set; }

        public string TrainingLocation { get; set; }

        [Required]
        public string TrainingStatus { get; set; }

        public string ProgressNotes { get; set; }

        public DateTime? NextSessionDate { get; set; }

        public bool IsGroupSession { get; set; }

        public string PetBehaviorBefore { get; set; }

        public string PetBehaviorAfter { get; set; }

        // Relationship to Training Options
        //public virtual ICollection<TrainingSelection> SelectedOptions { get; set; }
    }
}
