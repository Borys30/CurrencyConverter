using System.Collections.Concurrent;

namespace CurrencyConverter.Services.Cache.Eviction.Strategy
{
    /// <summary>
    /// A null-object pattern implementation of cache eviction strategy that performs no eviction.
    /// Used when no specific eviction strategy is configured.
    public class NotSpecifiedEvictionStrategy<TKey, TValue> : ICacheEvictionStrategy<TKey, TValue>
    {
        /// <summary>
        /// No-op implementation that doesn't evict any entries from the cache.
        /// </summary>
        public void Evict(ConcurrentDictionary<TKey, CacheEntry<TValue>> cache)
        {
            // Do nothing because there is no eviction strategy provided.
        }
    }
}
