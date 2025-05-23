using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCare_system.Models
{
    public class Pet_Adoption
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Adoption_Id { get; set; }
        public int Id { get; set; }

        [ForeignKey("Id")]
        public virtual Pet Pet { get; set; }

        // Adoption Details



        [Required(ErrorMessage = "Experience level is required")]
        [Display(Name = "Previous Pet Experience")]
        public string ExperienceLevel { get; set; } // None, Some, Experienced

        [Required(ErrorMessage = "Home description is required")]
        [StringLength(500, ErrorMessage = "Home description cannot exceed 500 characters")]
        [Display(Name = "Home Environment")]
        public string HomeDescription { get; set; }

        [Required(ErrorMessage = "Adoption reason is required")]
        [StringLength(1000, ErrorMessage = "Adoption reason cannot exceed 1000 characters")]
        [Display(Name = "Adoption Reason")]
        public string AdoptionReason { get; set; }

        // Status and Metadata
        [Display(Name = "Application Status")]
        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected

        [Required(ErrorMessage = "You must agree to the terms")]
        [Display(Name = "Agreement")]
        public bool HasAgreedToTerms { get; set; }

        [Display(Name = "Application Date")]
        public DateTime ApplicationDate { get; set; } = DateTime.Now;
        // Snapshot of Pet Info
        public string PetName { get; set; }
        public string PetType { get; set; }
        public string PetBreed { get; set; }
        public DateTime DateOfBirth { get; set; }

        // Snapshot of User Info
        public string AdopterFullName { get; set; }
        public string AdopterEmail { get; set; }
        public string AdopterPhone { get; set; }



        // Additional fields you might want to add later
        /*
        [Display(Name = "Landlord Approval")]
        public bool HasLandlordApproval { get; set; }
        
        [Display(Name = "Veterinarian Reference")]
        public string VeterinarianReference { get; set; }
        
        [Display(Name = "Household Members")]
        public int HouseholdMembers { get; set; }
        */

        // Relationship to user
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public decimal GetPetPrice(string petType)
        {
            decimal price = 0;

            switch (petType)
            {
                case "Dog":
                    price = 500;
                    break;
                case "Cat":
                    price = 300;
                    break;
                case "Rabbit":
                    price = 600;
                    break;
                case "Bird":
                    price = 100;
                    break;
                case "Other":
                    price = 1000;
                    break;
                default:
                    price = 0;
                    break;
            }

            return price;
        }

    }
}