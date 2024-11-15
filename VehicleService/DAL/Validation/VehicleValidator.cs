using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.VisualBasic.CompilerServices;
using VehicleService.DAL.Models;

namespace VehicleService.DAL.Validation
{
    public static class VehicleValidator
    {
        public static void ValidateVehicle(Vehicle vehicle)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(vehicle);

            // check the DataAnnotations in the model
            Validator.TryValidateObject(vehicle, validationContext, validationResults, validateAllProperties: true);

            if (vehicle.VIN.Length != 17)
            {
                validationResults.Add(new ValidationResult("VIN must be exactly 17 characters long.", new[] { nameof(vehicle.VIN) }));
            }

            if (vehicle.HorsePower <= 0)
            {
                validationResults.Add(new ValidationResult("Horse Power must be greater than 0.", new[] { nameof(vehicle.HorsePower) }));
            }

            if (vehicle.ModelName != null && vehicle.ModelName.Length > 100)
            {
                validationResults.Add(new ValidationResult("ModelName must not exceed 100 characters.", new[] { nameof(vehicle.ModelName) }));
            }

            if (vehicle.ModelYear <= 0)
            {
                validationResults.Add(new ValidationResult("ModelYear must be a valid positive integer.", new[] { nameof(vehicle.ModelYear) }));
            }

            if (vehicle.PurchasePrice <= 0)
            {
                validationResults.Add(new ValidationResult("Purchase price must be greater than 0.", new[] { nameof(vehicle.PurchasePrice) }));
            }

            if (vehicle.FuelType != null && vehicle.FuelType.Length > 50)
            {
                validationResults.Add(new ValidationResult("FuelType must not exceed 50 characters.", new[] { nameof(vehicle.FuelType) }));
            }

            if (validationResults.Any())
            {
                throw new VehicleValidationException(validationResults.Select(r => r.ErrorMessage).ToList());
            }
        }
    }
}
