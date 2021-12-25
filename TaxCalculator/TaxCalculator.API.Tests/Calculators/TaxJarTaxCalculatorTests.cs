using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaxCalculator.API.Calculators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using TaxCalculator.API.DTOs;
using TaxCalculator.API.Constants;

namespace TaxCalculator.API.Calculators.Tests
{
    [TestClass()]
    public class TaxJarTaxCalculatorTests
    {
        TaxJarTaxCalculator calculator;

        [TestInitialize]
        public void TestInitialize()
        {
            var client = new HttpClient();
            calculator = new TaxJarTaxCalculator(client);
        }

        [TestMethod]
        public async Task GetRateForLocationAsync_ThrowsArgumentExceptionWhenZipIsNull()
        {
            // Arrange
            string testZip = null;
            var testCountry = "US";

            // Act / Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await calculator.GetRateForLocationAsync(testZip, testCountry));
        }

        [TestMethod]
        public async Task GetRateForLocationAsync_ThrowsArgumentExceptionWhenZipIsEmpty()
        {
            // Arrange
            string testZip = "";
            var testCountry = "US";

            // Act / Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await calculator.GetRateForLocationAsync(testZip, testCountry));
        }

        [TestMethod]
        public async Task GetRateForLocationAsync_ThrowsArgumentExceptionWhenZipIsWrongFormatForUnitedStates()
        {
            // Arrange
            string testZip = "ABC 123";
            var testCountry = "US";

            // Act / Assert
            await Assert .ThrowsExceptionAsync<ArgumentException>(async () => await calculator.GetRateForLocationAsync(testZip, testCountry));
        }

        [TestMethod]
        public async Task GetRateForLocationAsync_ThrowsArgumentExceptionWhenCountryIsNull()
        {
            // Arrange
            var testZip = "12345";
            string testCountry = null;

            // Act / Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await calculator.GetRateForLocationAsync(testZip, testCountry));
        }

        [TestMethod]
        public async Task GetRateForLocationAsync_ThrowsArgumentExceptionWhenCountryIsEmpty()
        {
            // Arrange
            var testZip = "12345";
            var testCountry = "";

            // Act / Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await calculator.GetRateForLocationAsync(testZip, testCountry));
        }

        [TestMethod]
        public async Task GetRateForLocationAsync_ThrowsArgumentExceptionWhenCountryIsOneLetter()
        {
            // Arrange
            var testZip = "12345";
            var testCountry = "U";

            // Act / Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await calculator.GetRateForLocationAsync(testZip, testCountry));
        }

        [TestMethod]
        public async Task GetRateForLocationAsync_ThrowsArgumentExceptionWhenCountryIsThreeLetters()
        {
            // Arrange
            var testZip = "12345";
            var testCountry = "USA";

            // Act / Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await calculator.GetRateForLocationAsync(testZip, testCountry));
        }

        [TestMethod]
        public async Task GetRateForLocationAsync_ThrowsArgumentExceptionWhenCountryIsANumber()
        {
            // Arrange
            var testZip = "12345";
            var testCountry = "1";

            // Act / Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await calculator.GetRateForLocationAsync(testZip, testCountry));
        }

        [TestMethod]
        public async Task GetTaxForOrderAsync_ThrowsArgumentExceptionWhenSubtotalIsZero()
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
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await calculator.GetTaxForOrderAsync(order));
        }

        [TestMethod]
        public async Task GetTaxForOrderAsync_ThrowsArgumentExceptionWhenDestinationCountryIsNull()
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
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await calculator.GetTaxForOrderAsync(order));
        }

        [TestMethod]
        public async Task GetTaxForOrderAsync_ThrowsArgumentExceptionWhenDestinationCountryIsEmpty()
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
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await calculator.GetTaxForOrderAsync(order));
        }

        [TestMethod]
        public async Task GetTaxForOrderAsync_ThrowsArgumentExceptionWhenDestinationCountryIsOneLetter()
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
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await calculator.GetTaxForOrderAsync(order));
        }

        [TestMethod]
        public async Task GetTaxForOrderAsync_ThrowsArgumentExceptionWhenDestinationCountryIsThreeLetters()
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
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await calculator.GetTaxForOrderAsync(order));
        }

        [TestMethod]
        public async Task GetTaxForOrderAsync_ThrowsArgumentExceptionWhenDestinationCountryIsANumber()
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
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await calculator.GetTaxForOrderAsync(order));
        }

        [TestMethod]
        public async Task GetTaxForOrderAsync_ThrowsArgumentExceptionWhenOriginCountryIsNull()
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
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await calculator.GetTaxForOrderAsync(order));
        }

        [TestMethod]
        public async Task GetTaxForOrderAsync_ThrowsArgumentExceptionWhenOriginCountryIsEmpty()
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
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await calculator.GetTaxForOrderAsync(order));
        }

        [TestMethod]
        public async Task GetTaxForOrderAsync_ThrowsArgumentExceptionWhenOriginCountryIsOneLetter()
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
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await calculator.GetTaxForOrderAsync(order));
        }

        [TestMethod]
        public async Task GetTaxForOrderAsync_ThrowsArgumentExceptionWhenOriginCountryIsThreeLetters()
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
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await calculator.GetTaxForOrderAsync(order));
        }

        [TestMethod]
        public async Task GetTaxForOrderAsync_ThrowsArgumentExceptionWhenOriginCountryIsANumber()
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
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await calculator.GetTaxForOrderAsync(order));
        }

        [TestMethod]
        public async Task GetTaxForOrderAsync_ThrowsArgumentExceptionWhenOriginStateIsNull()
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
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await calculator.GetTaxForOrderAsync(order));
        }

        [TestMethod]
        public async Task GetTaxForOrderAsync_ThrowsArgumentExceptionWhenOriginStateIsEmpty()
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
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await calculator.GetTaxForOrderAsync(order));
        }

        [TestMethod]
        public async Task GetTaxForOrderAsync_ThrowsArgumentExceptionWhenOriginZipIsNull()
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
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await calculator.GetTaxForOrderAsync(order));
        }

        [TestMethod]
        public async Task GetTaxForOrderAsync_ThrowsArgumentExceptionWhenOriginZipIsEmpty()
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
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await calculator.GetTaxForOrderAsync(order));
        }

        [TestMethod]
        public async Task GetTaxForOrderAsync_ThrowsArgumentExceptionWhenOriginZipIsIncorrectFormatForUnitedStates()
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
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await calculator.GetTaxForOrderAsync(order));
        }

        [TestMethod]
        public async Task GetTaxForOrderAsync_ThrowsArgumentExceptionWhenDestinationCountryIsUnitedStatesAndDestinationZipIsNull()
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
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await calculator.GetTaxForOrderAsync(order));
        }

        [TestMethod]
        public async Task GetTaxForOrderAsync_ThrowsArgumentExceptionWhenDestinationCountryIsUnitedStatesAndDestinationZipIsEmpty()
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
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await calculator.GetTaxForOrderAsync(order));
        }

        [TestMethod]
        public async Task GetTaxForOrderAsync_ThrowsArgumentExceptionWhenDestinationCountryIsUnitedStatesAndDestinationZipIsIncorrectFormatForUnitedStates()
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
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await calculator.GetTaxForOrderAsync(order));
        }

        [TestMethod]
        public async Task GetTaxForOrderAsync_ThrowsArgumentExceptionWhenDestinationCountryIsUnitedStatesOrCanadaAndDestinationStateIsNull()
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
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await calculator.GetTaxForOrderAsync(order));
        }

        [TestMethod]
        public async Task GetTaxForOrderAsync_ThrowsArgumentExceptionWhenDestinationCountryIsUnitedStatesOrCanadaAndDestinationStateIsEmpty()
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
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await calculator.GetTaxForOrderAsync(order));
        }
    }
}
