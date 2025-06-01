using CurrencyConverter.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CurrencyConverter.Services.Currency
{
    /// <summary>
    /// Defines the interface for currency service operations, including retrieving available currencies
    /// and conversion rates for both latest and historical data.
    /// </summary>
    public interface ICurrencyService
    {
        /// <summary>
        /// Retrieves a list of available currencies supported by the service.
        /// Returns a list of CurrencyInfo objects.
        /// </summary>
        Task<List<CurrencyInfo>> GetAvailableCurrenciesAsync();

        /// <summary>
        /// Retrieves the latest conversion rate for the specified source and target currencies and amount.
        /// Returns a LatestRequestedData object containing the conversion information.
        /// </summary>
        Task<LatestRequestedData> GetLatestConversionRateAsync(string sourceCurrency, string targetCurrency, string sourceAmount);

        /// <summary>
        /// Retrieves historical conversion rates for the specified query parameters.
        /// Returns a HistoricalRequestedData object containing historical rate information.
        /// </summary>
        Task<HistoricalRequestedData> GetHistoricalConversionRatesAsync(HistoricalRateQuery historicalRateQuery);
    }
}
