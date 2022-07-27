namespace Cashmere.Finacle.Integration.CQRS.Helpers
{
    public class PostConfiguration
    {
        public string ServerURI { get; set; }
        public string SOAVersion { get; set; }
        public string ChannelID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string DateFormat { get; set; }
        public string SystemCode { get; set; }
        public object UserID { get; set; }
        public string TransactionCode { get; set; }
        public string Realm { get; set; }
    }
}
