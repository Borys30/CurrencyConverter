using CurrencyConverter.Services.Cache.Eviction;

namespace CurrencyConverter.Services.CacheSettings.Persistence
{
    /// <summary>
    /// Data transfer object representing cache settings that can be persisted.
    /// </summary>
    public class CacheSettingsData
    {
        public int? MaxAgeMinutes { get; set; }

        public int? MaxElements { get; set; }

        public EvictionStrategy? EvictionStrategy { get; set; }
    }
}
