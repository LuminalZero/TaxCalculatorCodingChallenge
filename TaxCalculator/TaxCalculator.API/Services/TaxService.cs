using System.Threading.Tasks;
using TaxCalculator.API.DTOs;
using TaxCalculator.API.Interfaces;
using TaxCalculator.API.Models;

namespace TaxCalculator.API.Services
{
    public class TaxService : ITaxService
    {
        private readonly ITaxCalculator _taxCalculator;

        public TaxService(ITaxCalculator taxCalculator)
        {
            _taxCalculator = taxCalculator;
        }

        public async Task<double> CalculateTaxesForOrder(OrderDetails order)
        {
            return await _taxCalculator.GetTaxForOrderAsync(order);
        }

        public Task<TaxRate> GetTaxRate(string zip, string country)
        {
            return _taxCalculator.GetRateForLocationAsync(zip, country);
        }
    }
}
