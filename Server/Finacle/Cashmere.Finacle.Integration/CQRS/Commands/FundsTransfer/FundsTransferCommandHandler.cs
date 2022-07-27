using AutoMapper;
using BSAccountFundsTransferServiceReference;
using Cashmere.Finacle.Integration.CQRS.DTOs.FundsTransfer;
using Cashmere.Finacle.Integration.CQRS.DTOs.ValidateAccount;
using Cashmere.Finacle.Integration.CQRS.Events;
using MediatR;
using System.ServiceModel;
using FundsTransferRequestHeaderType = BSAccountFundsTransferServiceReference.RequestHeaderType;

namespace Cashmere.Finacle.Integration.CQRS.Commands.FundsTransfer
{
    public class FundsTransferCommandHandler : IRequestHandler<FundsTransferCommand, int>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<FundsTransferCommandHandler> _logger;
        private readonly IMapper _mapper;
        public FundsTransferCommandHandler(IMapper mapper, IMediator mediator, ILogger<FundsTransferCommandHandler>logger)
        {
            _mediator = mediator;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<int> Handle(FundsTransferCommand request, CancellationToken cancellationToken)
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            var remoteAddress = new EndpointAddress("http://192.168.0.180/Account/FundsTransfer/Get/3.0");
            //validation
            var bsAccountClient = new BSAccountClient(binding, remoteAddress);
            var requestHeaderTypeDto = new RequestHeaderTypeDto()
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

            var fundsTransfer = _mapper.Map<FundsTransferType>(request.FundsTransfer);
            var requestHeaderType = _mapper.Map<FundsTransferRequestHeaderType>(requestHeaderTypeDto);
            var postRequest = new PostRequest
            {
                FundsTransfer = fundsTransfer,
                RequestHeader = requestHeaderType
            };
            var response = await bsAccountClient.PostAsync(postRequest);

            //mapper
            var validateAccountResponseDto = _mapper.Map<FundsTransferResponseDto>(response);

            //notification
            await _mediator.Publish(new FundsTransfered(validateAccountResponseDto));

            return 1;
        }
    }
}
