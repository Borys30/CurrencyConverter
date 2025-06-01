using CurrencyConverter.Core.Common;
using System;

namespace CurrencyConverter.Domain.Models
{
    /// <summary>
    /// Represents a query for historical currency conversion rates over a date range.
    /// </summary>
    public class HistoricalRateQuery
    {
        public string SourceCurrency { get; set; }

        public string TargetCurrency { get; set; }

        public decimal Amount { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        /// <summary>
        /// Determines whether this query is equal to another object by comparing all properties.
        /// Returns true if the objects have identical property values, false otherwise.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (HistoricalRateQuery)obj;

            return SourceCurrency == other.SourceCurrency
                && TargetCurrency == other.TargetCurrency
                && Amount == other.Amount
                && StartDate.Date == other.StartDate.Date
                && EndDate.Date == other.EndDate.Date;
        }

        /// <summary>
        /// Generates a hash code based on all properties of the query for use in collections.
        /// Returns an integer hash code uniquely representing this combination of properties.
        /// </summary>
        public override int GetHashCode()
        {
            return $"{SourceCurrency}|{TargetCurrency}|{Amount}|{StartDate.Date.ToString(Constants.DateFormat)}|{EndDate.Date.ToString(Constants.DateFormat)}".GetHashCode();
        }
    }
}
