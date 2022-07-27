using Cashmere.Finacle.Integration.CQRS.DTOs.FundsTransfer;
using MediatR;

namespace Cashmere.Finacle.Integration.CQRS.Commands.FundsTransfer
{
    public class FundsTransferCommand : IRequest<int>
    {
        public FundsTransferRequestDto FundsTransfer { get; set; }
    }
}
