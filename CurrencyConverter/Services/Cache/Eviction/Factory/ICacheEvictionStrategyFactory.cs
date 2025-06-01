using CurrencyConverter.Services.Cache.Eviction.Strategy;

namespace CurrencyConverter.Services.Cache.Eviction.Factory
{
    /// <summary>
    /// Interface defining a factory for creating cache eviction strategies.
    /// </summary>
    public interface ICacheEvictionStrategyFactory<TKey, TValue>
    {
        /// <summary>
        /// Gets the currently configured cache eviction strategy based on application settings.
        /// Returns an appropriate implementation of ICacheEvictionStrategy.
        /// </summary>
        ICacheEvictionStrategy<TKey, TValue> GetCurrentStrategy();
    }
}
