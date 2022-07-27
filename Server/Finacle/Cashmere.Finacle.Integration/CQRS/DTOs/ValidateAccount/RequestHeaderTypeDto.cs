namespace Cashmere.Finacle.Integration.CQRS.DTOs.ValidateAccount
{
    public class RequestHeaderTypeDto
    {

        public DateTime CreationTimestamp { get; set; }
        public bool CreationTimestampSpecified { get; set; }
        public string CorrelationID { get; set; }
        public string FaultTO { get; set; }
        public string MessageID { get; set; }
        public string ReplyTO { get; set; }
        public CredentialsTypeDto Credentials { get; set; }
    }
}