using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaxCalculator.API.DTOs;
using TaxCalculator.API.Interfaces;

namespace TaxCalculator.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderTaxController : ControllerBase
    {
        private readonly ITaxService _taxService;

        public OrderTaxController(ITaxService taxService)
        {
            _taxService = taxService;
        }

        [HttpPost]
        public async Task<double> Post(OrderDetails order)
        {
            return await _taxService.CalculateTaxesForOrder(order);
        }
    }
}
