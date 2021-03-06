namespace TaxCalculator.API.DTOs
{
    public class OrderDetails
    {
        public double Subtotal { get; set; }
        public double Shipping { get; set; }
        public string OriginCountry { get; set; }
        public string OriginZip { get; set; }
        public string OriginState { get; set; }
        public string OriginCity { get; set; }
        public string OriginStreet { get; set; }
        public string DestinationCountry { get; set; }
        public string DestinationState { get; set; }
        public string DestinationZip { get; set; }
    }
}
