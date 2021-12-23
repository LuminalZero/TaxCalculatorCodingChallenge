using System.Threading.Tasks;
using TaxCalculator.API.Models;

namespace TaxCalculator.API.Interfaces
{
    public interface ITaxCalculator
    {
        Task<TaxRate> GetRateForLocationAsync(string zip, string country);
    }
}
