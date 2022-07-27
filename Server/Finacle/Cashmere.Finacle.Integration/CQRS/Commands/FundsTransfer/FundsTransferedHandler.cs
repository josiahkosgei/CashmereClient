using Cashmere.Finacle.Integration.CQRS.Events;
using MediatR;

namespace Cashmere.Finacle.Integration.CQRS.Commands.FundsTransfer
{
    public class FundsTransferedHandler : INotificationHandler<FundsTransfered>
    {
        private readonly ILogger<FundsTransferedHandler> _logger;

        public FundsTransferedHandler(ILogger<FundsTransferedHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(FundsTransfered notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
