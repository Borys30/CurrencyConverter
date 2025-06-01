using System.Windows;

namespace CurrencyConverter.Services.Notification
{
    /// <summary>
    /// Implements the INotificationService interface to display various types of notifications
    /// to the user using the Windows MessageBox functionality.
    /// </summary>
    public class NotificationService : INotificationService
    {
        /// <summary>
        /// Displays an informational message to the user with the specified title and content.
        /// </summary>
        public void ShowInfo(string title, string message)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Displays a warning message to the user with the specified title and content.
        /// </summary>
        public void ShowWarning(string title, string message)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        /// <summary>
        /// Displays an error message to the user with the specified title and content.
        /// </summary>
        public void ShowError(string title, string message)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
