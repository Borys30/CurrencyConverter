using System;

namespace CurrencyConverter.Services.Cache
{
    /// <summary>
    /// Interface defining operations for a generic cache service.
    /// </summary>
    public interface ICacheService<TKey, TValue> : IDisposable
    {
        /// <summary>
        /// Attempts to retrieve a cached value by its key.
        /// Returns true if the value was found and not expired; otherwise false.
        /// </summary>
        bool TryGet(TKey key, out TValue value);

        /// <summary>
        /// Adds a new entry to the cache or updates an existing one.
        /// </summary>
        void AddOrUpdate(TKey key, TValue value);

        /// <summary>
        /// Removes all entries from the cache.
        /// </summary>
        void Clear();
    }
}
