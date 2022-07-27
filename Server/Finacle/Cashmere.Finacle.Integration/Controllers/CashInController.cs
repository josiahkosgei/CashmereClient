using Cashmere.Finacle.Integration.CQRS.Commands.ValidateAccount;
using Cashmere.Finacle.Integration.CQRS.DTOs.FundsTransfer;
using Cashmere.Finacle.Integration.CQRS.DTOs.ValidateAccount;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Cashmere.Finacle.Integration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CashInController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CashInController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("ValidateAccount", Name = "ValidateAccount")]
        [ProducesResponseType(typeof(ValidateAccountResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ValidateAccount([FromBody] ValidateAccountRequestDto validateAccountRequestDto)
        {
            try
            {
                var command = new ValidateAccountCommand { ValidateAccount = validateAccountRequestDto };
                var validatedAccount = await _mediator.Send(command);
                return Ok(validatedAccount);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("FundsTransfer", Name = "FundsTransfer")]
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
                return BadRequest(ex.Message);
            }
        }
    }
}
