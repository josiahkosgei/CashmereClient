namespace Cashmere.Finacle.Integration.CQRS.DTOs.ValidateAccount
{
    public class ValidateAccountRequestDto
    {
        public RequestHeaderTypeDto RequestHeaderType { get; set; }
        public AccountDetailsRequestTypeDto AccountDetailsRequestType { get; set; }
    }
}
