using CurrencyConverter.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyConverter
{
    /// <summary>
    /// Locator class that provides access to main view model through the dependency injection container.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Returns a resolved instance of MainViewModel that's ready for binding to the UI.
        /// </summary>
        public MainViewModel MainViewModel => App.ServiceProvider.GetRequiredService<MainViewModel>();
    }
}
