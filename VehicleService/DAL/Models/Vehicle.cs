using System.ComponentModel.DataAnnotations;

namespace VehicleService.DAL.Models
{
    public class Vehicle
    {
        [Key]
        [Required]
        public string VIN { get; set; } 

        [Required] 
        public string ManufacturerName { get; set; }
        
        [Required] 
        public string Description { get; set; }

        [Required]
        public int HorsePower { get; set; }
        
        [Required] [StringLength(100)] 
        public string ModelName { get; set; }
        [Required] 
        public int ModelYear { get; set; }
        [Required]
        public decimal PurchasePrice { get; set; }

        [Required] [StringLength(50)] 
        public string FuelType { get; set; }
    }
}