namespace Cashmere.Finacle.Integration.CQRS.Helpers
{

    public class SOAServerConfiguration
    {
        public object ServerURI { get; set; }
        public static string APP_BASE_URL { get; set; }
        public static string APP_PATH{ get; set; }
        public string APPLICATION_PATH => APP_BASE_URL.AppendAsURL(APP_PATH);
        public string AmountFormat { get; set; }
        public string DateFormat { get; set; }
        public string IsDebug { get; set; }
        public string ContentType { get; set; }
        public PostConfiguration PostConfiguration { get; set; }
        public AccountValidationConfiguration AccountValidationConfiguration { get; set; }
        public MonitoringConfiguration MonitoringConfiguration { get; set; }
    }
}
