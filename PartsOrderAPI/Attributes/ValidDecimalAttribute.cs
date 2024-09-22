using System;
using System.ComponentModel.DataAnnotations;

namespace PartsOrderAPI.Attributes
{
    public class ValidDecimalAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || !decimal.TryParse(value.ToString(), out _))
            {
                return new ValidationResult("The field must be a valid decimal number.");
            }

            return ValidationResult.Success;
        }
    }
}
