using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.RegularExpressions;
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
            if (Regex.Match(zip, @"^\d{5}(?:-\d{4})?$").Success == false)
                throw new ArgumentException("Zip must be in the form 12345 or 12345-6789.", nameof(zip));

            if (Regex.Match(country, @"^[a-zA-Z]{2}$").Success == false)
                throw new ArgumentException("Country must be a two-letter ISO country code.");

            return await _taxService.GetTaxRate(zip, country);
        }
    }
}
