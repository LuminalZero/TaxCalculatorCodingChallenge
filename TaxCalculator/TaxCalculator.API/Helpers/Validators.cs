using System;
using System.Text.RegularExpressions;
using TaxCalculator.API.Constants;

namespace TaxCalculator.API.Helpers
{
    public static class Validators
    {
        public static bool IsCountryCodeValid(string country) => Regex.Match(country, @"^[a-zA-Z]{2}$").Success;

        public static bool IsZipValid(string zip) => Regex.Match(zip, @"^\d{5}(?:-\d{4})?$").Success;

        public static void ValidateRequired(double value)
        {
            if (value == default)
                throw new ArgumentException($@"{nameof(value)} is required.", nameof(value));
        }

        public static void ValidateRequired(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($@"{nameof(value)} is required.", nameof(value));
        }

        public static void ValidateCountryCode(string countryCode)
        {
            if (IsCountryCodeValid(countryCode) == false)
                throw new ArgumentException($@"Parameter {nameof(countryCode)} must be a two-letter ISO country code.", nameof(countryCode));
        }

        public static void ValidateZip(string countryCode, string zip)
        {
            if (countryCode != CountryConstants.UnitedStates) return;

            if (IsZipValid(zip) == false)
                throw new ArgumentException($@"Parameter {nameof(zip)} must be in the form 12345 or 12345-6789.", nameof(zip));
        }
    }
}
