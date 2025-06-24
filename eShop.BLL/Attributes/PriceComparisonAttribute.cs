using System.ComponentModel.DataAnnotations;

namespace eShop.BLL.Attributes
{
    internal class PriceComparisonAttribute : ValidationAttribute
    {
        private readonly string _otherPropertyName;
        private readonly string _errorMessage;

        public PriceComparisonAttribute(string otherPropertyName, string errorMessage)
        {
            _otherPropertyName = otherPropertyName;
            _errorMessage = errorMessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var currentPrice = value as decimal?;
            var otherProperty = validationContext.ObjectType.GetProperty(_otherPropertyName);

            if (otherProperty == null)
                return new ValidationResult($"Unknown property: {_otherPropertyName}");

            var prevPrice = otherProperty.GetValue(validationContext.ObjectInstance) as decimal?;

            if (!prevPrice.HasValue || prevPrice.Value == 0)
                return ValidationResult.Success;

            if (currentPrice.Value >= prevPrice.Value)
            {
                return new ValidationResult(_errorMessage);
            }


            return ValidationResult.Success;
        }
    }
}
