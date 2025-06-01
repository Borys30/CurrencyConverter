namespace CurrencyConverter.Services.Notification
{
    /// <summary>
    /// Defines the interface for notification service operations that display 
    /// informational, warning, and error messages to the user.
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Displays an informational message to the user with the specified title and content.
        /// </summary>
        void ShowInfo(string title, string message);

        /// <summary>
        /// Displays a warning message to the user with the specified title and content.
        /// </summary>
        void ShowWarning(string title, string message);

        /// <summary>
        /// Displays an error message to the user with the specified title and content.
        /// </summary>
        void ShowError(string title, string message);
    }
}
