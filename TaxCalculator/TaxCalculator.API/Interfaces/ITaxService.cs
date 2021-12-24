using System.Threading.Tasks;
using TaxCalculator.API.DTOs;
using TaxCalculator.API.Models;

namespace TaxCalculator.API.Interfaces
{
    public interface ITaxService
    {
        public Task<TaxRate> GetTaxRate(string zip, string country);
        public Task<double> CalculateTaxesForOrder(OrderDetails order);
    }
}
