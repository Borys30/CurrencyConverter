using CurrencyConverter.Services.Cache.Eviction;
using CurrencyConverter.Services.CacheSettings.Persistence;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CurrencyConverter.Services.CacheSettings.CacheSettingsView
{
    /// <summary>
    /// Service that manages cache settings with property change notifications and persistence.
    /// </summary>
    public class CacheSettingsViewService : ICacheSettingsViewService
    {
        private readonly ICacheSettingsPersistenceService _cacheSettingsPersistenceService;

        private int? _maxAgeMinutes;
        private int? _maxElements;
        private EvictionStrategy? _selectedEvictionStrategy;

        /// <summary>
        /// Initializes a new cache settings view service and loads saved settings from persistence.
        /// </summary>
        public CacheSettingsViewService(ICacheSettingsPersistenceService cacheSettingsPersistenceService)
        {
            _cacheSettingsPersistenceService = cacheSettingsPersistenceService;

            LoadSettings(cacheSettingsPersistenceService);
        }

        public int? MaxAgeMinutes
        {
            get => _maxAgeMinutes;
            set
            {
                if (_maxAgeMinutes != value)
                {
                    _maxAgeMinutes = value;
                    OnPropertyChanged();
                    SaveSettings();
                }
            }
        }

        public int? MaxElements
        {
            get => _maxElements;
            set
            {
                if (_maxElements != value)
                {

                    _maxElements = value;
                    OnPropertyChanged();
                    SaveSettings();
                }
            }
        }

        public EvictionStrategy? SelectedEvictionStrategy
        {
            get => _selectedEvictionStrategy;
            set
            {
                if (_selectedEvictionStrategy != value)
                {
                    _selectedEvictionStrategy = value;
                    OnPropertyChanged();
                    SaveSettings();
                }
            }
        }

        /// <summary>
        /// Loads cache settings from the persistence service.
        /// </summary>
        private void LoadSettings(ICacheSettingsPersistenceService cacheSettingsPersistenceService)
        {
            var savedSettings = cacheSettingsPersistenceService.Load();

            if (savedSettings != null)
            {
                _maxAgeMinutes = savedSettings.MaxAgeMinutes;
                _maxElements = savedSettings.MaxElements;
                _selectedEvictionStrategy = savedSettings.EvictionStrategy;
            }
        }

        /// <summary>
        /// Saves current cache settings to the persistence service.
        /// </summary>
        private void SaveSettings() 
        {
            var cacheSettingsData = new CacheSettingsData
            {
                MaxAgeMinutes = MaxAgeMinutes,
                MaxElements = MaxElements,
                EvictionStrategy = SelectedEvictionStrategy
            };

            _cacheSettingsPersistenceService.Save(cacheSettingsData);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
