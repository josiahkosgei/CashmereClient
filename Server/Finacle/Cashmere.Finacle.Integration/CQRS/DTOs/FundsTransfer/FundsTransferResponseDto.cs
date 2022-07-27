using Cashmere.Finacle.Integration.CQRS.DTOs.ValidateAccount;

namespace Cashmere.Finacle.Integration.CQRS.DTOs.FundsTransfer
{
    public class FundsTransferResponseDto
    {
        public  ResponseHeaderTypeDto ResponseHeaderType {get;set;}
        public  FundsTransferTypeDto FundsTransferType {get;set;}
    }
}
