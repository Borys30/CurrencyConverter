using CurrencyConverter.Domain.Models;
using CurrencyConverter.Services.Cache;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CurrencyConverter.Services.Currency
{
    /// <summary>
    /// Implements a caching layer over the currency service to improve performance by storing 
    /// previously requested historical rate data.
    /// </summary>
    public class CachedCurrencyService : ICurrencyService
    {
        private readonly ICurrencyService _underlyingCurrencyService;
        private readonly ICacheService<HistoricalRateQuery, HistoricalRequestedData> _cacheService;

        /// <summary>
        /// Initializes a new instance of the CachedCurrencyService with the underlying currency service 
        /// and cache service for historical rate queries.
        /// </summary>
        public CachedCurrencyService(ICurrencyService underlyingCurrencyService, ICacheService<HistoricalRateQuery, HistoricalRequestedData> cacheService)
        {
            _underlyingCurrencyService = underlyingCurrencyService;
            _cacheService = cacheService;
        }

        /// <summary>
        /// Retrieves a list of available currencies by delegating to the underlying currency service.
        /// Returns a list of currency information objects.
        /// </summary>
        public async Task<List<CurrencyInfo>> GetAvailableCurrenciesAsync()
        {
            return await _underlyingCurrencyService.GetAvailableCurrenciesAsync();
        }

        /// <summary>
        /// Retrieves historical conversion rates for the specified query, using cache when available.
        /// Returns historical requested data either from cache or from the underlying service.
        /// </summary>
        public async Task<HistoricalRequestedData> GetHistoricalConversionRatesAsync(HistoricalRateQuery historicalRateQuery)
        {
            if (_cacheService.TryGet(historicalRateQuery, out var cachedHistoricalRequestedData))
            {
                return cachedHistoricalRequestedData;
            }

            var historicalRequestedData = await _underlyingCurrencyService.GetHistoricalConversionRatesAsync(historicalRateQuery);

            _cacheService.AddOrUpdate(historicalRateQuery, historicalRequestedData);

            return historicalRequestedData;
        }

        /// <summary>
        /// Retrieves the latest conversion rate for the specified currencies and amount by delegating to the underlying service.
        /// Returns the latest conversion rate data.
        /// </summary>
        public async Task<LatestRequestedData> GetLatestConversionRateAsync(string sourceCurrency, string targetCurrency, string sourceAmount)
        {
            return await _underlyingCurrencyService.GetLatestConversionRateAsync(sourceCurrency, targetCurrency, sourceAmount);
        }
    }
}
