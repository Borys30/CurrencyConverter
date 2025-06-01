namespace CurrencyConverter.Domain.Models
{
    /// <summary>
    /// Represents currency information with code and description, used for currency selection and display.
    /// </summary>
    public class CurrencyInfo
    {
        public string Code { get; set; }

        public string Description { get; set; }

        public override string ToString() => $"{Code} - {Description}";
    }
}
