using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

   
    using System.ComponentModel.DataAnnotations;

    namespace PetCare_system.Models
    {
        public class PetPatientViewModel
        {
            public int Id { get; set; }

            [Display(Name = "Pet Name")]
            public string Name { get; set; }

            [Display(Name = "Animal Type")]
            public string Type { get; set; }

            public string Breed { get; set; }

            [Display(Name = "Age")]
            public int Age { get; set; }

            [Display(Name = "Date of Birth")]
            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
            public DateTime DateOfBirth { get; set; }

            public string PicturePath { get; set; }

            [Display(Name = "Owner")]
            public string OwnerName { get; set; }

            // Helper property for image display
            public string ImageUrl
            {
                get
                {
                    if (!string.IsNullOrEmpty(PicturePath))
                    {
                        return PicturePath.StartsWith("~") ? VirtualPathUtility.ToAbsolute(PicturePath) : PicturePath;
                    }
                    return "/Content/Images/default-pet.jpg";
                }
            }
        }
    }