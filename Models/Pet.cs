using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PetCare_system.Models
{
    public class Pet
    {


        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Type { get; set; }

        public string Breed { get; set; }


        public DateTime DateOfBirth { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        // New property for storing the image file path
        public string PicturePath { get; set; }
    }

}