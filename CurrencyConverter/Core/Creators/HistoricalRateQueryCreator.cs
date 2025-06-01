using CurrencyConverter.Core.Common;
using CurrencyConverter.Domain.Models;
using CurrencyConverter.Extensions;
using System;

namespace CurrencyConverter.Core.Creators
{
    /// <summary>
    /// Creates query objects for historical currency rate retrieval with proper parameter handling.
    /// </summary>
    public class HistoricalRateQueryCreator : IHistoricalRateQueryCreator
    {
        /// <summary>
        /// Creates a structured query object from individual currency and date range parameters. Returns a fully configured HistoricalRateQuery object.
        /// </summary>
        public HistoricalRateQuery Create(string sourceCurrency, string targetCurrency, string amount,
            DateTime startDate, DateTime endDate)
        {
            var parsedAmount = amount.TryGetDecimal(out var sourceAmount) ? sourceAmount : Constants.DefaultAmountValue;

            return new HistoricalRateQuery
            {
                SourceCurrency = sourceCurrency,
                TargetCurrency = targetCurrency,
                Amount = parsedAmount,
                StartDate = startDate,
                EndDate = endDate
            };
        }
    }
}
