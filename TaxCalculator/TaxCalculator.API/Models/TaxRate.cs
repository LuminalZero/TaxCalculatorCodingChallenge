namespace TaxCalculator.API.Models
{
    public class TaxRate
    {
        /// <summary>
        /// City name for given location.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// City sales tax rate for given location.
        /// </summary>
        public double CityRate { get; set; }

        /// <summary>
        /// Aggregate rate for all city and county sales tax districts effective at the location.
        /// </summary>
        public double CombinedDistrictRate { get; set; }

        /// <summary>
        /// Overall sales tax rate which includes state, county, city and district tax. This rate
        /// should be used to determine how much sales tax to collect for an order.
        /// </summary>
        public double CombinedRate { get; set; }

        /// <summary>
        /// Country for given location if SST state.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Country sales tax rate for given location if SST state.
        /// </summary>
        public double CountryRate { get; set; }

        /// <summary>
        /// County name for given location.
        /// </summary>
        public string County { get; set; }

        /// <summary>
        /// County sales tax rate for given location.
        /// </summary>
        public double CountyRate { get; set; }

        /// <summary>
        /// Distance selling threshold for given location.
        /// </summary>
        public double DistanceSaleThreshold { get; set; }

        /// <summary>
        /// Freight taxability for given location.
        /// </summary>
        public bool FreightTaxable { get; set; }

        /// <summary>
        /// Country name for given location.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Parking rate for given location.
        /// </summary>
        public double ParkingRate { get; set; }

        /// <summary>
        /// Reduced rate for given location.
        /// </summary>
        public double ReducedRate { get; set; }

        /// <summary>
        /// Standard rate for given location.
        /// </summary>
        public double StandardRate { get; set; }

        /// <summary>
        /// Postal abbreviated state name for given location.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// State sales tax rate for given location.
        /// </summary>
        public double StateRate { get; set; }

        /// <summary>
        /// Super reduced rate for given location.
        /// </summary>
        public double SuperReducedRate { get; set; }

        /// <summary>
        /// Postal code for given location.
        /// </summary>
        public string Zip { get; set; }
    }
}
