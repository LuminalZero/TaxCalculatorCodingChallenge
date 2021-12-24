using System.Threading.Tasks;
using TaxCalculator.API.DTOs;
using TaxCalculator.API.Models;

namespace TaxCalculator.API.Interfaces
{
    public interface ITaxCalculator
    {
        Task<TaxRate> GetRateForLocationAsync(string zip, string country);
        Task<double> GetTaxForOrderAsync(OrderDetails order);
    }
}
