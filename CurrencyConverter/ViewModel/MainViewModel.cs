using CurrencyConverter.ViewModel.Base;
using System.ComponentModel;

namespace CurrencyConverter.ViewModel
{
    /// <summary>
    /// Main view model that coordinates the application's primary components.
    /// Manages the relationship between currency conversion, historical rates, and cache settings.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        public CurrencyConversionViewModel CurrencyViewModel { get; }
        public HistoricalRatesViewModel HistoricalViewModel { get; }
        public CacheSettingsViewModel CacheSettingsViewModel { get; }

        /// <summary>
        /// Initializes a new instance of the MainViewModel with the required child view models.
        /// Sets up property change notifications to synchronize currency data between view models.
        /// </summary>
        public MainViewModel(CurrencyConversionViewModel currencyViewModel,
            HistoricalRatesViewModel historicalViewModel,
            CacheSettingsViewModel cacheSettingsViewModel)
        {
            CurrencyViewModel = currencyViewModel;
            HistoricalViewModel = historicalViewModel;
            CacheSettingsViewModel = cacheSettingsViewModel;

            CurrencyViewModel.PropertyChanged += OnCurrencyViewModelPropertyChanged;
        }

        /// <summary>
        /// Handles property change events from the CurrencyViewModel.
        /// When currency-related properties change, updates the HistoricalViewModel with the new values
        /// to ensure synchronized data across components.
        /// </summary>
        private void OnCurrencyViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CurrencyViewModel.SelectedSourceCurrency) ||
                e.PropertyName == nameof(CurrencyViewModel.SelectedTargetCurrency) ||
                e.PropertyName == nameof(CurrencyViewModel.Amount))
            {
                HistoricalViewModel.UpdateCurrencyInformation(
                    CurrencyViewModel.SelectedSourceCurrency,
                    CurrencyViewModel.SelectedTargetCurrency,
                    CurrencyViewModel.Amount);
            }
        }
    }
}
