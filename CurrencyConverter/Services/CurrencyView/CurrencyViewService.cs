using CurrencyConverter.Core.Common;
using CurrencyConverter.Domain.Models;
using CurrencyConverter.Extensions;
using CurrencyConverter.Services.Currency;
using CurrencyConverter.Services.Notification;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyConverter.Services.CurrencyView
{
    /// <summary>
    /// Implements the ICurrencyViewService interface to provide currency conversion functionality
    /// with presentation-specific logic for the UI layer.
    /// </summary>
    public class CurrencyViewService : ICurrencyViewService
    {
        private readonly ICurrencyService _currencyService;
        private readonly INotificationService _notificationService;

        /// <summary>
        /// Initializes a new instance of the CurrencyViewService with the required currency service
        /// and notification service dependencies.
        /// </summary>
        public CurrencyViewService(ICurrencyService currencyService, INotificationService notificationService)
        {
            _currencyService = currencyService;
            _notificationService = notificationService;
        }

        /// <summary>
        /// Retrieves a list of available currencies formatted for display.
        /// Returns an enumerable collection of currency strings.
        /// </summary>
        public async Task<IEnumerable<string>> GetAvailableCurrenciesAsync()
        {
            var currenciesInfo = await _currencyService.GetAvailableCurrenciesAsync();
            return currenciesInfo.Select(currency => currency.ToString());
        }

        /// <summary>
        /// Retrieves the latest conversion rate for the specified currencies and amount,
        /// formatting the result for display.
        /// Returns a LatestConversionResult object with formatted conversion information.
        /// </summary>
        public async Task<LatestConversionResult> GetLatestConversionRateAsync(string sourceCurrency, string targetCurrency, string amount)
        {
            var latestRequestedData = await _currencyService.GetLatestConversionRateAsync(sourceCurrency, targetCurrency, amount);
            var result = new LatestConversionResult();

            if (latestRequestedData.Rates.TryGetValue(targetCurrency.ExtractCurrencyCodeOrEmpty(), out var conversionRate))
            {
                result.ConvertedAmount = conversionRate.ToString(Constants.NumberFormat, CultureInfo.InvariantCulture);
                result.ConversionDate = latestRequestedData.Date.ToString(Constants.DateFormat);
            }

            return result;
        }

        /// <summary>
        /// Validates if a conversion can be performed with the given parameters.
        /// Returns true if the parameters are valid for conversion, false otherwise.
        /// </summary>
        public bool CanPerformConversion(string sourceCurrency, string targetCurrency, string amount)
        {
            if (string.IsNullOrEmpty(sourceCurrency) || string.IsNullOrEmpty(targetCurrency))
                return false;

            if (string.IsNullOrWhiteSpace(amount) || !amount.TryGetDecimal(out var parsedAmount))
                return false;

            if (sourceCurrency == targetCurrency)
            {
                _notificationService.ShowWarning(Constants.WarningTitle, $"Source currency:'{sourceCurrency}'; Target currency;'{targetCurrency}'. Source and target currencies should be different. Latest converted results will not be updated. Please, correct the errors.");
                return false;
            }

            if (parsedAmount == 0)
                return false;

            return true;
        }

        /// <summary>
        /// Handles exceptions by displaying an error notification to the user.
        /// </summary>
        public void HandleError(string title, Exception exception)
        {
            _notificationService.ShowError(title, $"Error has occured when requesting data from Frankfurter API. Exception:{exception.Message}.");
        }
    }
}
