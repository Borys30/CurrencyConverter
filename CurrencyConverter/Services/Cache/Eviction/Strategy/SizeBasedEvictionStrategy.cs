using System.Collections.Concurrent;
using System.Linq;

namespace CurrencyConverter.Services.Cache.Eviction.Strategy
{
    /// <summary>
    /// Eviction strategy that keeps the cache size within a specified limit by removing oldest entries.
    /// </summary>
    public class SizeBasedEvictionStrategy<TKey, TValue> : ICacheEvictionStrategy<TKey, TValue>
    {
        private readonly int _maxElements;

        public SizeBasedEvictionStrategy(int maxElements)
        {
            _maxElements = maxElements;
        }

        /// <summary>
        /// Evicts entries from the cache when the total count exceeds the maximum allowed elements.
        /// Removes oldest entries first (based on their revision date) to maintain the cache size limit.
        /// </summary>
        public void Evict(ConcurrentDictionary<TKey, CacheEntry<TValue>> cache)
        {
            if (cache.Count > _maxElements)
            {
                var itemsToRemove = cache
                    .OrderBy(item => item.Value.RevisionDate)
                    .Take(cache.Count - _maxElements)
                    .Select(item => item.Key)
                    .ToList();

                foreach (var key in itemsToRemove)
                    cache.TryRemove(key, out _);
            }
        }
    }
}
