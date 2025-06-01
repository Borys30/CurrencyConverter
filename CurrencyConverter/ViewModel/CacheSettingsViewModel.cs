using CurrencyConverter.Services.Cache.Eviction;
using CurrencyConverter.Services.CacheSettings.CacheSettingsView;
using CurrencyConverter.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CurrencyConverter.ViewModel
{
    /// <summary>
    /// View model that provides cache settings configuration functionality,
    /// including maximum age, maximum elements, and eviction strategy.
    /// </summary>
    public class CacheSettingsViewModel : ViewModelBase
    {
        private readonly ICacheSettingsViewService _cacheSettingsViewService;

        public int? MaxAgeMinutes
        {
            get => _cacheSettingsViewService.MaxAgeMinutes;
            set
            {
                if (_cacheSettingsViewService.MaxAgeMinutes != value)
                {
                    _cacheSettingsViewService.MaxAgeMinutes = value;
                    OnPropertyChanged();
                }
            }
        }

        public int? MaxElements
        {
            get => _cacheSettingsViewService.MaxElements;
            set
            {
                if (_cacheSettingsViewService.MaxElements != value)
                {
                    _cacheSettingsViewService.MaxElements = value;
                    OnPropertyChanged();
                }
            }
        }

        public EvictionStrategy? SelectedEvictionStrategy
        {
            get => _cacheSettingsViewService.SelectedEvictionStrategy;
            set
            {
                if (_cacheSettingsViewService.SelectedEvictionStrategy != value)
                {
                    _cacheSettingsViewService.SelectedEvictionStrategy = value;
                    OnPropertyChanged();
                }
            }
        }

        public IEnumerable<EvictionStrategy> EvictionStrategies =>
            Enum.GetValues(typeof(EvictionStrategy)).Cast<EvictionStrategy>();

        public CacheSettingsViewModel(ICacheSettingsViewService cacheSettingsService)
        {
            _cacheSettingsViewService = cacheSettingsService;
            _cacheSettingsViewService.PropertyChanged += (sender, args) => OnPropertyChanged(args.PropertyName);
        }
    }
}
