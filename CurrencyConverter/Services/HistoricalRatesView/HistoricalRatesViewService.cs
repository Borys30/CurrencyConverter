using CurrencyConverter.Core.Common;
using CurrencyConverter.Core.Converters.HistoricalRates;
using CurrencyConverter.Core.Creators;
using CurrencyConverter.Domain.Models;
using CurrencyConverter.Extensions;
using CurrencyConverter.Services.Currency;
using CurrencyConverter.Services.Notification;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CurrencyConverter.Services.HistoricalRatesView
{
    /// <summary>
    /// Implements the IHistoricalRatesViewService interface to provide functionality for retrieving
    /// and validating historical currency conversion rates for display.
    /// </summary>
    public class HistoricalRatesViewService : IHistoricalRatesViewService
    {
        private readonly ICurrencyService _currencyService;
        private readonly IHistoricalRateQueryCreator _historicalRateQueryCreator;
        private readonly IHistoricalRateConverter _historicalRateConverter;
        private readonly INotificationService _notificationService;

        /// <summary>
        /// Initializes a new instance of the HistoricalRatesViewService with the required dependencies.
        /// </summary>
        public HistoricalRatesViewService(
            ICurrencyService currencyService,
            IHistoricalRateQueryCreator historicalRateQueryCreator,
            IHistoricalRateConverter historicalRateConverter,
            INotificationService notificationService)
        {
            _currencyService = currencyService;
            _historicalRateQueryCreator = historicalRateQueryCreator;
            _historicalRateConverter = historicalRateConverter;
            _notificationService = notificationService;
        }

        /// <summary>
        /// Retrieves historical conversion rates for the specified parameters.
        /// Returns a collection of HistoricalRate objects formatted for display.
        /// </summary>
        public async Task<IEnumerable<HistoricalRate>> GetHistoricalRatesAsync(string sourceCurrency, string targetCurrency, string amount, DateTime startDate, DateTime endDate)
        {
            var historicalRateQuery = _historicalRateQueryCreator.Create(sourceCurrency, targetCurrency, amount, startDate, endDate);

            var historicalRequestedData = await _currencyService.GetHistoricalConversionRatesAsync(historicalRateQuery);

            return _historicalRateConverter.Convert(historicalRequestedData);
        }

        /// <summary>
        /// Validates if the specified parameters are valid for a historical rate request.
        /// Returns true if the parameters are valid, false otherwise.
        /// </summary>
        public bool ValidateHistoricalRateRequest(string sourceCurrency, string targetCurrency, string amount, DateTime? startDate, DateTime? endDate)
        {
            if (sourceCurrency == targetCurrency)
                return false;

            if (!amount.TryGetDecimal(out var parsedAmount) || parsedAmount <= 0)
                return false;

            if (startDate > endDate || endDate > DateTime.UtcNow)
            {
                _notificationService.ShowWarning(Constants.WarningTitle, $"StartDate: '{startDate.Value.ToString(Constants.DateFormat)}'; EndDate: '{endDate.Value.ToString(Constants.DateFormat)}'. StartDate should be less or equal than EndDate and EndDate should be less or equal than UtcNow. Historical rate results will not be updated. Please, correct the errors.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Determines if historical rates can be loaded with the given parameters.
        /// Returns true if historical rates can be loaded, false otherwise.
        /// </summary>
        public bool CanLoadHistoricalRates(string sourceCurrency, string targetCurrency, string amount, DateTime? startDate, DateTime? endDate)
        {
            return !string.IsNullOrEmpty(sourceCurrency)
                && !string.IsNullOrEmpty(targetCurrency)
                && !string.IsNullOrWhiteSpace(amount)
                && amount.TryGetDecimal(out _)
                && startDate.HasValue
                && endDate.HasValue;
        }

        /// <summary>
        /// Handles exceptions by displaying an error notification to the user.
        /// </summary>
        public void HandleError(string message, Exception exception)
        {
            _notificationService.ShowError(Constants.ErrorTitle, $"{message}. Error has occured when loading historical rates. Exception:{exception.Message} ");
        }
    }
}
