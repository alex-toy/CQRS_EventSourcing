using CQRS.Core.Queries;
using Microsoft.AspNetCore.Mvc;
using Post.Common.DTOs;
using Post.Query.Api.DTOs;
using Post.Query.Api.Queries.Orders;
using Post.Query.Domain.Entities.Orders;

namespace Post.Query.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IQueryDispatcher<OrderDb> _queryDispatcher;

        public OrderController(ILogger<OrderController> logger, IQueryDispatcher<OrderDb> queryDispatcher)
        {
            _logger = logger;
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet("GetAllOrders")]
        public async Task<ActionResult> GetAllOrdersAsync()
        {
            try
            {
                var posts = await _queryDispatcher.HandleAsync(new GetAllOrdersQuery());
                return NormalResponse(posts);
            }
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSAGE = "Error while processing request to retrieve all orders!";
                return ErrorResponse(ex, SAFE_ERROR_MESSAGE);
            }
        }

        [HttpGet("GetOrderById/{orderId}")]
        public async Task<ActionResult> GetOrderByIdAsync(Guid orderId)
        {
            try
            {
                List<OrderDb>? posts = await _queryDispatcher.HandleAsync(new GetOrderByIdQuery { Id = orderId });

                if (posts is null || !posts.Any()) return NoContent();

                return Ok(new OrderDto
                {
                    Orders = posts,
                    Message = "Successfully returned order!"
                });
            }
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSAGE = "Error while processing request to find order by ID!";
                return ErrorResponse(ex, SAFE_ERROR_MESSAGE);
            }
        }

        //[HttpGet("GetPostsByAuthor/{author}")]
        //public async Task<ActionResult> GetPostsByAuthorAsync(string author)
        //{
        //    try
        //    {
        //        List<PostDb> posts = await _queryDispatcher.SendAsync(new GetPostsByAuthorQuery { Author = author });
        //        return NormalResponse(posts);
        //    }
        //    catch (Exception ex)
        //    {
        //        const string SAFE_ERROR_MESSAGE = "Error while processing request to find posts by author!";
        //        return ErrorResponse(ex, SAFE_ERROR_MESSAGE);
        //    }
        //}

        //[HttpGet("GetPostsWithComments")]
        //public async Task<ActionResult> GetPostsWithCommentsAsync()
        //{
        //    try
        //    {
        //        List<PostDb> posts = await _queryDispatcher.SendAsync(new GetPostsWithCommentsQuery());
        //        return NormalResponse(posts);
        //    }
        //    catch (Exception ex)
        //    {
        //        const string SAFE_ERROR_MESSAGE = "Error while processing request to find posts with comments!";
        //        return ErrorResponse(ex, SAFE_ERROR_MESSAGE);
        //    }
        //}

        //[HttpGet("GetPostsWithLikes/{numberOfLikes}")]
        //public async Task<ActionResult> GetPostsWithLikesAsync(int numberOfLikes)
        //{
        //    try
        //    {
        //        List<PostDb> posts = await _queryDispatcher.SendAsync(new GetPostsWithLikesQuery { NumberOfLikes = numberOfLikes });
        //        return NormalResponse(posts);
        //    }
        //    catch (Exception ex)
        //    {
        //        const string SAFE_ERROR_MESSAGE = "Error while processing request to find posts with likes!";
        //        return ErrorResponse(ex, SAFE_ERROR_MESSAGE);
        //    }
        //}

        private ActionResult NormalResponse(List<OrderDb> posts)
        {
            if (posts is null || !posts.Any()) return NoContent();

            int count = posts.Count;
            return Ok(new OrderDto
            {
                Orders = posts,
                Message = $"Successfully returned {count} post{(count > 1 ? "s" : string.Empty)}!"
            });
        }

        private ActionResult ErrorResponse(Exception ex, string safeErrorMessage)
        {
            _logger.LogError(ex, safeErrorMessage);

            return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
            {
                Message = safeErrorMessage
            });
        }
    }
}