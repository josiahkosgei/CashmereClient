using AutoMapper;
using BSAccountDetailsServiceReference;
using Cashmere.Finacle.Integration.CQRS.DTOs.ValidateAccount;
using Cashmere.Finacle.Integration.CQRS.Events;
using Cashmere.Finacle.Integration.CQRS.Helpers;
using MediatR;
using Microsoft.Extensions.Options;
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
        private readonly SOAServerConfiguration _soaServerConfiguration;

        public ValidateAccountCommandHandler(IMapper mapper, IMediator mediator, ILogger<ValidateAccountCommandHandler> logger, IOptionsMonitor<SOAServerConfiguration> optionsMonitor)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
            _soaServerConfiguration = optionsMonitor.CurrentValue;
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
                    SystemCode = _soaServerConfiguration.AccountValidationConfiguration.SystemCode,
                    Password = _soaServerConfiguration.AccountValidationConfiguration.Password,
                    Username = _soaServerConfiguration.AccountValidationConfiguration.Username,
                },

            };

            var accountDetailsRequestType = _mapper.Map<AccountDetailsRequestType>(request.ValidateAccountRequest.AccountDetailsRequestType);
            var requestHeaderType = _mapper.Map<AccountDetailsRequestHeaderType>(request.ValidateAccountRequest.RequestHeaderType);
            //var response = await bsGetAccountDetailsClient.GetAccountDetailsAsync(requestHeaderType, accountDetailsRequestType);
            var response = new operationOutput()
            {
                AccountDetailsResponse = new AccountDetailsResponseType
                {
                    AccountNumber = request.ValidateAccountRequest.AccountDetailsRequestType.AccountNumber
                },
                ResponseHeader = new ResponseHeaderType
                {
                    CorrelationID = new Guid().ToString(),
                    StatusCode = "0",
                    MessageID = new Guid().ToString(),

                }
            };
            //mapper
            var validateAccountResponseDto = _mapper.Map<ValidateAccountResponseDto>(response);

            //notification
            await _mediator.Publish(new AccountValidated(validateAccountResponseDto));

            return 1;
        }
    }
}
