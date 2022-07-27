using AutoMapper;
using BSAccountDetailsServiceReference;
using Cashmere.Finacle.Integration.CQRS.DTOs.ValidateAccount;
using Cashmere.Finacle.Integration.CQRS.Events;
using MediatR;
using Serilog;
using Serilog.Core;
using System.ServiceModel;
using AccountDetailsRequestHeaderType = BSAccountDetailsServiceReference.RequestHeaderType;

namespace Cashmere.Finacle.Integration.CQRS.Commands.ValidateAccount
{
    public class ValidateAccountCommandHandler : IRequestHandler<ValidateAccountCommand, int>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<ValidateAccountCommandHandler> _logger;

        public ValidateAccountCommandHandler(IMapper mapper, IMediator mediator, ILogger<ValidateAccountCommandHandler>logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int> Handle(ValidateAccountCommand request, CancellationToken cancellationToken)
        {
            var remoteAddress = new EndpointAddress("http://192.168.0.180/Account/AccountDetails/Get/3.0");
            //validation
            BasicHttpBinding binding = new BasicHttpBinding();
            var bsGetAccountDetailsClient = new BSGetAccountDetailsClient(binding, remoteAddress);
            request.ValidateAccountRequest.RequestHeaderType = new RequestHeaderTypeDto()
            {
                CorrelationID = new Guid().ToString(),
                CreationTimestamp = DateTime.Now.ToUniversalTime(),
                MessageID = new Guid().ToString(),
                Credentials = new CredentialsTypeDto
                {
                    BankID = "01",
                    SystemCode = "0",
                    Password = "",
                    Username = "",
                },

            };

            var accountDetailsRequestType = _mapper.Map<AccountDetailsRequestType>(request.ValidateAccountRequest.AccountDetailsRequestType);
            var requestHeaderType = _mapper.Map<AccountDetailsRequestHeaderType>(request.ValidateAccountRequest.RequestHeaderType);
            var response = await bsGetAccountDetailsClient.GetAccountDetailsAsync(requestHeaderType, accountDetailsRequestType);

            //mapper
            var validateAccountResponseDto = _mapper.Map<ValidateAccountResponseDto>(response);

            //notification
            await _mediator.Publish(new AccountValidated(validateAccountResponseDto));

            return 1;
        }
    }
}
