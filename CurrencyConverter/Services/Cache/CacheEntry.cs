using System;

namespace CurrencyConverter.Services.Cache
{
    /// <summary>
    /// Represents a cached item with its value and last revision timestamp.
    /// </summary>
    public class CacheEntry<T>
    {
        public T Value { get; set; }

        public DateTime RevisionDate { get; set; }

        /// <summary>
        /// Creates a new cache entry with the given value and sets the revision date to current UTC time.
        /// </summary>
        public CacheEntry(T value)
        {
            Value = value;
            RevisionDate = DateTime.UtcNow;
        }
    }
}
