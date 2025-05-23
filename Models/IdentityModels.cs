using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace PetCare_system.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }


        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Cellphone number must be exactly 10 digits.")]
        public string CellphoneNumber { get; set; }

        [Required]
        [RegularExpression(@"^\d{13}$", ErrorMessage = "ID number must be exactly 13 digits.")]
        public string IdNumber { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        public DbSet<Pet> pets { get; set; }
        public DbSet<Vet_Consultations> Vet_Consultations { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<Training> Trainings { get; set; } 
        public DbSet<TrainingType> TrainingTypes {  get; set; }
        public DbSet<DayCare> DayCares { get; set; }
      
        public DbSet<Pet_Boarding> pet_Boardings { get; set; }
        public DbSet<PaymentForBoard> paymentForBoards { get; set; }
        public DbSet<Pet_Adoption> pet_Adoptions { get; set; }
        public DbSet<PaymentForAdoption> paymentForAdoptions { get; set; }
        public DbSet<BookingSpar> bookingSpars { get; set; }
        public DbSet<PaymentForSpar> paymentForSpars { get; set; }
        public DbSet<SpaService> spaServices { get; set; }
        public DbSet<Spar_Grooming> spar_Groomings { get; set; }
        public DbSet<Spar_GroomPayment> spar_GroomPayments { get; set; }
        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<Adoption> Adoptions { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }


        public System.Data.Entity.DbSet<PetCare_system.Models.GroomingStaff> GroomingStaffs { get; set; }
    }
}