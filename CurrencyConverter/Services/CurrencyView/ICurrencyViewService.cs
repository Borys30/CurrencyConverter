using CurrencyConverter.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CurrencyConverter.Services.CurrencyView
{
    /// <summary>
    /// Defines the interface for currency view service operations that provide
    /// presentation-specific currency conversion functionality.
    /// </summary>
    public interface ICurrencyViewService
    {
        /// <summary>
        /// Retrieves a list of available currencies formatted for display.
        /// Returns an enumerable collection of currency strings.
        /// </summary>
        Task<IEnumerable<string>> GetAvailableCurrenciesAsync();

        /// <summary>
        /// Retrieves the latest conversion rate for the specified currencies and amount,
        /// formatting the result for display.
        /// Returns a LatestConversionResult object with formatted conversion information.
        /// </summary>
        Task<LatestConversionResult> GetLatestConversionRateAsync(string sourceCurrency,
            string targetCurrency, string amount);

        /// <summary>
        /// Validates if a conversion can be performed with the given parameters.
        /// Returns true if the parameters are valid for conversion, false otherwise.
        /// </summary>
        bool CanPerformConversion(string sourceCurrency, string targetCurrency, string amount);

        /// <summary>
        /// Handles exceptions by displaying an error notification to the user.
        /// </summary>
        void HandleError(string message, Exception exceptionDetails);

    }
}
