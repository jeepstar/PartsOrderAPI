using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace PartsOrderAPI.Attributes
{
    public class ValidPatternAttribute : ValidationAttribute
    {
        private readonly string _pattern;

        public ValidPatternAttribute(string pattern)
        {
            _pattern = pattern;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var stringValue = value as string;
            if (string.IsNullOrEmpty(stringValue) || !Regex.IsMatch(stringValue, _pattern))
            {
                return new ValidationResult($"The value must match the pattern: {_pattern}");
            }

            return ValidationResult.Success;
        }
    }
}
