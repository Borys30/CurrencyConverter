using System;

namespace CurrencyConverter.Services.CacheSettings.Persistence
{
    /// <summary>
    /// Interface for a service that handles persistence of cache settings.
    /// </summary>
    public interface ICacheSettingsPersistenceService : IDisposable
    {
        /// <summary>
        /// Loads cache settings from persistent storage.
        /// Returns the cached settings data or null if no settings are saved.
        /// </summary>
        CacheSettingsData Load();

        /// <summary>
        /// Saves cache settings to persistent storage.
        /// </summary>
        void Save(CacheSettingsData cacheSettingsData);
    }
}
