using Post.Query.Api.Queries.Orders;
using Post.Query.Domain.Entities.Orders;
using Post.Query.Domain.Repositories.Orders;

namespace Post.Query.Api.Handlers.Orders;

public class OrderQueryHandler : IOrderQueryHandler
{
    private readonly IOrderRepository _orderRepository;

    public OrderQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<List<OrderDb>> HandleAsync(GetAllOrdersQuery query)
    {
        return await _orderRepository.GetAllAsync();
    }

    public async Task<List<OrderDb>> HandleAsync(GetOrderByIdQuery query)
    {
        OrderDb order = await _orderRepository.GetByIdAsync(query.Id);
        return new List<OrderDb> { order };
    }

    public async Task<List<OrderDb>> HandleAsync(GetOrdersWithItemsQuery query)
    {
        return await _orderRepository.GetAllWithItemsAsync();
    }
}