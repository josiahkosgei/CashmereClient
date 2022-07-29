using AutoMapper;
using BSAccountDetailsServiceReference;
using Cashmere.Finacle.Integration.CQRS.DTOs.ValidateAccount;
using Cashmere.Finacle.Integration.CQRS.Events;
using Cashmere.Finacle.Integration.CQRS.Helpers;
using MediatR;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Core;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Description;
using AccountDetailsRequestHeaderType = BSAccountDetailsServiceReference.RequestHeaderType;

namespace Cashmere.Finacle.Integration.CQRS.Commands.ValidateAccount
{
    public class ValidateAccountCommandHandler : IRequestHandler<ValidateAccountCommand, ValidateAccountResponseDto>
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

        public async Task<ValidateAccountResponseDto> Handle(ValidateAccountCommand request, CancellationToken cancellationToken)
        {
            try
            {

                var remoteAddress = new EndpointAddress(_soaServerConfiguration.AccountValidationConfiguration.ServerURI);
                //validation
                BasicHttpBinding binding = new BasicHttpBinding();
                binding.Security.Mode = BasicHttpSecurityMode.None;
                binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;


                var bsGetAccountDetailsClient = new BSGetAccountDetailsClient(binding, remoteAddress);
                bsGetAccountDetailsClient.ClientCredentials.UserName.Password = _soaServerConfiguration.AccountValidationConfiguration.Password;
                bsGetAccountDetailsClient.ClientCredentials.UserName.UserName = _soaServerConfiguration.AccountValidationConfiguration.Username;

                request.ValidateAccountRequest.RequestHeaderType = new RequestHeaderTypeDto()
                {
                    CorrelationID = Guid.NewGuid().ToString(),

                    CreationTimestamp = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                    CreationTimestampSpecified = true,
                    // CreationTimestamp = Convert.ToDateTime(DateTime.Now.ToString(_soaServerConfiguration.AccountValidationConfiguration.DateFormat)),
                    MessageID = Guid.NewGuid().ToString(),
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
                _logger.LogInformation($"CommandHandler Finacle Request Body: {accountDetailsRequestType.ToJson()}");
                _logger.LogInformation($"CommandHandler Finacle Request Header: {requestHeaderType.ToJson()}");
                var response = await bsGetAccountDetailsClient.GetAccountDetailsAsync(requestHeaderType, accountDetailsRequestType);

                _logger.LogInformation($"CommandHandler: Finacle Response: {response.AsJson()}");
                //mapper

                var validateAccountResponseDto = _mapper.Map<ValidateAccountResponseDto>(response);

                //notification
                await _mediator.Publish(new AccountValidated(validateAccountResponseDto));
                return validateAccountResponseDto;
            }
            catch (Exception ex)
            {

                _logger.LogError($"CommandHandler Finacle Error: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }
    }
}
