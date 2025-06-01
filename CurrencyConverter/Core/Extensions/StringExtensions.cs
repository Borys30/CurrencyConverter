using System.Globalization;

namespace CurrencyConverter.Extensions
{
    /// <summary>
    /// Provides extension methods for string manipulation and parsing specific to currency-related operations.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Extracts the currency code "EUR" from a formatted string like "EUR - Euro".
        /// Returns string.Empty if the input is null or empty.
        /// </summary>
        public static string ExtractCurrencyCodeOrEmpty(this string formattedCurrency)
        {
            if (string.IsNullOrWhiteSpace(formattedCurrency))
                return string.Empty;

            var parts = formattedCurrency.Split('-');

            return parts.Length > 0 ? parts[0].Trim() : string.Empty;
        }

        /// <summary>
        /// Attempts to parse the input string to a decimal using invariant culture and number style.
        /// </summary>
        public static bool TryGetDecimal(this string input, out decimal value)
        {
            return decimal.TryParse(
                input,
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out value
            );
        }
    }
}
