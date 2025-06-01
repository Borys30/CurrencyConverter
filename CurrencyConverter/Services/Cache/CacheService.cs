using CurrencyConverter.Services.Cache.Eviction;
using CurrencyConverter.Services.Cache.Eviction.Factory;
using CurrencyConverter.Services.CacheSettings.CacheSettingsView;
using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace CurrencyConverter.Services.Cache
{
    /// <summary>
    /// Generic cache service implementation that provides thread-safe caching with configurable eviction strategies.
    /// </summary>
    public class CacheService<TKey, TValue> : ICacheService<TKey, TValue>
    {
        private readonly ConcurrentDictionary<TKey, CacheEntry<TValue>> _cache = new ConcurrentDictionary<TKey, CacheEntry<TValue>>();
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        private readonly PropertyChangedEventHandler _settingsChangedHandler;

        private readonly ICacheSettingsViewService _cacheSettingsService;
        private readonly ICacheEvictionStrategyFactory<TKey, TValue> _cacheEvictionStrategyFactory;

        /// <summary>
        /// Initializes a new cache service with the specified settings and eviction strategy factory.
        /// Sets up event handling to react to cache settings changes.
        /// </summary>
        public CacheService(ICacheSettingsViewService cacheSettingsService, ICacheEvictionStrategyFactory<TKey, TValue> cacheEvictionStrategyFactory)
        {
            _cacheSettingsService = cacheSettingsService;
            _cacheEvictionStrategyFactory = cacheEvictionStrategyFactory;

            _settingsChangedHandler = (sender, args) =>
            {
                if (args.PropertyName == nameof(ICacheSettingsViewService.MaxElements)
                || args.PropertyName == nameof(ICacheSettingsViewService.MaxAgeMinutes)
                || args.PropertyName == nameof(ICacheSettingsViewService.SelectedEvictionStrategy))
                { 
                    CleanUp(); 
                }
            };

            _cacheSettingsService.PropertyChanged += _settingsChangedHandler;
        }

        /// <summary>
        /// Attempts to retrieve a value from the cache by its key. 
        /// Returns true and sets the output value if found and not expired; otherwise returns false.
        /// For time-based strategy, checks if the entry is expired before returning it.
        /// </summary>
        public bool TryGet(TKey key, out TValue value)
        {
            if (_cache.TryGetValue(key, out var cacheEntry))
            {
                if (_cacheSettingsService.SelectedEvictionStrategy == EvictionStrategy.TimeBased 
                    && (DateTime.UtcNow - cacheEntry.RevisionDate).TotalMinutes > _cacheSettingsService.MaxAgeMinutes)
                {
                    Remove(key);
                    value = default;
                    return false;
                }

                value = cacheEntry.Value;
                return true;
            }

            value = default;
            return false;
        }

        /// <summary>
        /// Adds a new entry to the cache or updates an existing one, then runs the eviction strategy.
        /// Uses write locking to ensure thread safety.
        /// </summary>
        public void AddOrUpdate(TKey key, TValue value)
        {
            _lock.EnterWriteLock();
            try
            {
                _cache[key] = new CacheEntry<TValue>(value);
                CleanUp();
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Removes all entries from the cache.
        /// Uses write locking to ensure thread safety.
        /// </summary>
        public void Clear()
        {
            _lock.EnterWriteLock();

            try
            {
                _cache.Clear();
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Applies the currently configured eviction strategy to clean up the cache.
        /// </summary>
        public void CleanUp()
        {
            var evictionStrategy = _cacheEvictionStrategyFactory.GetCurrentStrategy();

            evictionStrategy.Evict(_cache);
        }

        public void Dispose()
        {
            if (_settingsChangedHandler != null && _cacheSettingsService != null)
                _cacheSettingsService.PropertyChanged -= _settingsChangedHandler;

            _lock.Dispose();
        }

        private void Remove(TKey key)
        {
            _cache.TryRemove(key, out _);
        }
    }
}
