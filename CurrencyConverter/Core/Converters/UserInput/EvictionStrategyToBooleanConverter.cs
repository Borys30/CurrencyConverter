using CurrencyConverter.Services.Cache.Eviction;
using System;
using System.Globalization;
using System.Windows.Data;

namespace CurrencyConverter.Core.Converters.UserInput
{
    /// <summary>
    /// Converter that transforms EvictionStrategy enum values to boolean values for use in UI binding.
    /// </summary>
    public class EvictionStrategyToBooleanConverter : IValueConverter
    {
        /// <summary>
        /// Converts an EvictionStrategy value to a boolean by comparing it to the expected strategy name.
        /// Returns true if the strategy is not NotSpecified and its string representation matches the expected parameter.
        /// Returns false otherwise or if inputs are not of expected types.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is EvictionStrategy strategy && parameter is string expected)
            {
                return strategy != EvictionStrategy.NotSpecified && strategy.ToString() == expected;
            }

            return false;
        }

        /// <summary>
        /// Converts a boolean value back to an EvictionStrategy value.
        /// This method is not implemented.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
