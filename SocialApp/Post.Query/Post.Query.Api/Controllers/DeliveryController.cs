using CQRS.Core.Queries;
using Microsoft.AspNetCore.Mvc;
using Post.Common.DTOs;
using Post.Query.Api.Dtos;
using Post.Query.Api.Queries.Deliveries;
using Post.Query.Domain.Entities.Deliveries;

namespace Post.Query.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DeliveryController : ControllerBase
{
    private readonly ILogger<DeliveryController> _logger;
    private readonly IQueryDispatcher<DeliveryDb> _queryDispatcher;

    public DeliveryController(ILogger<DeliveryController> logger, IQueryDispatcher<DeliveryDb> queryDispatcher)
    {
        _logger = logger;
        _queryDispatcher = queryDispatcher;
    }

    [HttpGet("GetAllDeliveries")]
    public async Task<ActionResult> GetAllDeliveriesAsync()
    {
        try
        {
            List<DeliveryDb> orders = await _queryDispatcher.HandleAsync(new GetAllDeliveriesQuery());
            return NormalResponse(orders);
        }
        catch (Exception ex)
        {
            const string SAFE_ERROR_MESSAGE = "Error while processing request to retrieve all deliveries!";
            return ErrorResponse(ex, SAFE_ERROR_MESSAGE);
        }
    }

    [HttpGet("GetDeliveryById/{deliveryId}")]
    public async Task<ActionResult> GetDeliveryByIdAsync(Guid deliveryId)
    {
        try
        {
            List<DeliveryDb>? deliveries = await _queryDispatcher.HandleAsync(new GetDeliveryByIdQuery { Id = deliveryId });

            if (deliveries is null || !deliveries.Any()) return NoContent();

            return Ok(new DeliveriesDto
            {
                Deliveries = deliveries.Select(o => DeliveryDto.GetDto(o)).ToList(),
                Message = "Successfully returned delivery!"
            });
        }
        catch (Exception ex)
        {
            const string SAFE_ERROR_MESSAGE = "Error while processing request to find delivery by ID!";
            return ErrorResponse(ex, SAFE_ERROR_MESSAGE);
        }
    }

    [HttpGet("GetOrdersWithItems")]
    public async Task<ActionResult> GetOrdersWithItemsAsync()
    {
        try
        {
            List<DeliveryDb> orders = await _queryDispatcher.HandleAsync(new GetDeliveriesWithOrdersQuery());
            return NormalResponse(orders);
        }
        catch (Exception ex)
        {
            const string SAFE_ERROR_MESSAGE = "Error while processing request to find deliveries with orders!";
            return ErrorResponse(ex, SAFE_ERROR_MESSAGE);
        }
    }

    private ActionResult NormalResponse(List<DeliveryDb> deliveries)
    {
        if (deliveries is null || !deliveries.Any()) return NoContent();

        int count = deliveries.Count;
        return Ok(new DeliveriesDto
        {
            Deliveries = deliveries.Select(delivery => DeliveryDto.GetDto(delivery)).ToList(),
            Message = $"Successfully returned {count} delivery{(count > 1 ? "s" : string.Empty)}!"
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
