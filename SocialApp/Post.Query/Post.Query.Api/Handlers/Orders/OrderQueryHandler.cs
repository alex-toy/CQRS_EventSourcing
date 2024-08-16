using Post.Query.Api.Queries.Orders;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;

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
        return await _orderRepository.ListAllAsync();
    }

    public async Task<List<OrderDb>> HandleAsync(GetOrderByIdQuery query)
    {
        var post = await _orderRepository.GetByIdAsync(query.Id);
        return new List<OrderDb> { post };
    }
}