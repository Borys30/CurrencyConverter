namespace CurrencyConverter.Core.Common
{
    /// <summary>
    /// Provides application-wide constant values for API endpoints, formatting patterns, and default values.
    /// </summary>
    public static class Constants
    {
        public const string BaseFrankfurterApiUri = "https://api.frankfurter.dev/";

        public const string CacheSettingsFilePath = "..\\..\\..\\..\\cache_settings.json";

        public const string DateFormat = "yyyy-MM-dd";
        public const string NumberFormat = "0.##";

        public const string InfoTitle = "Info";
        public const string WarningTitle = "Warning";
        public const string ErrorTitle = "Error";

        public const decimal DefaultAmountValue = 1m;
    }
}
