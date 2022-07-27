using Cashmere.Finacle.Integration.CQRS.DTOs.ValidateAccount;
using Cashmere.Finacle.Integration.CQRS.Interfaces;
using MediatR;

namespace Cashmere.Finacle.Integration.CQRS.Commands.ValidateAccount
{
    public class ValidateAccountCommand  : IRequest<int>
    {
        public ValidateAccountRequestDto ValidateAccountRequest { get; set; }
    }
}
