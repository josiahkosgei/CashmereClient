using Cashmere.Finacle.Integration.CQRS.Events;
using MediatR;

namespace Cashmere.Finacle.Integration.CQRS.Commands.ValidateAccount
{
    public class AccountValidatedHandler: INotificationHandler<AccountValidated>
    {

        private readonly ILogger<AccountValidatedHandler> _logger;

        public AccountValidatedHandler(ILogger<AccountValidatedHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(AccountValidated notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

   
    }
}
