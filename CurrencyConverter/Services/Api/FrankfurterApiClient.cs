using CurrencyConverter.Core.Common;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CurrencyConverter.Services.Api
{
    /// <summary>
    /// Client for accessing the Frankfurter currency exchange API to retrieve currency data.
    /// </summary>
    public class FrankfurterApiClient : IFrankfurterApiClient
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the FrankfurterApiClient with the specified HttpClient.
        /// </summary>
        public FrankfurterApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(Constants.BaseFrankfurterApiUri);
        }

        /// <summary>
        /// Retrieves the list of available currencies from the API. Returns a JSON string containing currency codes and names.
        /// </summary>
        public async Task<string> GetAvailableCurrenciesJsonAsync()
        {
            var response = await _httpClient.GetAsync("v1/currencies");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Retrieves the latest conversion rate for the specified currencies and amount. Returns a JSON string with the conversion result.
        /// </summary>
        public async Task<string> GetLatestConversionRateJsonAsync(string fromCurrency, string toCurrency, decimal amount)
        {
            var response = await _httpClient.GetAsync($"v1/latest?from={fromCurrency}&to={toCurrency}&amount={amount}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Retrieves historical exchange rates for the specified date range and currencies. Returns a JSON string with historical conversion data.
        /// </summary>
        public async Task<string> GetHistoricalRatesJsonAsync(DateTime startDate, DateTime endDate, string fromCurrency, string toCurrency, decimal amount)
        {
            var range = $"{startDate.ToString(Constants.DateFormat)}..{endDate.ToString(Constants.DateFormat)}";

            var response = await _httpClient.GetAsync($"v1/{range}?from={fromCurrency}&to={toCurrency}&amount={amount}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}
