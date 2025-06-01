using CurrencyConverter.Services.Cache.Eviction.Strategy;
using CurrencyConverter.Services.CacheSettings.CacheSettingsView;

namespace CurrencyConverter.Services.Cache.Eviction.Factory
{
    /// <summary>
    /// Factory class responsible for creating appropriate cache eviction strategy based on current cache settings.
    /// </summary>
    public class CacheEvictionStrategyFactory<TKey, TValue> : ICacheEvictionStrategyFactory<TKey, TValue>
    {
        private readonly ICacheSettingsViewService _cacheSettingsViewService;

        public CacheEvictionStrategyFactory(ICacheSettingsViewService cacheSettingsViewService)
        {
            _cacheSettingsViewService = cacheSettingsViewService;
        }

        /// <summary>
        /// Creates and returns the appropriate cache eviction strategy based on current application settings.
        /// Returns a time-based, size-based, or not-specified eviction strategy depending on configured options.
        /// </summary>
        public ICacheEvictionStrategy<TKey, TValue> GetCurrentStrategy()
        {
            if (_cacheSettingsViewService.SelectedEvictionStrategy == EvictionStrategy.TimeBased 
                && _cacheSettingsViewService.MaxAgeMinutes.HasValue)
                return new TimeBasedEvictionStrategy<TKey, TValue>(_cacheSettingsViewService.MaxAgeMinutes.Value);

            if (_cacheSettingsViewService.SelectedEvictionStrategy == EvictionStrategy.SizeBased 
                && _cacheSettingsViewService.MaxElements.HasValue)
                return new SizeBasedEvictionStrategy<TKey, TValue>(_cacheSettingsViewService.MaxElements.Value);

            // default behavior if eviction is not provided
            return new NotSpecifiedEvictionStrategy<TKey, TValue>();
        }
    }
}
