using CQRS.Core.Commands;
using CQRS.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Post.Command.Api.Commands.Orders;
using Post.Common.DTOs;

namespace Post.Cmd.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly ICommandDispatcher _commandDispatcher;

        public OrderController(ILogger<OrderController> logger, ICommandDispatcher commandDispatcher)
        {
            _logger = logger;
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost("CreateOrder")]
        public async Task<ActionResult> CreateOrderAsync(CreateOrderCommand command)
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

        [HttpDelete("DeleteOrder/{id}")]
        public async Task<ActionResult> DeleteOrderAsync(Guid id, DeleteOrderCommand command)
        {
            try
            {
                command.Id = id;
                await _commandDispatcher.SendAsync(command);

                return Ok(new BaseResponse
                {
                    Message = "Delete order request completed successfully!"
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
                const string SAFE_ERROR_MESSAGE = "Error while processing request to delete a post!";
                _logger.Log(LogLevel.Error, ex, SAFE_ERROR_MESSAGE);

                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
                {
                    Message = SAFE_ERROR_MESSAGE
                });
            }
        }

        //[HttpPut("LikePost/{id}")]
        //public async Task<ActionResult> LikePostAsync(Guid id)
        //{
        //    try
        //    {
        //        await _commandDispatcher.SendAsync(new LikePostCommand { Id = id });

        //        return Ok(new BaseResponse
        //        {
        //            Message = "Like post request completed successfully!"
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
        //        const string SAFE_ERROR_MESSAGE = "Error while processing request to like a post!";
        //        _logger.Log(LogLevel.Error, ex, SAFE_ERROR_MESSAGE);

        //        return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
        //        {
        //            Message = SAFE_ERROR_MESSAGE
        //        });
        //    }
        //}

        [HttpPut("UpdateOrder/{id}")]
        public async Task<ActionResult> UpdateOrderAsync(Guid id, UpdateOrderCommand command)
        {
            try
            {
                command.Id = id;
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

        //[HttpPut("AddComment/{id}")]
        //public async Task<ActionResult> AddCommentAsync(Guid id, CreateCommentCommand command)
        //{
        //    try
        //    {
        //        command.Id = id;
        //        await _commandDispatcher.SendAsync(command);

        //        return Ok(new BaseResponse
        //        {
        //            Message = "Add comment request completed successfully!"
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
        //        const string SAFE_ERROR_MESSAGE = "Error while processing request to add a comment to a post!";
        //        _logger.Log(LogLevel.Error, ex, SAFE_ERROR_MESSAGE);

        //        return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
        //        {
        //            Message = SAFE_ERROR_MESSAGE
        //        });
        //    }
        //}

        //[HttpPut("EditComment/{id}")]
        //public async Task<ActionResult> EditCommentAsync(Guid id, UpdateCommentCommand command)
        //{
        //    try
        //    {
        //        command.Id = id;
        //        await _commandDispatcher.SendAsync(command);

        //        return Ok(new BaseResponse
        //        {
        //            Message = "Edit comment request completed successfully!"
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
        //        const string SAFE_ERROR_MESSAGE = "Error while processing request to edit a comment on a post!";
        //        _logger.Log(LogLevel.Error, ex, SAFE_ERROR_MESSAGE);

        //        return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
        //        {
        //            Message = SAFE_ERROR_MESSAGE
        //        });
        //    }
        //}

        //[HttpDelete("RemoveComment/{id}")]
        //public async Task<ActionResult> RemoveCommentAsync(Guid id, DeleteCommentCommand command)
        //{
        //    try
        //    {
        //        command.Id = id;
        //        await _commandDispatcher.SendAsync(command);

        //        return Ok(new BaseResponse
        //        {
        //            Message = "Remove comment request completed successfully!"
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
        //        const string SAFE_ERROR_MESSAGE = "Error while processing request to remove a comment from a post!";
        //        _logger.Log(LogLevel.Error, ex, SAFE_ERROR_MESSAGE);

        //        return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
        //        {
        //            Message = SAFE_ERROR_MESSAGE
        //        });
        //    }
        //}
    }
}