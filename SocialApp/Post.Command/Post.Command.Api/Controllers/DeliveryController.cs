using CQRS.Core.Commands;
using CQRS.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Post.Command.Api.Commands.Deliveries;
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
    public async Task<ActionResult> CreateDeliveryAsync(CreateDeliveryCommand command)
    {
        Guid id = Guid.NewGuid();
        command.AggregateId = id;
        try
        {
            await _commandDispatcher.SendAsync(command);

            return StatusCode(StatusCodes.Status201Created, new
            {
                Id = id,
                Message = "New delivery creation request completed successfully!"
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
            const string SAFE_ERROR_MESSAGE = "Error while processing request to create a new delivery!";
            _logger.Log(LogLevel.Error, ex, SAFE_ERROR_MESSAGE);

            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                Id = id,
                Message = SAFE_ERROR_MESSAGE
            });
        }
    }

    [HttpPut("UpdateDelivery/{id}")]
    public async Task<ActionResult> UpdateDeliveryAsync(Guid id, UpdateDeliveryCommand command)
    {
        try
        {
            command.AggregateId = id;
            await _commandDispatcher.SendAsync(command);

            return Ok(new BaseResponse
            {
                Message = "Edit message request completed successfully!"
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
        catch (AggregateNotFoundException ex)
        {
            _logger.Log(LogLevel.Warning, ex, "Could not retrieve aggregate, client passed an incorrect post ID targetting the aggregate!");
            return BadRequest(new BaseResponse
            {
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            const string SAFE_ERROR_MESSAGE = "Error while processing request to edit a post!";
            _logger.Log(LogLevel.Error, ex, SAFE_ERROR_MESSAGE);

            return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
            {
                Message = SAFE_ERROR_MESSAGE
            });
        }
    }

    [HttpDelete("DeleteDelivery")]
    public async Task<ActionResult> DeleteDeliveryAsync(DeleteDeliveryCommand command)
    {
        try
        {
            await _commandDispatcher.SendAsync(command);

            return Ok(new BaseResponse
            {
                Message = "Delete Delivery request completed successfully!"
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
        catch (AggregateNotFoundException ex)
        {
            _logger.Log(LogLevel.Warning, ex, "Could not retrieve aggregate, client passed an incorrect post ID targetting the aggregate!");
            return BadRequest(new BaseResponse
            {
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            const string SAFE_ERROR_MESSAGE = "Error while processing request to delete a delivery!";
            _logger.Log(LogLevel.Error, ex, SAFE_ERROR_MESSAGE);

            return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
            {
                Message = SAFE_ERROR_MESSAGE
            });
        }
    }

    //[HttpPut("AddOrder/{id}")]
    //public async Task<ActionResult> AddOrderAsync(Guid id, CreateOrderCommand command)
    //{
    //    try
    //    {
    //        command.Id = id;
    //        await _commandDispatcher.SendAsync(command);

    //        return Ok(new BaseResponse
    //        {
    //            Message = "Add item request completed successfully!"
    //        });
    //    }
    //    catch (InvalidOperationException ex)
    //    {
    //        _logger.Log(LogLevel.Warning, ex, "Client made a bad request!");
    //        return BadRequest(new BaseResponse
    //        {
    //            Message = ex.Message
    //        });
    //    }
    //    catch (AggregateNotFoundException ex)
    //    {
    //        _logger.Log(LogLevel.Warning, ex, "Could not retrieve aggregate, client passed an incorrect post ID targetting the aggregate!");
    //        return BadRequest(new BaseResponse
    //        {
    //            Message = ex.Message
    //        });
    //    }
    //    catch (Exception ex)
    //    {
    //        const string SAFE_ERROR_MESSAGE = "Error while processing request to add an item to an Delivery!";
    //        _logger.Log(LogLevel.Error, ex, SAFE_ERROR_MESSAGE);

    //        return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
    //        {
    //            Message = SAFE_ERROR_MESSAGE
    //        });
    //    }
    //}
}
