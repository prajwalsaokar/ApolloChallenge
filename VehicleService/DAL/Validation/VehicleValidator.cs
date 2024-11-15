using System.ComponentModel.DataAnnotations;
using VehicleService.DAL.Models;
namespace VehicleService.DAL.Validation
{   
    public static class VehicleValidator
    {
        public static void ValidateVehicle(Vehicle vehicle)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(vehicle, null, null);
            
            if (!Validator.TryValidateObject(vehicle, validationContext, validationResults, true))
            {
                var errors = new List<string>();
                foreach (var validationResult in validationResults)
                {
                    errors.Add(validationResult.ErrorMessage);
                }

                if (errors.Count > 0)
                {
                    throw new VehicleValidationException(errors);
                }
            }
        }

    }
}