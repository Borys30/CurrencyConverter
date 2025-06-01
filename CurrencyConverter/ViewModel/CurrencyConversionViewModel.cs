using CurrencyConverter.Commands;
using CurrencyConverter.Core.Collections;
using CurrencyConverter.Core.Common;
using CurrencyConverter.Services.CurrencyView;
using CurrencyConverter.ViewModel.Base;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CurrencyConverter.ViewModel
{
    /// <summary>
    /// View model responsible for handling currency conversion operations, including fetching available currencies
    /// and performing real-time currency conversions based on user selections.
    /// </summary>
    public class CurrencyConversionViewModel : ViewModelBase
    {
        private readonly ICurrencyViewService _currencyViewService;

        private string _selectedSourceCurrency;
        private string _selectedTargetCurrency;
        private string _amount;
        private string _latestConvertedAmount;
        private string _latestConversionDate;

        // custom field to track id of the request regarding updating property
        private int _latestRequestId = 0;

        public BulkObservableCollection<string> Currencies { get; } = new BulkObservableCollection<string>();

        public string SelectedSourceCurrency
        {
            get => _selectedSourceCurrency;
            set 
            {
                if (SetProperty(ref _selectedSourceCurrency, value))
                {
                    int currentRequestId = Interlocked.Increment(ref _latestRequestId);
                    _ = UpdateLatestConversion(currentRequestId);
                }
            }
        }

        public string SelectedTargetCurrency
        {
            get => _selectedTargetCurrency;
            set 
            {
                if (SetProperty(ref _selectedTargetCurrency, value))
                {
                    int currentRequestId = Interlocked.Increment(ref _latestRequestId);
                    _ = UpdateLatestConversion(currentRequestId);
                }
            }
        }

        public string Amount
        {
            get => _amount;
            set 
            {
                if (SetProperty(ref _amount, value))
                {
                    int currentRequestId = Interlocked.Increment(ref _latestRequestId);
                    _ = UpdateLatestConversion(currentRequestId);
                }
            }
        }

        public string LatestConvertedAmount
        {
            get => _latestConvertedAmount;
            set => SetProperty(ref _latestConvertedAmount, value);
        }

        public string LatestConversionDate
        {
            get => _latestConversionDate;
            set => SetProperty(ref _latestConversionDate, value);
        }

        public ICommand LoadCurrenciesCommand { get; }

        public CurrencyConversionViewModel(ICurrencyViewService currencyViewService)
        {
            _currencyViewService = currencyViewService;
            LoadCurrenciesCommand = new AsyncRelayCommand(LoadCurrenciesAsync);
        }

        /// <summary>
        /// Asynchronously loads all available currencies from the currency service.
        /// Clears existing currencies and populates the Currencies collection with the results.
        /// Handles any exceptions that may occur during the operation.
        /// </summary>
        private async Task LoadCurrenciesAsync()
        {
            try
            {
                Currencies.Clear();

                var currenciesInfo = await _currencyViewService.GetAvailableCurrenciesAsync();

                Currencies.AddRange(currenciesInfo.Select(currencyInfo => currencyInfo.ToString()));
            }
            catch (Exception exception)
            {
                _currencyViewService.HandleError(Constants.ErrorTitle, exception);
            }
        }

        /// <summary>
        /// Updates the latest conversion results based on current selections.
        /// When valid selections are made, fetches and displays the latest conversion rate and date.
        /// Sets default values if the conversion cannot be performed or if an error occurs.
        /// </summary>
        private async Task UpdateLatestConversion(int currentRequestId)
        {
            try
            {
                if (!_currencyViewService.CanPerformConversion(SelectedSourceCurrency, SelectedTargetCurrency, Amount))
                {
                    SetDefaultConversionResults();
                    return;
                }

                // verify current request is still the newest using a thread-safe read
                if (currentRequestId != Interlocked.CompareExchange(ref _latestRequestId, _latestRequestId, _latestRequestId))
                    return;

                var result = await _currencyViewService.GetLatestConversionRateAsync(
                    SelectedSourceCurrency, SelectedTargetCurrency, Amount);
                
                LatestConvertedAmount = result.ConvertedAmount;
                LatestConversionDate = result.ConversionDate;
            }
            catch (Exception exception)
            {
                // verify current request is still the newest using a thread-safe read
                if (currentRequestId == Interlocked.CompareExchange(ref _latestRequestId, _latestRequestId, _latestRequestId))
                {
                    SetDefaultConversionResults();
                    _currencyViewService.HandleError(Constants.ErrorTitle, exception);
                }
            }
        }

        /// <summary>
        /// Resets the conversion results to their default values.
        /// Used when conversion cannot be performed or when an error occurs.
        /// </summary>
        private void SetDefaultConversionResults()
        {
            LatestConvertedAmount = default;
            LatestConversionDate = default;
        }
    }
}
