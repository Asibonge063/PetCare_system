using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCare_system.Models
{

    public enum PetType
    {
        Dog,
        Cat,
        Other
    }

    [Flags]
    public enum AddOnServices
    {
        None = 0,
        FleaTreatment = 1,
        DeShedding = 2,
        TeethCleaning = 4
    }

    public enum ServiceType
    {
        BasicGrooming,
        FullGroomingPackage,
        DeluxeSpaTreatment,
        PuppyKittenFirstVisit,
        SpecialtyCuts
    }

    public enum BookingStatus
    {
        Pending,
        Confirmed,
        Completed,
        Cancelled
    }

    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed,
        Refunded
    }

    public enum PaymentMethod
    {
        CreditCard,
        PayPal,
        BankTransfer,
        Cash
    }
    public class Spar_Grooming
    {

        [Key]
        public int BookingId { get; set; }

        // Owner Information
        [Required(ErrorMessage = "Owner name is required")]
        [DisplayName("Owner Name")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
        public string OwnerName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [DisplayName("Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        // Pet Information
        [Required(ErrorMessage = "Pet name is required")]
        [DisplayName("Pet's Name")]
        [StringLength(50, ErrorMessage = "Pet name cannot be longer than 50 characters")]
        public string PetName { get; set; }

        [Required(ErrorMessage = "Pet type is required")]
        [DisplayName("Pet Type")]
        public PetType PetType { get; set; }

        [Required(ErrorMessage = "Breed is required")]
        [StringLength(50, ErrorMessage = "Breed cannot be longer than 50 characters")]
        public string Breed { get; set; }

        // Service Information
        [Required(ErrorMessage = "Service is required")]
        [DisplayName("Service Type")]
        public ServiceType ServiceType { get; set; }

        [DisplayName("Add-on Services")]
        public AddOnServices AddOnServices { get; set; } = AddOnServices.None;

        [Required(ErrorMessage = "Duration is required")]
        [Range(1, 3, ErrorMessage = "Duration must be between 1 and 3 hours")]
        public double DurationHours { get; set; }

        [Required(ErrorMessage = "Booking date is required")]
        [DisplayName("Preferred Date")]
        [DataType(DataType.Date)]
        [FutureDate(ErrorMessage = "Booking date must be in the future")]
        public DateTime PreferredDate { get; set; }

        [Required(ErrorMessage = "Booking time is required")]
        [DisplayName("Preferred Time")]
        public string PreferredTime { get; set; }

        [DisplayName("Special Instructions")]
        [StringLength(500, ErrorMessage = "Special instructions cannot be longer than 500 characters")]
        public string SpecialInstructions { get; set; }

        [DisplayName("Booking Date")]
        public DateTime BookingDate { get; set; } = DateTime.Now;

        // Add to Spar_Grooming class

        [DisplayName("Payment Status")]
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

        [DisplayName("Payment Date")]
        public DateTime? PaymentDate { get; set; }

        [DisplayName("Payment Method")]
        public PaymentMethod? PaymentMethod { get; set; }

        [DisplayName("Transaction ID")]
        [StringLength(50)]
        public string TransactionId { get; set; }

        // Calculated properties
        [DisplayName("Base Price")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal BasePrice => (ServiceType == ServiceType.PuppyKittenFirstVisit) ? 50m : (decimal)DurationHours * 50m;
        [NotMapped]
        [DisplayName("Selected Pet")]
        public int? SelectedPetId { get; set; }
        [DisplayName("Add-on Price")]
        [DisplayFormat(DataFormatString = "{0:C}")]


        public decimal AddOnPrice
        {
            get
            {
                decimal total = 0;
                if (AddOnServices.HasFlag(AddOnServices.FleaTreatment)) total += 25;
                if (AddOnServices.HasFlag(AddOnServices.DeShedding)) total += 30;
                if (AddOnServices.HasFlag(AddOnServices.TeethCleaning)) total += 50;
                return total;
            }
        }

        [DisplayName("Total Price")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal TotalPrice => BasePrice + AddOnPrice;

        // Status tracking
        public BookingStatus Status { get; set; } = BookingStatus.Pending;
    }

    // Custom validation attribute for future dates
    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is DateTime date)
            {
                return date.Date >= DateTime.Today;
            }
            return false;
        }
    }

}
