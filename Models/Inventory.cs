using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PetCare_system.Models
{

    public enum InveType
    {
        Flea_and_Tick_Prevention,
        Heartworm_Prevention,
        Pain_Relief,
        Allergy_Relief,
        Antibiotics

    }
    public class Inventory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InventoryId { get; set; }
        public string MedicationName { get; set; }
        public InveType InventoryType { get; set; }
        public int InventoryQuantity { get; set; }
        public string MedDiscription { get; set; }
    }
}