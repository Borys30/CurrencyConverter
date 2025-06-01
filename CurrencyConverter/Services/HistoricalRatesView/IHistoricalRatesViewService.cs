using CurrencyConverter.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CurrencyConverter.Services.HistoricalRatesView
{
    /// <summary>
    /// Defines the interface for historical rates view service operations that provide 
    /// functionality for retrieving and validating historical currency conversion rates.
    /// </summary>
    public interface IHistoricalRatesViewService
    {
        /// <summary>
        /// Retrieves historical conversion rates for the specified parameters.
        /// Returns a collection of HistoricalRate objects formatted for display.
        /// </summary>
        Task<IEnumerable<HistoricalRate>> GetHistoricalRatesAsync(
            string sourceCurrency, string targetCurrency, string amount,
            DateTime startDate, DateTime endDate);

        /// <summary>
        /// Determines if historical rates can be loaded with the given parameters.
        /// Returns true if historical rates can be loaded, false otherwise.
        /// </summary>
        bool CanLoadHistoricalRates(
            string sourceCurrency, string targetCurrency, string amount,
            DateTime? startDate, DateTime? endDate);

        /// <summary>
        /// Validates if the specified parameters are valid for a historical rate request.
        /// Returns true if the parameters are valid, false otherwise.
        /// </summary>
        bool ValidateHistoricalRateRequest(
            string sourceCurrency, string targetCurrency, string amount,
            DateTime? startDate, DateTime? endDate);

        /// <summary>
        /// Handles exceptions by displaying an error notification to the user.
        /// </summary>
        void HandleError(string message, Exception exception);
    }
}
