using CurrencyConverter.Services.Cache.Eviction;
using System.ComponentModel;

namespace CurrencyConverter.Services.CacheSettings.CacheSettingsView
{
    /// <summary>
    /// Interface for a service that provides access to cache settings with property change notifications.
    /// </summary>
    public interface ICacheSettingsViewService : INotifyPropertyChanged
    {
        int? MaxAgeMinutes { get; set; }

        int? MaxElements { get; set; }

        EvictionStrategy? SelectedEvictionStrategy { get; set; }
    }
}
