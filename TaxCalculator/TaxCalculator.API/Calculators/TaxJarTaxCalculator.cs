using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TaxCalculator.API.Constants;
using TaxCalculator.API.DTOs;
using TaxCalculator.API.Helpers;
using TaxCalculator.API.Interfaces;
using TaxCalculator.API.Models;

namespace TaxCalculator.API.Calculators
{
    public class TaxJarTaxCalculator : ITaxCalculator
    {
        private readonly HttpClient _client;

        public TaxJarTaxCalculator(HttpClient client)
        {
            _client = client;
        }

        public async Task<TaxRate> GetRateForLocationAsync(string zip, string countryCode)
        {
            Validators.ValidateRequired(zip);
            Validators.ValidateRequired(countryCode);

            Validators.ValidateCountryCode(countryCode);
            Validators.ValidateZip(countryCode, zip);

            try
            {
                var response = await _client.GetFromJsonAsync<TaxJarRate>(GetRatesUrl(zip, countryCode));

                return MapTaxRate(response);
            }
            // unknown error happened, make a fuss about it...
            catch
            {
                throw;
            }
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

            var response = await _client.PostAsJsonAsync("taxes", data);
            if (response.IsSuccessStatusCode == false) throw new Exception();

            var content = await response.Content.ReadFromJsonAsync<TaxJarTax>();

            return content.tax.amount_to_collect;
        }

        private static string GetRatesUrl(string zip, string country) => $@"rates/{zip}?country={country}";

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

        private static void ValidateOrderDetails(OrderDetails order)
        {
            Validators.ValidateRequired(order.Subtotal);
            Validators.ValidateRequired(order.Shipping);

            TestOrderDetailsDestination(order);
            TestOrderDetailsOrigin(order);
        }

        private static void TestOrderDetailsDestination(OrderDetails order)
        {
            Validators.ValidateRequired(order.DestiationCountry);
            Validators.ValidateCountryCode(order.DestiationCountry);

            if (order.DestiationCountry == CountryConstants.UnitedStates)
            {
                Validators.ValidateRequired(order.DestiationZip);
                Validators.ValidateZip(order.DestiationCountry, order.DestiationZip);
            }

            if (order.DestiationCountry == CountryConstants.UnitedStates || order.DestiationCountry == CountryConstants.Canada)
            {
                Validators.ValidateRequired(order.DestiationState);
            }
        }

        private static void TestOrderDetailsOrigin(OrderDetails order)
        {
            Validators.ValidateRequired(order.OriginCountry);
            Validators.ValidateCountryCode(order.OriginCountry);
            Validators.ValidateRequired(order.OriginState);
            Validators.ValidateRequired(order.OriginZip);
            Validators.ValidateZip(order.OriginCountry, order.OriginZip);
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
