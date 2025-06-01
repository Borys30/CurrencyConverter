namespace CurrencyConverter.Domain.Models
{
    /// <summary>
    /// Represents the result of a current currency conversion for display in the UI.
    /// </summary>
    public class LatestConversionResult
    {
        public string ConvertedAmount { get; set; }
        public string ConversionDate { get; set; }
    }
}
