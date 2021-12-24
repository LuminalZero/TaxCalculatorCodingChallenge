using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TaxCalculator.API.Constants;
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
            ValidateOrderDetails(order);

            return await _taxService.CalculateTaxesForOrder(order);
        }

        private static void ValidateOrderDetails(OrderDetails order)
        {
            if (order.Subtotal == default)
                throw new ArgumentException($@"{nameof(order.Subtotal)} is required.", nameof(order));

            if (order.Shipping == default)
                throw new ArgumentException($@"{nameof(order.Shipping)} is required.", nameof(order));

            TestOrderDetailsDestination(order);

            TestOrderDetailsOrigin(order);
        }

        private static void TestOrderDetailsDestination(OrderDetails order)
        {
            if (string.IsNullOrWhiteSpace(order.DestiationCountry))
                throw new ArgumentException($@"{nameof(order.DestiationCountry)} is required.", nameof(order));

            if (Regex.Match(order.DestiationCountry, @"^[a-zA-Z]{2}$").Success == false)
                throw new ArgumentException($@"{nameof(order.DestiationCountry)} must be a two-letter ISO country code.");

            if (order.DestiationCountry == CountryConstants.UnitedStates)
            {
                if (string.IsNullOrWhiteSpace(order.DestiationZip))
                    throw new ArgumentException($@"{nameof(order.DestiationZip)} is required when {order.DestiationCountry} is '{CountryConstants.UnitedStates}'.", nameof(order));

                if (Regex.Match(order.DestiationZip, @"^\d{5}(?:-\d{4})?$").Success == false)
                    throw new ArgumentException($@"{order.DestiationZip} must be in the form 12345 or 12345-6789.", nameof(order));
            }

            if ((order.DestiationCountry == CountryConstants.UnitedStates || order.DestiationCountry == CountryConstants.Canada))
            {
                if (string.IsNullOrWhiteSpace(order.DestiationState))
                    throw new ArgumentException($@"{nameof(order.DestiationState)} is required when {order.DestiationCountry} is '{CountryConstants.UnitedStates}' or '{CountryConstants.Canada}'.", nameof(order));
            }
        }

        private static void TestOrderDetailsOrigin(OrderDetails order)
        {
            if (string.IsNullOrWhiteSpace(order.OriginCountry))
                throw new ArgumentException($@"{nameof(order.OriginCountry)} is required.", nameof(order));
            if (Regex.Match(order.OriginCountry, @"^[a-zA-Z]{2}$").Success == false)
                throw new ArgumentException($@"{nameof(order.OriginCountry)} must be a two-letter ISO country code.");
            if (string.IsNullOrWhiteSpace(order.OriginState))
                throw new ArgumentException($@"{nameof(order.OriginState)} is required.", nameof(order));
            if (string.IsNullOrWhiteSpace(order.OriginZip))
                throw new ArgumentException($@"{nameof(order.OriginZip)} is required.", nameof(order));
            if (Regex.Match(order.OriginZip, @"^\d{5}(?:-\d{4})?$").Success == false)
                throw new ArgumentException($@"{order.OriginZip} must be in the form 12345 or 12345-6789.", nameof(order));
        }
    }
}
