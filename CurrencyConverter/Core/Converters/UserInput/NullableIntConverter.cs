using System;
using System.Globalization;
using System.Windows.Data;

namespace CurrencyConverter.Core.Converters.UserInput
{
    /// <summary>
    /// Provides two-way conversion between nullable integer values and their string representations for UI binding.
    /// </summary>
    public class NullableIntConverter : IValueConverter
    {
        /// <summary>
        /// Converts a nullable integer to its string representation. Returns the string value or empty string if null.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString() ?? string.Empty;
        }

        /// <summary>
        /// Converts a string back to a nullable integer. Returns the parsed integer, null if empty, or DoNothing if invalid.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stringValue = value as string;

            if (string.IsNullOrWhiteSpace(stringValue))
                return null;

            if (int.TryParse(stringValue, out int result))
                return result;

            return Binding.DoNothing;
        }
    }
}
