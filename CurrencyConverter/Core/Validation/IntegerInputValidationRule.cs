using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CurrencyConverter.Core.Validation
{
    /// <summary>
    /// Validates that user input text represents a valid integer value.
    /// </summary>
    public class IntegerInputValidationRule : ValidationRule
    {
        /// <summary>
        /// Validates that the input string contains only digits (no decimal points).
        /// Returns ValidationResult.ValidResult for valid input or an error message for invalid input.
        /// </summary>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return ValidationResult.ValidResult;

            var inputValue = value.ToString();

            if (!Regex.IsMatch(inputValue, @"^[0-9]*$"))
                return new ValidationResult(false, "Please enter positive integer values only.");

            return ValidationResult.ValidResult;
        }
    }
}
