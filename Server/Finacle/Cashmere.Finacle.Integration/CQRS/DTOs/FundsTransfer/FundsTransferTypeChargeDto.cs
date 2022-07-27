namespace Cashmere.Finacle.Integration.CQRS.DTOs.FundsTransfer
{
    public class FundsTransferTypeChargeDto
    {
        public string EventType { get; set; }
        public string EventId { get; set; }
        public string ChargeAccountSerial { get; set; }
    }
}