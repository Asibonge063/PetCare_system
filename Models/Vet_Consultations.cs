using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PetCare_system.Models
{

    public class Vet_Consultations
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Consult_Id { get; set; }

        [Required]
        [StringLength(250)]
        public string Consult_Description { get; set; }

        [Required]
        public DateTime Consult_Date { get; set; }

        [Required]
        public DateTime Consult_Time { get; set; }

        [Display(Name = "Has your pet shown any signs of illness?")]
        public bool HasSignsOfIllness { get; set; }

        [Display(Name = "Is your pet currently on any medications?")]
        public bool IsOnMedication { get; set; }

        [Display(Name = "Has your pet been vaccinated in the past year?")]
        public bool IsVaccinated { get; set; }

        [Display(Name = "Has your pet had any recent changes in eating habits?")]
        public bool HasChangedEatingHabits { get; set; }

        [Display(Name = "Has your pet experienced any unusual behaviors?")]
        public bool HasUnusualBehaviors { get; set; }

        [Display(Name = "Has your pet been dewormed in the past six months?")]
        public bool IsDewormed { get; set; }
        // --------------------------------------------------------------------------------//
        public bool InjectionAndVaccination { get; set; }
        public bool Neutering { get; set; }
        public bool GeneralCheckUp { get; set; }
        public bool Deworming { get; set; }
        public bool PregnancyOrUltraSound { get; set; }
        public bool AllergyTest { get; set; }

        // Store the image path for the consultation (optional)
        public string PicturePath { get; set; }

        // Foreign key for Pet
        public int PetId { get; set; }

        [ForeignKey("PetId")]
        public virtual Pet Pet { get; set; }

        // ✅ New Fields to Copy Pet Details into Vet_Consultations
        public string PetName { get; set; }
        public string PetType { get; set; }
        public string PetBreed { get; set; }
        public DateTime PetDateOfBirth { get; set; }
        public string PetPicturePath { get; set; }

        [Required]
        public string ConsultationType { get; set; }
        [Display(Name = "Doctor Feedback")]
        public string Feedback { get; set; }

        [Display(Name = "Doctor Availability")]
        public bool DoctorAvailability { get; set; }
        public bool PaymentStatus { get; set; }

        public int? DoctorId { get; set; }

        [ForeignKey("DoctorId")]
        public virtual Doctor Doctor { get; set; }

    }
}
