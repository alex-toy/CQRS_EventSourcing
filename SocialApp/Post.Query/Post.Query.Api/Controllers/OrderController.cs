using CQRS.Core.Queries;
using Microsoft.AspNetCore.Mvc;
using Post.Common.DTOs;
using Post.Query.Api.Dtos;
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
                List<OrderDb> orders = await _queryDispatcher.HandleAsync(new GetAllOrdersQuery());
                return NormalResponse(orders);
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
                List<OrderDb>? orders = await _queryDispatcher.HandleAsync(new GetOrderByIdQuery { Id = orderId });

                if (orders is null || !orders.Any()) return NoContent();

                return Ok(new OrdersDto
                {
                    Orders = orders.Select(o => OrderDto.GetDto(o)).ToList(),
                    Message = "Successfully returned order!"
                });
            }
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSAGE = "Error while processing request to find order by ID!";
                return ErrorResponse(ex, SAFE_ERROR_MESSAGE);
            }
        }

        [HttpGet("GetOrdersWithItems")]
        public async Task<ActionResult> GetOrdersWithItemsAsync()
        {
            try
            {
                List<OrderDb> orders = await _queryDispatcher.HandleAsync(new GetOrdersWithItemsQuery());
                return NormalResponse(orders);
            }
            catch (Exception ex)
            {
                const string SAFE_ERROR_MESSAGE = "Error while processing request to find orders with items!";
                return ErrorResponse(ex, SAFE_ERROR_MESSAGE);
            }
        }

        private ActionResult NormalResponse(List<OrderDb> orders)
        {
            if (orders is null || !orders.Any()) return NoContent();

            int count = orders.Count;
            return Ok(new OrdersDto
            {
                Orders = orders.Select(o => OrderDto.GetDto(o)).ToList(),
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