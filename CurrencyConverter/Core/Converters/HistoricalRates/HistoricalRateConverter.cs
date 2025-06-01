using CurrencyConverter.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CurrencyConverter.Core.Converters.HistoricalRates
{
    /// <summary>
    /// Converts raw historical currency data into a structured format for display and analysis.
    /// </summary>
    public class HistoricalRateConverter : IHistoricalRateConverter
    {
        /// <summary>
        /// Transforms raw historical rate data into a list of HistoricalRate objects. Returns a list of formatted historical rates with dates, converted amounts, and target currencies.
        /// </summary>
        public List<HistoricalRate> Convert(HistoricalRequestedData historicalRequestedData)
        {
            return historicalRequestedData.Rates
                .Select(rate => new HistoricalRate
                {
                    ConversionRateDate = DateTime.TryParse(rate.Key, out var parsedDate)
                                         ? parsedDate :
                                         (DateTime?)null,
                    ConvertedAmount = rate.Value.First().Value,
                    TargetCurrency = rate.Value.First().Key
                })
                .ToList();
        }
    }
}
