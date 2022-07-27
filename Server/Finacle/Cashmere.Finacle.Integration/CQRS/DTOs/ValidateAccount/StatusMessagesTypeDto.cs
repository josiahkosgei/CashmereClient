namespace Cashmere.Finacle.Integration.CQRS.DTOs.ValidateAccount
{
    public class StatusMessagesTypeDto
    {
        public string ApplicationID { get; set; }
        public string MessageCode { get; set; }
        public string MessageDescription { get; set; }
        public string MessageType { get; set; }
    }
}