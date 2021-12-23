using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TaxCalculator.API.Interfaces;
using TaxCalculator.API.Models;

namespace TaxCalculator.API.Services
{
    public class TaxJarTaxService : ITaxService
    {
        private readonly HttpClient _client;
        private const string ENDPOINT = "rates";

        public TaxJarTaxService(HttpClient client)
        {
            _client = client;
        }

        public async Task<TaxRate> GetRateForLocationAsync(int zip)
        {
            try
            {
                var response = await _client.GetFromJsonAsync<TaxJarRate>($@"{ENDPOINT}/{zip}");

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
                FreightTaxable = from.rate.freight_taxable,
                State = from.rate.state,
                StateRate = from.rate.state_rate,
                Zip = from.rate.zip,
            };
        }

        private class TaxJarRate
        {
            public Rate rate { get; set; }
        }

        private class Rate
        {
            public string zip { get; set; }
            public string country { get; set; }
            public double country_rate { get; set; }
            public string state { get; set; }
            public double state_rate { get; set; }
            public string county { get; set; }
            public double county_rate { get; set; }
            public string city { get; set; }
            public double city_rate { get; set; }
            public double combined_district_rate { get; set; }
            public double combined_rate { get; set; }
            public bool freight_taxable { get; set; }
        }
    }
}
