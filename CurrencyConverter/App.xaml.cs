using CurrencyConverter.Core.Converters.HistoricalRates;
using CurrencyConverter.Core.Creators;
using CurrencyConverter.Domain.Models;
using CurrencyConverter.Services.Api;
using CurrencyConverter.Services.Cache;
using CurrencyConverter.Services.Cache.Eviction.Factory;
using CurrencyConverter.Services.CacheSettings.CacheSettingsView;
using CurrencyConverter.Services.CacheSettings.Persistence;
using CurrencyConverter.Services.Currency;
using CurrencyConverter.Services.CurrencyView;
using CurrencyConverter.Services.HistoricalRatesView;
using CurrencyConverter.Services.Notification;
using CurrencyConverter.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace CurrencyConverter
{
    /// <summary>
    /// Main application class that handles application initialization and dependency injection setup.
    /// Configures and registers all services and components required by the application.
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        /// <summary>
        /// Overrides the OnStartup method to configure and build the service provider.
        /// Called when the application starts.
        /// </summary>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = ConfigureServices();

            ServiceProvider = services.BuildServiceProvider();
        }

        /// <summary>
        /// Configures the application's dependency injection services.
        /// Registers all required services, view models, and their dependencies in the service collection.
        /// Returns the configured ServiceCollection ready for building the service provider.
        /// </summary>
        private static ServiceCollection ConfigureServices()
        {
            var services = new ServiceCollection();

            // Registration of helper services.
            services.AddHttpClient<IFrankfurterApiClient, FrankfurterApiClient>();
            services.AddSingleton<INotificationService, NotificationService>();
            services.AddSingleton<IHistoricalRateQueryCreator, HistoricalRateQueryCreator>();
            services.AddSingleton<IHistoricalRateConverter, HistoricalRateConverter>();

            // Registration of cache related services.
            services.AddSingleton<ICacheSettingsPersistenceService, CacheSettingsPersistenceService>();
            services.AddSingleton<ICacheSettingsViewService, CacheSettingsViewService>();
            services.AddSingleton<ICacheEvictionStrategyFactory<HistoricalRateQuery, HistoricalRequestedData>, CacheEvictionStrategyFactory<HistoricalRateQuery, HistoricalRequestedData>>();

            services.AddSingleton<ICacheService<HistoricalRateQuery, HistoricalRequestedData>>(provider =>
            {
                var cacheSettingsService = provider.GetRequiredService<ICacheSettingsViewService>();
                var cacheEvictionStrategyFactory = provider.GetRequiredService<ICacheEvictionStrategyFactory<HistoricalRateQuery, HistoricalRequestedData>>();

                return new CacheService<HistoricalRateQuery, HistoricalRequestedData>(cacheSettingsService, cacheEvictionStrategyFactory);
            });

            // Registration of currency service.
            services.AddTransient<CurrencyService>();
            services.AddTransient<ICurrencyService>(provider =>
            {
                var underlyingCurrencyService = provider.GetRequiredService<CurrencyService>();
                var cacheService = provider.GetRequiredService<ICacheService<HistoricalRateQuery, HistoricalRequestedData>>();
                return new CachedCurrencyService(underlyingCurrencyService, cacheService);
            });

            // Registration of currency view and historical view services.
            services.AddSingleton<ICurrencyViewService, CurrencyViewService>();
            services.AddSingleton<IHistoricalRatesViewService, HistoricalRatesViewService>();

            // Registration of view models.
            services.AddTransient<CurrencyConversionViewModel>();
            services.AddTransient<HistoricalRatesViewModel>();
            services.AddTransient<CacheSettingsViewModel>();

            // Registration of main view model.
            services.AddTransient(provider =>
            {
                return new MainViewModel(
                    provider.GetRequiredService<CurrencyConversionViewModel>(),
                    provider.GetRequiredService<HistoricalRatesViewModel>(),
                    provider.GetRequiredService<CacheSettingsViewModel>());
            });

            return services;
        }
    }
}
