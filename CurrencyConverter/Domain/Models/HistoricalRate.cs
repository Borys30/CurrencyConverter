using System;

namespace CurrencyConverter.Domain.Models
{
    /// <summary>
    /// Represents a historical currency conversion rate for a specific date.
    /// </summary>
    public class HistoricalRate
    {
        public DateTime? ConversionRateDate { get; set; }
        public decimal ConvertedAmount { get; set; }
        public string TargetCurrency { get; set; }
    }
}
