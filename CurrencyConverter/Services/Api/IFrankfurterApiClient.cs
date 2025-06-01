using System;
using System.Threading.Tasks;

namespace CurrencyConverter.Services.Api
{
    /// <summary>
    /// Defines the interface for accessing the Frankfurter currency exchange API.
    /// </summary>
    public interface IFrankfurterApiClient
    {
        /// <summary>
        /// Retrieves the list of available currencies from the API. Returns a JSON string containing currency codes and names.
        /// </summary>
        Task<string> GetAvailableCurrenciesJsonAsync();

        /// <summary>
        /// Retrieves the latest conversion rate for the specified currencies and amount. Returns a JSON string with the conversion result.
        /// </summary>
        Task<string> GetLatestConversionRateJsonAsync(string fromCurrency, string toCurrency, decimal amount);

        /// <summary>
        /// Retrieves historical exchange rates for the specified date range and currencies. Returns a JSON string with historical conversion data.
        /// </summary>
        Task<string> GetHistoricalRatesJsonAsync(DateTime startDate, DateTime endDate, string fromCurrency, string toCurrency, decimal amount);
    }
}
