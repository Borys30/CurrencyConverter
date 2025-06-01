using System;
using System.Collections.Concurrent;
using System.Linq;

namespace CurrencyConverter.Services.Cache.Eviction.Strategy
{
    /// <summary>
    /// Eviction strategy that removes cache entries that have exceeded their maximum age.
    /// </summary>
    public class TimeBasedEvictionStrategy<TKey, TValue> : ICacheEvictionStrategy<TKey, TValue>
    {
        private readonly int _maxAgeMinutes;

        public TimeBasedEvictionStrategy(int maxAgeMinutes)
        {
            _maxAgeMinutes = maxAgeMinutes;
        }

        /// <summary>
        /// Removes all entries from the cache that are older than the configured maximum age in minutes.
        /// Age is determined by comparing the current time with each entry's revision date.
        /// </summary>
        public void Evict(ConcurrentDictionary<TKey, CacheEntry<TValue>> cache)
        {
            var utcNow = DateTime.UtcNow;

            var oldItems = cache.Where(item => (utcNow - item.Value.RevisionDate).TotalMinutes > _maxAgeMinutes)
                                .Select(item => item.Key)
                                .ToList();

            foreach (var key in oldItems)
                cache.TryRemove(key, out _);
        }
    }
}
