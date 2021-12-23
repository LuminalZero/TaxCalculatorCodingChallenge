namespace TaxCalculator.API.Configuration
{
    public class IntegrationsSection
    {
        public ExternalApi TaxJar { get; set; }
    }

    public class ExternalApi
    {
        public string Key { get; set; }
        public string Url { get; set; }
    }
}
