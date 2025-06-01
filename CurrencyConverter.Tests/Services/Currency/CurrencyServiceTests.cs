using CurrencyConverter.Services.Api;
using CurrencyConverter.Services.Currency;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CurrencyConverter.Tests.Services.Currency
{
    public class CurrencyServiceTests
    {
        private const string EURLiteral = "EUR";
        private const string USDLiteral = "USD";

        private readonly IFrankfurterApiClient _mockApiClient = Substitute.For<IFrankfurterApiClient>();

        private readonly CurrencyService _currencyService;

        public CurrencyServiceTests()
        {
            _currencyService = new CurrencyService(_mockApiClient);
        }

        [Fact]
        public async Task should_return_list_of_available_currencies()
        {
            // Arrange
            const string jsonResponse = "{\"USD\": \"United States Dollar\", \"EUR\": \"Euro\"}";

            _mockApiClient.GetAvailableCurrenciesJsonAsync().Returns(Task.FromResult(jsonResponse));

            // Act
            var result = await _currencyService.GetAvailableCurrenciesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, c => c.Code == USDLiteral && c.Description == "United States Dollar");
            Assert.Contains(result, c => c.Code == EURLiteral && c.Description == "Euro");
        }

        [Fact]
        public async Task should_return_latest_conversion_rate()
        {
            // Arrange;
            const string sourceAmount = "100.0";
            const string conversionRateJson = "{\"rates\": {\"EUR\": 0.85}, \"amount\": 100.0}";

            _mockApiClient.GetLatestConversionRateJsonAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<decimal>())
                .Returns(Task.FromResult(conversionRateJson));

            // Act
            var result = await _currencyService.GetLatestConversionRateAsync(USDLiteral, EURLiteral, sourceAmount);

            // Assert
            Assert.Equal(100.0m, result.Amount);
            Assert.Equal(0.85m, result.Rates[EURLiteral]);
        }

        [Fact]
        public async Task should_throw_error_in_case_of_invalid_input_amount()
        {
            // Arrange
            const string sourceCurrency = EURLiteral;
            const string targetCurrency = USDLiteral;
            const string sourceAmount = "invalid_amount";

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _currencyService.GetLatestConversionRateAsync(sourceCurrency, targetCurrency, sourceAmount));

            Assert.Equal("The provided amount is not valid:'invalid_amount'.", exception.Message);
        }
    }
}
