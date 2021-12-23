using System.Threading.Tasks;
using TaxCalculator.API.Models;

namespace TaxCalculator.API.Interfaces
{
    public interface ITaxService
    {
        Task<TaxRate> GetRateForLocationAsync(int zip);
    }
}
