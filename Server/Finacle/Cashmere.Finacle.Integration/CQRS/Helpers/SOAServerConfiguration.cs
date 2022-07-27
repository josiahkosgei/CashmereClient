namespace Cashmere.Finacle.Integration.CQRS.Helpers
{

    public class SOAServerConfiguration
    {
        public string ServerURI { get; set; }
        public string APP_BASE_URL { get; set; }
        public string APP_PATH{ get; set; }
        public string APPLICATION_PATH => APP_BASE_URL.AppendAsURL(APP_PATH);
        public string AmountFormat { get; set; }
        public string DateFormat { get; set; }
        public string IsDebug { get; set; }
        public string ContentType { get; set; }
        public virtual PostConfiguration PostConfiguration { get; set; }
        public virtual AccountValidationConfiguration AccountValidationConfiguration { get; set; }
        public virtual MonitoringConfiguration MonitoringConfiguration { get; set; }
    }
}
