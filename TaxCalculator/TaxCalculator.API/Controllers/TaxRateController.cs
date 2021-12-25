using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaxCalculator.API.Interfaces;
using TaxCalculator.API.Models;

namespace TaxCalculator.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaxRateController : ControllerBase
    {
        private readonly ITaxService _taxService;

        public TaxRateController(ITaxService taxService)
        {
            _taxService = taxService;
        }

        [HttpGet]
        public async Task<TaxRate> Get(string zip, string country = "US")
        {
            return await _taxService.GetTaxRateAsync(zip, country);
        }
    }
}
