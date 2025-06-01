using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CurrencyConverter.Domain.Models
{
    /// <summary>
    /// Represents the response data from a historical currency rates API request.
    /// </summary>
    public class HistoricalRequestedData
    {
        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("base")]
        public string SourceCurrency { get; set; }

        [JsonProperty("start_date")]
        public DateTime StartDate { get; set; }

        [JsonProperty("end_date")]
        public DateTime EndDate { get; set; }

        [JsonProperty("rates")]
        public Dictionary<string, Dictionary<string, decimal>> Rates { get; set; }
    }
}
