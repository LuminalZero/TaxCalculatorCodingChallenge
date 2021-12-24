using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaxCalculator.API.Calculators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

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
        public void GetTaxForOrderAsyncTest()
        {
            
        }
    }
}
