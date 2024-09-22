using System;
using System.ComponentModel.DataAnnotations;

namespace PartsOrderAPI.Attributes
{
    public class ValidIntegerAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || !int.TryParse(value.ToString(), out _))
            {
                return new ValidationResult("The field must be a valid integer.");
            }

            return ValidationResult.Success;
        }
    }
}
