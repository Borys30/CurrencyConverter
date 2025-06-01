using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace CurrencyConverter.Core.Validation
{
    /// <summary>
    /// Validates that user input text represents a valid numeric value with up to two decimal places.
    /// </summary>
    public class DecimalInputValidationRule : ValidationRule
    {
        /// <summary>
        /// Validates that the input string contains only numbers with optional decimal point and up to two decimal places.
        /// Returns ValidationResult.ValidResult for valid input or an error message for invalid input.
        /// </summary>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return ValidationResult.ValidResult;

            var inputValue = value.ToString();

            if (!Regex.IsMatch(inputValue, @"^[0-9]*(\.[0-9]{0,2})?$"))
                return new ValidationResult(false, "Please enter positive numbers only (with up to 2 decimal places using \'.\' separator).");

            return ValidationResult.ValidResult;
        }
    }
}
