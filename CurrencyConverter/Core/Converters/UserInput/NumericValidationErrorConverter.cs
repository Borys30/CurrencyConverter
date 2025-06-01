using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace CurrencyConverter.Core.Converters.UserInput
{
    /// <summary>
    /// Converts validation errors into user-friendly error messages for display in the UI.
    /// </summary>
    public class NumericValidationErrorConverter : IValueConverter
    {
        /// <summary>
        /// Extracts the error content from a collection of validation errors. Returns the first error message or null if no errors.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ReadOnlyObservableCollection<ValidationError> errors && errors.Count > 0)
            {
                return errors[0].ErrorContent;
            }

            return null;
        }

        /// <summary>
        /// Not implemented as conversion is only needed in one direction.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
