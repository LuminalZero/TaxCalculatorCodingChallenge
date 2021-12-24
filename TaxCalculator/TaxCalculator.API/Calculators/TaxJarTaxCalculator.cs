using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TaxCalculator.API.Constants;
using TaxCalculator.API.DTOs;
using TaxCalculator.API.Interfaces;
using TaxCalculator.API.Models;

namespace TaxCalculator.API.Calculators
{
    public class TaxJarTaxCalculator : ITaxCalculator
    {
        private readonly HttpClient _client;
        private const string RATE_ENDPOINT = "rates";
        private const string TAXES_ENDPOINT = "taxes";

        public TaxJarTaxCalculator(HttpClient client)
        {
            _client = client;
        }

        public async Task<TaxRate> GetRateForLocationAsync(string zip, string country)
        {
            if (Regex.Match(zip, @"^\d{5}(?:-\d{4})?$").Success == false)
                throw new ArgumentException($@"Parameter {nameof(zip)} must be in the form 12345 or 12345-6789.", nameof(zip));

            if (Regex.Match(country, @"^[a-zA-Z]{2}$").Success == false)
                throw new ArgumentException($@"Parameter {nameof(country)} must be a two-letter ISO country code.");

            try
            {
                var response = await _client.GetFromJsonAsync<TaxJarRate>($@"{RATE_ENDPOINT}/{zip}?country={country}");

                return MapTaxRate(response);
            }
            // purposefully not using caught exception, just pass back an empty object.
            catch (HttpRequestException)
            {
                return new TaxRate { Zip = zip.ToString() };
            }
            // unknown error happened, make a fuss about it...
            catch
            {
                throw;
            }
        }

        private static TaxRate MapTaxRate(TaxJarRate from)
        {
            return new TaxRate
            {
                City = from.rate.city,
                CityRate = from.rate.city_rate,
                CombinedDistrictRate = from.rate.combined_district_rate,
                CombinedRate = from.rate.combined_rate,
                Country = from.rate.country,
                CountryRate = from.rate.country_rate,
                County = from.rate.county,
                CountyRate = from.rate.county_rate,
                DistanceSaleThreshold = from.rate.distance_sale_threshold,
                FreightTaxable = from.rate.freight_taxable,
                Name = from.rate.name,
                ParkingRate = from.rate.parking_rate,
                ReducedRate = from.rate.reduced_rate,
                StandardRate = from.rate.standard_rate,
                State = from.rate.state,
                StateRate = from.rate.state_rate,
                SuperReducedRate = from.rate.super_reduced_rate,
                Zip = from.rate.zip,
            };
        }

        public async Task<double> GetTaxForOrderAsync(OrderDetails order)
        {
            ValidateOrderDetails(order);

            var data = new
            {
                amount = order.Subtotal,
                shipping = order.Shipping,
                from_country = order.OriginCountry,
                from_zip = order.OriginZip,
                from_state = order.OriginState,
                from_city = order.OriginCity,
                from_street = order.OriginStreet,
                to_country = order.DestiationCountry,
                to_state = order.DestiationState,
                to_zip = order.DestiationZip,
            };

            var response = await _client.PostAsJsonAsync(TAXES_ENDPOINT, data);
            if (response.IsSuccessStatusCode == false) throw new Exception();

            var content = await response.Content.ReadFromJsonAsync<TaxJarTax>();

            return content.tax.amount_to_collect;
        }

        private static void ValidateOrderDetails(OrderDetails order)
        {
            if (order.Subtotal == default)
                throw new ArgumentException($@"{nameof(order.Subtotal)} is required.", nameof(order));

            if (order.Shipping == default)
                throw new ArgumentException($@"{nameof(order.Shipping)} is required.", nameof(order));

            TestOrderDetailsDestination(order);

            TestOrderDetailsOrigin(order);
        }

        private static void TestOrderDetailsDestination(OrderDetails order)
        {
            if (string.IsNullOrWhiteSpace(order.DestiationCountry))
                throw new ArgumentException($@"{nameof(order.DestiationCountry)} is required.", nameof(order));

            if (Regex.Match(order.DestiationCountry, @"^[a-zA-Z]{2}$").Success == false)
                throw new ArgumentException($@"{nameof(order.DestiationCountry)} must be a two-letter ISO country code.");

            if (order.DestiationCountry == CountryConstants.UnitedStates)
            {
                if (string.IsNullOrWhiteSpace(order.DestiationZip))
                    throw new ArgumentException($@"{nameof(order.DestiationZip)} is required when {order.DestiationCountry} is '{CountryConstants.UnitedStates}'.", nameof(order));

                if (Regex.Match(order.DestiationZip, @"^\d{5}(?:-\d{4})?$").Success == false)
                    throw new ArgumentException($@"{order.DestiationZip} must be in the form 12345 or 12345-6789.", nameof(order));
            }

            if (order.DestiationCountry == CountryConstants.UnitedStates || order.DestiationCountry == CountryConstants.Canada)
            {
                if (string.IsNullOrWhiteSpace(order.DestiationState))
                    throw new ArgumentException($@"{nameof(order.DestiationState)} is required when {order.DestiationCountry} is '{CountryConstants.UnitedStates}' or '{CountryConstants.Canada}'.", nameof(order));
            }
        }

        private static void TestOrderDetailsOrigin(OrderDetails order)
        {
            if (string.IsNullOrWhiteSpace(order.OriginCountry))
                throw new ArgumentException($@"{nameof(order.OriginCountry)} is required.", nameof(order));
            if (Regex.Match(order.OriginCountry, @"^[a-zA-Z]{2}$").Success == false)
                throw new ArgumentException($@"{nameof(order.OriginCountry)} must be a two-letter ISO country code.");
            if (string.IsNullOrWhiteSpace(order.OriginState))
                throw new ArgumentException($@"{nameof(order.OriginState)} is required.", nameof(order));
            if (string.IsNullOrWhiteSpace(order.OriginZip))
                throw new ArgumentException($@"{nameof(order.OriginZip)} is required.", nameof(order));
            if (Regex.Match(order.OriginZip, @"^\d{5}(?:-\d{4})?$").Success == false)
                throw new ArgumentException($@"{order.OriginZip} must be in the form 12345 or 12345-6789.", nameof(order));
        }

        private class TaxJarRate
        {
            public Rate rate { get; set; }
        }

        private class Rate
        {
            public string city { get; set; }
            public double city_rate { get; set; }
            public double combined_district_rate { get; set; }
            public double combined_rate { get; set; }
            public string country { get; set; }
            public double country_rate { get; set; }
            public string county { get; set; }
            public double county_rate { get; set; }
            public double distance_sale_threshold { get; set; }
            public bool freight_taxable { get; set; }
            public string name { get; set; }
            public double parking_rate { get; set; }
            public double reduced_rate { get; set; }
            public double standard_rate { get; set; }
            public string state { get; set; }
            public double state_rate { get; set; }
            public double super_reduced_rate { get; set; }
            public string zip { get; set; }
        }

        private class TaxJarTax
        {
            public Tax tax { get; set; }
        }

        private class Tax
        {
            public double order_total_amount { get; set; }
            public double shipping { get; set; }
            public double taxable_amount { get; set; }
            public double amount_to_collect { get; set; }
            public double rate { get; set; }
            public bool freight_taxable { get; set; }
            public string tax_source { get; set; }
            public string exemption_type { get; set; }
        }
    }
}
