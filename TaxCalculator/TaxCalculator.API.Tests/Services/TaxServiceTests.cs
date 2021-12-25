using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TaxCalculator.API.Calculators;
using TaxCalculator.API.Constants;
using TaxCalculator.API.DTOs;

namespace TaxCalculator.API.Services.Tests
{
    [TestClass()]
    public class TaxServiceTests
    {
        TaxService taxService;

        [TestInitialize]
        public void TestInitialize()
        {
            var client = new HttpClient();
            var calculator = new TaxJarTaxCalculator(client);
            taxService = new TaxService(calculator);
        }

        [TestMethod]
        public async Task GetTaxRate_ThrowsArgumentExceptionWhenZipIsNull()
        {
            // Arrange
            string testZip = null;
            var testCountry = "US";

            // Act / Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await taxService.GetTaxRateAsync(testZip, testCountry));
        }

        [TestMethod]
        public async Task GetTaxRate_ThrowsArgumentExceptionWhenZipIsEmpty()
        {
            // Arrange
            string testZip = "";
            var testCountry = "US";

            // Act / Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await taxService.GetTaxRateAsync(testZip, testCountry));
        }

        [TestMethod]
        public async Task GetTaxRate_ThrowsArgumentExceptionWhenZipIsWrongFormatForUnitedStates()
        {
            // Arrange
            string testZip = "ABC 123";
            var testCountry = "US";

            // Act / Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await taxService.GetTaxRateAsync(testZip, testCountry));
        }

        [TestMethod]
        public async Task GetTaxRate_ThrowsArgumentExceptionWhenCountryIsNull()
        {
            // Arrange
            var testZip = "12345";
            string testCountry = null;

            // Act / Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await taxService.GetTaxRateAsync(testZip, testCountry));
        }

        [TestMethod]
        public async Task GetTaxRate_ThrowsArgumentExceptionWhenCountryIsEmpty()
        {
            // Arrange
            var testZip = "12345";
            var testCountry = "";

            // Act / Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await taxService.GetTaxRateAsync(testZip, testCountry));
        }

        [TestMethod]
        public async Task GetTaxRate_ThrowsArgumentExceptionWhenCountryIsOneLetter()
        {
            // Arrange
            var testZip = "12345";
            var testCountry = "U";

            // Act / Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await taxService.GetTaxRateAsync(testZip, testCountry));
        }

        [TestMethod]
        public async Task GetTaxRate_ThrowsArgumentExceptionWhenCountryIsThreeLetters()
        {
            // Arrange
            var testZip = "12345";
            var testCountry = "USA";

            // Act / Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await taxService.GetTaxRateAsync(testZip, testCountry));
        }

        [TestMethod]
        public async Task GetTaxRate_ThrowsArgumentExceptionWhenCountryIsANumber()
        {
            // Arrange
            var testZip = "12345";
            var testCountry = "1";

            // Act / Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await taxService.GetTaxRateAsync(testZip, testCountry));
        }

        [TestMethod]
        public async Task CalculateTaxesForOrderAsync_ThrowsArgumentExceptionWhenSubtotalIsZero()
        {
            // Arrange
            var order = new OrderDetails
            {
                DestinationCountry = CountryConstants.UnitedStates,
                DestinationState = "TN",
                DestinationZip = "37043",
                OriginCountry = CountryConstants.UnitedStates,
                OriginState = "TN",
                OriginZip = "37043",
                Shipping = 2.00,
                Subtotal = 0,
            };

            // Act / Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await taxService.CalculateTaxesForOrderAsync(order));
        }

        [TestMethod]
        public async Task CalculateTaxesForOrderAsync_ThrowsArgumentExceptionWhenDestinationCountryIsNull()
        {
            // Arrange
            var order = new OrderDetails
            {
                DestinationCountry = null,
                DestinationState = "TN",
                DestinationZip = "37043",
                OriginCountry = CountryConstants.UnitedStates,
                OriginState = "TN",
                OriginZip = "37043",
                Shipping = 2.00,
                Subtotal = 20.00,
            };

            // Act / Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await taxService.CalculateTaxesForOrderAsync(order));
        }

        [TestMethod]
        public async Task CalculateTaxesForOrderAsync_ThrowsArgumentExceptionWhenDestinationCountryIsEmpty()
        {
            // Arrange
            var order = new OrderDetails
            {
                DestinationCountry = "",
                DestinationState = "TN",
                DestinationZip = "37043",
                OriginCountry = CountryConstants.UnitedStates,
                OriginState = "TN",
                OriginZip = "37043",
                Shipping = 2.00,
                Subtotal = 20.00,
            };

            // Act / Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await taxService.CalculateTaxesForOrderAsync(order));
        }

        [TestMethod]
        public async Task CalculateTaxesForOrderAsync_ThrowsArgumentExceptionWhenDestinationCountryIsOneLetter()
        {
            // Arrange
            var order = new OrderDetails
            {
                DestinationCountry = "U",
                DestinationState = "TN",
                DestinationZip = "37043",
                OriginCountry = CountryConstants.UnitedStates,
                OriginState = "TN",
                OriginZip = "37043",
                Shipping = 2.00,
                Subtotal = 20.00,
            };

            // Act / Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await taxService.CalculateTaxesForOrderAsync(order));
        }

        [TestMethod]
        public async Task CalculateTaxesForOrderAsync_ThrowsArgumentExceptionWhenDestinationCountryIsThreeLetters()
        {
            // Arrange
            var order = new OrderDetails
            {
                DestinationCountry = "USA",
                DestinationState = "TN",
                DestinationZip = "37043",
                OriginCountry = CountryConstants.UnitedStates,
                OriginState = "TN",
                OriginZip = "37043",
                Shipping = 2.00,
                Subtotal = 20.00,
            };

            // Act / Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await taxService.CalculateTaxesForOrderAsync(order));
        }

        [TestMethod]
        public async Task CalculateTaxesForOrderAsync_ThrowsArgumentExceptionWhenDestinationCountryIsANumber()
        {
            // Arrange
            var order = new OrderDetails
            {
                DestinationCountry = "1",
                DestinationState = "TN",
                DestinationZip = "37043",
                OriginCountry = CountryConstants.UnitedStates,
                OriginState = "TN",
                OriginZip = "37043",
                Shipping = 2.00,
                Subtotal = 20.00,
            };

            // Act / Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await taxService.CalculateTaxesForOrderAsync(order));
        }

        [TestMethod]
        public async Task CalculateTaxesForOrderAsync_ThrowsArgumentExceptionWhenOriginCountryIsNull()
        {
            // Arrange
            var order = new OrderDetails
            {
                DestinationCountry = CountryConstants.UnitedStates,
                DestinationState = "TN",
                DestinationZip = "37043",
                OriginCountry = null,
                OriginState = "TN",
                OriginZip = "37043",
                Shipping = 2.00,
                Subtotal = 20.00,
            };

            // Act / Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await taxService.CalculateTaxesForOrderAsync(order));
        }

        [TestMethod]
        public async Task CalculateTaxesForOrderAsync_ThrowsArgumentExceptionWhenOriginCountryIsEmpty()
        {
            // Arrange
            var order = new OrderDetails
            {
                DestinationCountry = CountryConstants.UnitedStates,
                DestinationState = "TN",
                DestinationZip = "37043",
                OriginCountry = "",
                OriginState = "TN",
                OriginZip = "37043",
                Shipping = 2.00,
                Subtotal = 20.00,
            };

            // Act / Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await taxService.CalculateTaxesForOrderAsync(order));
        }

        [TestMethod]
        public async Task CalculateTaxesForOrderAsync_ThrowsArgumentExceptionWhenOriginCountryIsOneLetter()
        {
            // Arrange
            var order = new OrderDetails
            {
                DestinationCountry = CountryConstants.UnitedStates,
                DestinationState = "TN",
                DestinationZip = "37043",
                OriginCountry = "U",
                OriginState = "TN",
                OriginZip = "37043",
                Shipping = 2.00,
                Subtotal = 20.00,
            };

            // Act / Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await taxService.CalculateTaxesForOrderAsync(order));
        }

        [TestMethod]
        public async Task CalculateTaxesForOrderAsync_ThrowsArgumentExceptionWhenOriginCountryIsThreeLetters()
        {
            // Arrange
            var order = new OrderDetails
            {
                DestinationCountry = CountryConstants.UnitedStates,
                DestinationState = "TN",
                DestinationZip = "37043",
                OriginCountry = "USA",
                OriginState = "TN",
                OriginZip = "37043",
                Shipping = 2.00,
                Subtotal = 20.00,
            };

            // Act / Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await taxService.CalculateTaxesForOrderAsync(order));
        }

        [TestMethod]
        public async Task CalculateTaxesForOrderAsync_ThrowsArgumentExceptionWhenOriginCountryIsANumber()
        {
            // Arrange
            var order = new OrderDetails
            {
                DestinationCountry = CountryConstants.UnitedStates,
                DestinationState = "TN",
                DestinationZip = "37043",
                OriginCountry = "1",
                OriginState = "TN",
                OriginZip = "37043",
                Shipping = 2.00,
                Subtotal = 20.00,
            };

            // Act / Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await taxService.CalculateTaxesForOrderAsync(order));
        }

        [TestMethod]
        public async Task CalculateTaxesForOrderAsync_ThrowsArgumentExceptionWhenOriginStateIsNull()
        {
            // Arrange
            var order = new OrderDetails
            {
                DestinationCountry = CountryConstants.UnitedStates,
                DestinationState = "TN",
                DestinationZip = "37043",
                OriginCountry = CountryConstants.UnitedStates,
                OriginState = null,
                OriginZip = "37043",
                Shipping = 2.00,
                Subtotal = 20.00,
            };

            // Act / Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await taxService.CalculateTaxesForOrderAsync(order));
        }

        [TestMethod]
        public async Task CalculateTaxesForOrderAsync_ThrowsArgumentExceptionWhenOriginStateIsEmpty()
        {
            // Arrange
            var order = new OrderDetails
            {
                DestinationCountry = CountryConstants.UnitedStates,
                DestinationState = "TN",
                DestinationZip = "37043",
                OriginCountry = CountryConstants.UnitedStates,
                OriginState = "",
                OriginZip = "37043",
                Shipping = 2.00,
                Subtotal = 20.00,
            };

            // Act / Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await taxService.CalculateTaxesForOrderAsync(order));
        }

        [TestMethod]
        public async Task CalculateTaxesForOrderAsync_ThrowsArgumentExceptionWhenOriginZipIsNull()
        {
            // Arrange
            var order = new OrderDetails
            {
                DestinationCountry = CountryConstants.UnitedStates,
                DestinationState = "TN",
                DestinationZip = "37043",
                OriginCountry = CountryConstants.UnitedStates,
                OriginState = "TN",
                OriginZip = null,
                Shipping = 2.00,
                Subtotal = 20.00,
            };

            // Act / Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await taxService.CalculateTaxesForOrderAsync(order));
        }

        [TestMethod]
        public async Task CalculateTaxesForOrderAsync_ThrowsArgumentExceptionWhenOriginZipIsEmpty()
        {
            // Arrange
            var order = new OrderDetails
            {
                DestinationCountry = CountryConstants.UnitedStates,
                DestinationState = "TN",
                DestinationZip = "37043",
                OriginCountry = CountryConstants.UnitedStates,
                OriginState = "TN",
                OriginZip = "",
                Shipping = 2.00,
                Subtotal = 20.00,
            };

            // Act / Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await taxService.CalculateTaxesForOrderAsync(order));
        }

        [TestMethod]
        public async Task CalculateTaxesForOrderAsync_ThrowsArgumentExceptionWhenOriginZipIsIncorrectFormatForUnitedStates()
        {
            // Arrange
            var order = new OrderDetails
            {
                DestinationCountry = CountryConstants.UnitedStates,
                DestinationState = "TN",
                DestinationZip = "37043",
                OriginCountry = CountryConstants.UnitedStates,
                OriginState = "TN",
                OriginZip = "ABC 123",
                Shipping = 2.00,
                Subtotal = 20.00,
            };

            // Act / Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await taxService.CalculateTaxesForOrderAsync(order));
        }

        [TestMethod]
        public async Task CalculateTaxesForOrderAsync_ThrowsArgumentExceptionWhenDestinationCountryIsUnitedStatesAndDestinationZipIsNull()
        {
            // Arrange
            var order = new OrderDetails
            {
                DestinationCountry = CountryConstants.UnitedStates,
                DestinationState = "TN",
                DestinationZip = null,
                OriginCountry = CountryConstants.UnitedStates,
                OriginState = "TN",
                OriginZip = "37043",
                Shipping = 2.00,
                Subtotal = 20.00,
            };

            // Act / Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await taxService.CalculateTaxesForOrderAsync(order));
        }

        [TestMethod]
        public async Task CalculateTaxesForOrderAsync_ThrowsArgumentExceptionWhenDestinationCountryIsUnitedStatesAndDestinationZipIsEmpty()
        {
            // Arrange
            var order = new OrderDetails
            {
                DestinationCountry = CountryConstants.UnitedStates,
                DestinationState = "TN",
                DestinationZip = "",
                OriginCountry = CountryConstants.UnitedStates,
                OriginState = "TN",
                OriginZip = "37043",
                Shipping = 2.00,
                Subtotal = 20.00,
            };

            // Act / Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await taxService.CalculateTaxesForOrderAsync(order));
        }

        [TestMethod]
        public async Task CalculateTaxesForOrderAsync_ThrowsArgumentExceptionWhenDestinationCountryIsUnitedStatesAndDestinationZipIsIncorrectFormatForUnitedStates()
        {
            // Arrange
            var order = new OrderDetails
            {
                DestinationCountry = CountryConstants.UnitedStates,
                DestinationState = "TN",
                DestinationZip = "ABC 123",
                OriginCountry = CountryConstants.UnitedStates,
                OriginState = "TN",
                OriginZip = "37043",
                Shipping = 2.00,
                Subtotal = 20.00,
            };

            // Act / Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await taxService.CalculateTaxesForOrderAsync(order));
        }

        [TestMethod]
        public async Task CalculateTaxesForOrderAsync_ThrowsArgumentExceptionWhenDestinationCountryIsUnitedStatesOrCanadaAndDestinationStateIsNull()
        {
            // Arrange
            var order = new OrderDetails
            {
                DestinationCountry = CountryConstants.Canada,
                DestinationState = null,
                DestinationZip = "37043",
                OriginCountry = CountryConstants.UnitedStates,
                OriginState = "37043",
                OriginZip = "37043",
                Shipping = 2.00,
                Subtotal = 20.00,
            };

            // Act / Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await taxService.CalculateTaxesForOrderAsync(order));
        }

        [TestMethod]
        public async Task CalculateTaxesForOrderAsync_ThrowsArgumentExceptionWhenDestinationCountryIsUnitedStatesOrCanadaAndDestinationStateIsEmpty()
        {
            // Arrange
            var order = new OrderDetails
            {
                DestinationCountry = CountryConstants.Canada,
                DestinationState = "",
                DestinationZip = "37043",
                OriginCountry = CountryConstants.UnitedStates,
                OriginState = "37043",
                OriginZip = "37043",
                Shipping = 2.00,
                Subtotal = 20.00,
            };

            // Act / Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await taxService.CalculateTaxesForOrderAsync(order));
        }
    }
}
