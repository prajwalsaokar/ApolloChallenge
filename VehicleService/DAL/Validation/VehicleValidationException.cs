namespace VehicleService.DAL.Validation
{
    public class VehicleValidationException : Exception
    {
        public IEnumerable<string> ValidationErrors { get; }

        public VehicleValidationException(IEnumerable<string> validationErrors)
            : base("Vehicle validation failed. See validation errors for details.")
        {
            ValidationErrors = validationErrors;
        }

        public override string ToString()
        {
            return $"{Message} Errors: {string.Join("; ", ValidationErrors)}";
        }
    }
}