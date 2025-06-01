using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CurrencyConverter.Extensions;
using CurrencyConverter.Domain.Models;
using CurrencyConverter.Services.Api;

namespace CurrencyConverter.Services.Currency
{
    /// <summary>
    /// Implements the ICurrencyService interface to provide currency conversion functionality 
    /// using the Frankfurter API for retrieving currency exchange rates.
    /// </summary>
    public class CurrencyService : ICurrencyService
    {
        private readonly IFrankfurterApiClient _frankfurterApiClient;

        /// <summary>
        /// Initializes a new instance of the CurrencyService with the Frankfurter API client.
        /// </summary>
        public CurrencyService(IFrankfurterApiClient frankfurterApiClient)
        {
            _frankfurterApiClient = frankfurterApiClient;
        }

        /// <summary>
        /// Retrieves a list of available currencies from the Frankfurter API.
        /// Returns a list of CurrencyInfo objects containing code and description for each currency.
        /// </summary>
        public async Task<List<CurrencyInfo>> GetAvailableCurrenciesAsync()
        {
            var json = await _frankfurterApiClient.GetAvailableCurrenciesJsonAsync();

            var deserializedResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            return deserializedResponse.Select(data => new CurrencyInfo { Code = data.Key, Description = data.Value })
                                       .ToList();
        }

        /// <summary>
        /// Retrieves the latest conversion rate for the specified currencies and amount.
        /// Returns a LatestRequestedData object containing the conversion rate information.
        /// </summary>
        public async Task<LatestRequestedData> GetLatestConversionRateAsync(string sourceCurrency, string targetCurrency, string sourceAmount)
        {
            if (!sourceAmount.TryGetDecimal(out var parsedAmount))
                throw new ArgumentException($"The provided amount is not valid:'{sourceAmount}'.");

            var fromCurrency = sourceCurrency.ExtractCurrencyCodeOrEmpty();
            var toCurrency = targetCurrency.ExtractCurrencyCodeOrEmpty();

            var hasDecimalPart = HasDecimalPart(parsedAmount);

            if (hasDecimalPart)
            {
                var unitData = await GetConversionRateAsync(fromCurrency, toCurrency, 1);


                var unitRate = unitData.Rates[toCurrency];

                var convertedAmount = parsedAmount * unitRate;

                unitData.Rates[toCurrency] = convertedAmount;
                unitData.Amount = parsedAmount;

                return unitData;
            }
            else
            {
                return await GetConversionRateAsync(fromCurrency, toCurrency, parsedAmount);
            }
        }

        /// <summary>
        /// Retrieves historical conversion rates for the specified query parameters.
        /// Returns a HistoricalRequestedData object containing historical rate information.
        /// </summary>
        public async Task<HistoricalRequestedData> GetHistoricalConversionRatesAsync(HistoricalRateQuery historicalRateQuery)
        {
            var fromCurrency = historicalRateQuery.SourceCurrency.ExtractCurrencyCodeOrEmpty();
            var toCurrency = historicalRateQuery.TargetCurrency.ExtractCurrencyCodeOrEmpty();

            var hasDecimalPart = HasDecimalPart(historicalRateQuery.Amount);

            if (hasDecimalPart)
            {
                var unitData = await GetHistoricalRatesAsync(historicalRateQuery.StartDate, historicalRateQuery.EndDate, fromCurrency, toCurrency, 1);

                ApplyMultiplierToHistoricalRates(unitData, historicalRateQuery.Amount);

                unitData.Amount = historicalRateQuery.Amount;

                return unitData;
            }
            else
            {
                return await GetHistoricalRatesAsync(historicalRateQuery.StartDate, historicalRateQuery.EndDate, fromCurrency, toCurrency, historicalRateQuery.Amount);
            }
        }

        /// <summary>
        /// Determines if a decimal amount has a fractional part.
        /// Returns true if the amount has a decimal component, false otherwise.
        /// </summary>
        private bool HasDecimalPart(decimal amount)
        {
            return amount != Math.Floor(amount);
        }

        /// <summary>
        /// Retrieves the latest conversion rate from the API for the specified currencies and amount.
        /// Returns a LatestRequestedData object with the conversion details.
        /// </summary>
        private async Task<LatestRequestedData> GetConversionRateAsync(string fromCurrency, string toCurrency, decimal amount)
        {
            var json = await _frankfurterApiClient.GetLatestConversionRateJsonAsync(fromCurrency, toCurrency, amount);

            return JsonConvert.DeserializeObject<LatestRequestedData>(json);
        }

        /// <summary>
        /// Retrieves historical rates from the API for the specified date range, currencies, and amount.
        /// Returns a HistoricalRequestedData object with the historical conversion data.
        /// </summary>
        private async Task<HistoricalRequestedData> GetHistoricalRatesAsync(DateTime startDate, DateTime endDate, string fromCurrency, string toCurrency, decimal amount)
        {
            var json = await _frankfurterApiClient.GetHistoricalRatesJsonAsync(startDate, endDate, fromCurrency, toCurrency, amount);

            return JsonConvert.DeserializeObject<HistoricalRequestedData>(json);
        }

        /// <summary>
        /// Applies a multiplier to all rates in the historical data.
        /// Modifies the data object directly with the scaled rates.
        /// </summary>
        private void ApplyMultiplierToHistoricalRates(HistoricalRequestedData data, decimal multiplier)
        {
            foreach (var date in data.Rates.Keys.ToList())
            {
                var rates = data.Rates[date];

                foreach (var currency in rates.Keys.ToList())
                    rates[currency] *= multiplier;
            }
        }
    }
}
