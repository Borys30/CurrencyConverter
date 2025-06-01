using CurrencyConverter.Commands;
using CurrencyConverter.Core.Collections;
using CurrencyConverter.Core.Common;
using CurrencyConverter.Domain.Models;
using CurrencyConverter.Services.HistoricalRatesView;
using CurrencyConverter.ViewModel.Base;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CurrencyConverter.ViewModel
{
    /// <summary>
    /// View model responsible for managing historical currency conversion rates.
    /// Provides functionality to fetch and display historical rates based on date ranges
    /// and currency information.
    /// </summary>
    public class HistoricalRatesViewModel : ViewModelBase
    {
        private readonly IHistoricalRatesViewService _historicalRatesViewService;

        private DateTime? _startDate;
        private DateTime? _endDate;

        public BulkObservableCollection<HistoricalRate> HistoricalRates { get; } = new BulkObservableCollection<HistoricalRate>();

        public DateTime? StartDate
        {
            get => _startDate;
            set { SetProperty(ref _startDate, value); UpdateHistoricalRates(); }
        }

        public DateTime? EndDate
        {
            get => _endDate;
            set { SetProperty(ref _endDate, value); UpdateHistoricalRates(); }
        }

        public ICommand LoadHistoricalRatesCommand { get; }

        // Information from the currency ViewModel
        private string _sourceCurrency;
        private string _targetCurrency;
        private string _amount;

        /// <summary>
        /// Initializes a new instance of the HistoricalRatesViewModel with the specified historical rates service.
        /// Sets up the LoadHistoricalRatesCommand with appropriate execution and can-execute conditions.
        /// </summary>
        public HistoricalRatesViewModel(IHistoricalRatesViewService historicalService)
        {
            _historicalRatesViewService = historicalService;
            LoadHistoricalRatesCommand = new AsyncRelayCommand(LoadHistoricalRatesAsync);
        }

        /// <summary>
        /// Updates the current currency information used for historical rate lookups.
        /// Takes source currency, target currency and amount, then triggers an update of historical rates.
        /// </summary>
        public void UpdateCurrencyInformation(string sourceCurrency, string targetCurrency, string amount)
        {
            _sourceCurrency = sourceCurrency;
            _targetCurrency = targetCurrency;
            _amount = amount;

            UpdateHistoricalRates();
        }

        /// <summary>
        /// Updates the historical rates based on current parameters.
        /// Executes the load command if conditions are valid, otherwise clears the historical rates collection.
        /// </summary>
        private void UpdateHistoricalRates()
        {
            if (CanLoadHistoricalRates())
                LoadHistoricalRatesCommand.Execute(null);
            else
                HistoricalRates.Clear();
        }

        /// <summary>
        /// Asynchronously loads historical rates based on current currency information and date range.
        /// Clears existing rates, validates inputs, fetches rates from the service, and populates the collection.
        /// Handles any exceptions that may occur during the operation.
        /// </summary>
        private async Task LoadHistoricalRatesAsync()
        {
            try
            {
                HistoricalRates.Clear();

                if (!_historicalRatesViewService.ValidateHistoricalRateRequest(
                    _sourceCurrency, _targetCurrency, _amount, StartDate, EndDate))
                    return;

                var historicalRates = await _historicalRatesViewService.GetHistoricalRatesAsync(
                    _sourceCurrency, _targetCurrency, _amount, StartDate.Value, EndDate.Value);

                HistoricalRates.AddRange(historicalRates);
            }
            catch (Exception exception)
            {
                _historicalRatesViewService.HandleError(Constants.ErrorTitle, exception);
            }

        }

        /// <summary>
        /// Determines whether historical rates can be loaded with current parameters.
        /// Returns true if the service validates that all required inputs are present and valid.
        /// </summary>
        private bool CanLoadHistoricalRates()
        {
            return _historicalRatesViewService.CanLoadHistoricalRates(_sourceCurrency, _targetCurrency, _amount, StartDate, EndDate);
        }

    }
}
