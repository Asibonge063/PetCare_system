using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCare_system.Models
{
    public class DayCare
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Membership")]
        public int MembershipId { get; set; }

        [Required]
        [Display(Name = "Pet")]
        public int PetId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "Care Type")]
        public string CareType { get; set; }

        [Required]
        [Display(Name = "Check-In Date/Time")]
        public DateTime CheckInDate { get; set; }

        [Display(Name = "Check-Out Date/Time")]
        public DateTime? CheckOutDate { get; set; }

        [Display(Name = "Special Instructions")]
        [DataType(DataType.MultilineText)]
        public string SpecialInstructions { get; set; }

        // Navigation properties
        [ForeignKey("MembershipId")]
        public virtual Membership Membership { get; set; }

        [ForeignKey("PetId")]
        public virtual Pet Pet { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
    }
}
