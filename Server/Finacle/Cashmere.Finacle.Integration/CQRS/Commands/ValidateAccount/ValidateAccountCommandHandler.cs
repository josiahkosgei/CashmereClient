using Cashmere.Finacle.Integration.CQRS.DTOs.ValidateAccount;
using Cashmere.Finacle.Integration.CQRS.Events;
using MediatR;

namespace Cashmere.Finacle.Integration.CQRS.Commands.ValidateAccount
{
    public class ValidateAccountCommandHandler : IRequestHandler<ValidateAccountCommand, int>
    {
        private readonly IMediator _mediator;

        public ValidateAccountCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<int> Handle(ValidateAccountCommand request, CancellationToken cancellationToken)
        {
            //validation
            
            //mapper
            var validateAccountResponseDto = new ValidateAccountResponseDto();
            //notification
            await _mediator.Publish(new AccountValidated(validateAccountResponseDto));

            return 1;
        }
    }
}
