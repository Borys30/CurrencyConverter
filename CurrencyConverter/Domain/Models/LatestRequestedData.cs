using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CurrencyConverter.Domain.Models
{
    /// <summary>
    /// Represents the response data from a current currency rate API request.
    /// </summary>
    public class LatestRequestedData
    {
        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("base")]
        public string SourceCurrency { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("rates")]
        public Dictionary<string, decimal> Rates { get; set; }
    }
}
