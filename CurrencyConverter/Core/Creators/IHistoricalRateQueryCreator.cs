using CurrencyConverter.Domain.Models;
using System;

namespace CurrencyConverter.Core.Creators
{
    /// <summary>
    /// Defines the interface for creating query objects for historical currency rate retrieval.
    /// </summary>
    public interface IHistoricalRateQueryCreator
    {
        /// <summary>
        /// Creates a structured query object from individual currency and date range parameters. Returns a fully configured HistoricalRateQuery object.
        /// </summary>
        HistoricalRateQuery Create(string sourceCurrency, string targetCurrency, string amount,
            DateTime startDate, DateTime endDate);
    }
}
