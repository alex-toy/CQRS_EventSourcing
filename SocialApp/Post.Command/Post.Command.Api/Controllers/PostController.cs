using CQRS.Core.Commands;
using CQRS.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Post.Command.Api.Commands.Comments;
using Post.Command.Api.Commands.Posts;
using Post.Common.DTOs;

namespace Post.Cmd.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly ILogger<PostController> _logger;
        private readonly ICommandDispatcher _commandDispatcher;

        public PostController(ILogger<PostController> logger, ICommandDispatcher commandDispatcher)
        {
            _logger = logger;
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost("CreatePost")]
        public async Task<ActionResult> CreatePostAsync(CreatePostCommand command)
        {
            var id = Guid.NewGuid();
            command.Id = id;
            try
            {
                await _commandDispatcher.SendAsync(command);

                return StatusCode(StatusCodes.Status201Created, new CreatePostCommand
                {
                    Id = id,
                    Message = "New post creation request completed successfully!"
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
                const string SAFE_ERROR_MESSAGE = "Error while processing request to create a new post!";
                _logger.Log(LogLevel.Error, ex, SAFE_ERROR_MESSAGE);

                return StatusCode(StatusCodes.Status500InternalServerError, new CreatePostCommand
                {
                    Id = id,
                    Message = SAFE_ERROR_MESSAGE
                });
            }
        }

        //[HttpDelete("{id}")]
        //public async Task<ActionResult> DeletePostAsync(Guid id, DeletePostCommand command)
        //{
        //    try
        //    {
        //        command.Id = id;
        //        await _commandDispatcher.SendAsync(command);

        //        return Ok(new BaseResponse
        //        {
        //            Message = "Delete post request completed successfully!"
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
        //        const string SAFE_ERROR_MESSAGE = "Error while processing request to delete a post!";
        //        _logger.Log(LogLevel.Error, ex, SAFE_ERROR_MESSAGE);

        //        return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
        //        {
        //            Message = SAFE_ERROR_MESSAGE
        //        });
        //    }
        //}

        //[HttpPut("{id}")]
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

        [HttpPut("UpdatePost/{id}")]
        public async Task<ActionResult> UpdatePostAsync(Guid id, UpdatePostCommand command)
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
                const string SAFE_ERROR_MESSAGE = "Error while processing request to edit the message of a post!";
                _logger.Log(LogLevel.Error, ex, SAFE_ERROR_MESSAGE);

                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
                {
                    Message = SAFE_ERROR_MESSAGE
                });
            }
        }

        //[HttpPut("{id}")]
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

        //[HttpPut("{id}")]
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

        //[HttpDelete("{id}")]
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