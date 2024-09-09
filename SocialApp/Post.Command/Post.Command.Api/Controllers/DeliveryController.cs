using CQRS.Core.Commands;
using Microsoft.AspNetCore.Mvc;
using Post.Command.Api.Commands.Orders;
using Post.Common.DTOs;

namespace Post.Command.Api.Controllers;

public class DeliveryController : ControllerBase
{
    private readonly ILogger<DeliveryController> _logger;
    private readonly ICommandDispatcher _commandDispatcher;

    public DeliveryController(ILogger<DeliveryController> logger, ICommandDispatcher commandDispatcher)
    {
        _logger = logger;
        _commandDispatcher = commandDispatcher;
    }

    [HttpPost("CreateDelivery")]
    public async Task<ActionResult> CreateDeliveryAsync(CreateOrderCommand command)
    {
        Guid id = Guid.NewGuid();
        command.Id = id;
        try
        {
            await _commandDispatcher.SendAsync(command);

            return StatusCode(StatusCodes.Status201Created, new
            {
                Id = id,
                Message = "New order creation request completed successfully!"
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.Log(LogLevel.Warning, ex, "Client made a bad request!");
            return BadRequest(new BaseResponse
            {
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            const string SAFE_ERROR_MESSAGE = "Error while processing request to create a new order!";
            _logger.Log(LogLevel.Error, ex, SAFE_ERROR_MESSAGE);

            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                Id = id,
                Message = SAFE_ERROR_MESSAGE
            });
        }
    }
}
