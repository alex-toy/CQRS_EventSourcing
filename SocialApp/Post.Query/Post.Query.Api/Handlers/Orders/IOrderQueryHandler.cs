using Post.Query.Api.Queries.Orders;
using Post.Query.Domain.Entities;

namespace Post.Query.Api.Handlers.Orders;

public interface IOrderQueryHandler
{
    Task<List<OrderDb>> HandleAsync(GetAllOrdersQuery query);
    Task<List<OrderDb>> HandleAsync(GetOrderByIdQuery query);
}