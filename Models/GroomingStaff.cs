using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PetCare_system.Models
{
    public class GroomingStaff
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GroomStaffId { get; set; }

        [Required]
        [StringLength(50)]
        public string Groom_Name { get; set; }

        [Required]  
        [StringLength(50)]
        public string Groom_Surname { get; set; }

        [Required]  
        [StringLength(50)]
        public string Groom_Email { get;set; }
        public virtual ICollection<Spar_Grooming> SparGroomings { get; set; }
        public GroomingStaff()
        {
            SparGroomings = new HashSet<Spar_Grooming>();
        }


    }
}