using CurrencyConverter.Core.Common;
using Newtonsoft.Json;
using System.IO;
using System.Threading;

namespace CurrencyConverter.Services.CacheSettings.Persistence
{
    /// <summary>
    /// Service responsible for persisting and retrieving cache settings to/from disk.
    /// </summary>
    public class CacheSettingsPersistenceService : ICacheSettingsPersistenceService
    {
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        /// <summary>
        /// Loads cache settings from disk.
        /// Returns the deserialized settings or null if the settings file doesn't exist.
        /// Uses read locking for thread safety.
        /// </summary>
        public CacheSettingsData Load()
        {
            _lock.EnterReadLock();

            try
            {
                if (!File.Exists(Constants.CacheSettingsFilePath))
                    return null;

                var json = File.ReadAllText(Constants.CacheSettingsFilePath);

                return JsonConvert.DeserializeObject<CacheSettingsData>(json);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        /// <summary>
        /// Saves cache settings to disk as a JSON file.
        /// Uses write locking for thread safety.
        /// </summary>
        public void Save(CacheSettingsData cacheSettingsData)
        {
            _lock.EnterWriteLock();

            try
            {
                var json = JsonConvert.SerializeObject(cacheSettingsData, Formatting.Indented);

                File.WriteAllText(Constants.CacheSettingsFilePath, json);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public void Dispose()
        {
            _lock.Dispose();
        }
    }
}
