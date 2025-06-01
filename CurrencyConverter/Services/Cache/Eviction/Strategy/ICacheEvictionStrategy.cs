using System.Collections.Concurrent;

namespace CurrencyConverter.Services.Cache.Eviction.Strategy
{
    /// <summary>
    /// Interface defining a strategy for cache eviction operations.
    /// </summary>
    public interface ICacheEvictionStrategy<TKey, TValue>
    {
        /// <summary>
        /// Performs eviction of entries from the cache according to a specific strategy.
        /// </summary>
        void Evict(ConcurrentDictionary<TKey, CacheEntry<TValue>> cache);
    }
}
