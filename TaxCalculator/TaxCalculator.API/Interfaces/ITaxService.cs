﻿using System.Threading.Tasks;
using TaxCalculator.API.Models;

namespace TaxCalculator.API.Interfaces
{
    public interface ITaxService
    {
        public Task<TaxRate> CalculateTaxRate(string zip, string country);
    }
}
