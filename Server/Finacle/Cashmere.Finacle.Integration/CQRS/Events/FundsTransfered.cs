using Cashmere.Finacle.Integration.CQRS.DTOs.FundsTransfer;
using Cashmere.Finacle.Integration.CQRS.DTOs.ValidateAccount;
using MediatR;

namespace Cashmere.Finacle.Integration.CQRS.Events
{
    public class FundsTransfered : INotification
    {
        public FundsTransferResponseDto _fundsTransferResponseDto { get; }

        public FundsTransfered(FundsTransferResponseDto fundsTransferResponseDto)
        {
            _fundsTransferResponseDto = fundsTransferResponseDto;
        }
    }
}
