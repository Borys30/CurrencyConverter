using CurrencyConverter.Domain.Models;
using System.Collections.Generic;

namespace CurrencyConverter.Core.Converters.HistoricalRates
{
    /// <summary>
    /// Defines the interface for converting raw historical currency data into a structured format.
    /// </summary>
    public interface IHistoricalRateConverter
    {
        /// <summary>
        /// Transforms raw historical rate data into a list of HistoricalRate objects. Returns a list of formatted historical rates.
        /// </summary>
        List<HistoricalRate> Convert(HistoricalRequestedData historicalRequestedData);
    }
}
