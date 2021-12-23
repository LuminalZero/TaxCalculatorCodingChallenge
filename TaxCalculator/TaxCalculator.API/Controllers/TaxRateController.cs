using Microsoft.AspNetCore.Mvc;

namespace TaxCalculator.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaxRateController : ControllerBase
    {
        [HttpGet]
        public double Get()
        {
            return 0.0;
        }
    }
}
