﻿using Cashmere.Finacle.Integration.CQRS.Commands.FundsTransfer;
using Cashmere.Finacle.Integration.CQRS.Commands.ValidateAccount;
using Cashmere.Finacle.Integration.CQRS.DTOs.FundsTransfer;
using Cashmere.Finacle.Integration.CQRS.DTOs.ValidateAccount;
using Cashmere.Finacle.Integration.CQRS.Helpers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;

namespace Cashmere.Finacle.Integration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CashInController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CashInController> _logger;
        private readonly SOAServerConfiguration _soaServerConfiguration;

        public CashInController(ILogger<CashInController> logger, IMediator mediator, IOptionsMonitor<SOAServerConfiguration> optionsMonitor)
        {
            _logger = logger;
            _soaServerConfiguration = optionsMonitor.CurrentValue;
            _mediator = mediator;
        }
        [HttpPost("ValidateAccount", Name = "ValidateAccount")]
        [ProducesResponseType(typeof(ValidateAccountResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ValidateAccount([FromBody] AccountDetailsRequestTypeDto accountDetailsRequestTypeDto)
        {
            try
            {
                _logger.LogInformation("ValidateAccount Request: ", accountDetailsRequestTypeDto.AsJson());
                var command = new ValidateAccountCommand
                {
                    ValidateAccountRequest = new ValidateAccountRequestDto()
                    {
                        AccountDetailsRequestType = accountDetailsRequestTypeDto,
                        RequestHeaderType = new RequestHeaderTypeDto()
                    }
                };
                var validatedAccountResponse = await _mediator.Send(command);
                _logger.LogInformation("ValidateAccount Finacle Response: ", validatedAccountResponse.AsJson());
                return Ok(validatedAccountResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError("ValidateAccount Error: ", ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("FundsTransfer", Name = "FundsTransfer")]
        [ProducesResponseType(typeof(FundsTransferResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> FundsTransfer([FromBody] FundsTransferRequestDto FundsTransferRequestDto)
        {
            try
            {
                var command = new FundsTransferCommand { FundsTransfer = FundsTransferRequestDto };
                var validatedAccount = await _mediator.Send(command);
                return Ok(validatedAccount);
            }
            catch (Exception ex)
            {
                _logger.LogError("FundsTransfer Error: ", ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
