using System.ComponentModel.DataAnnotations;

namespace VehicleService.DAL.Models
{
    public class Vehicle
    {
        [Key]
        [Required]
        [StringLength(17, MinimumLength = 17, ErrorMessage = "VIN must be 17 characters long.")]
        public string VIN { get; set; } // Unique constraint, case-insensitive

        [Required] [StringLength(100)] public string ManufacturerName { get; set; }

        [StringLength(500)] public string Description { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Horse Power must be greater than 0.")]
        public int HorsePower { get; set; }

        [Required] [StringLength(100)] public string ModelName { get; set; }

        [Required] public int ModelYear { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Purchase price must be greater than 0.")]
        public decimal PurchasePrice { get; set; }

        [Required] [StringLength(50)] public string FuelType { get; set; }
    }
}