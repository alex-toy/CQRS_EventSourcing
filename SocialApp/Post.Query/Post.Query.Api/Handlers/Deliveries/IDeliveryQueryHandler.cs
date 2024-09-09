using Post.Query.Api.Queries.Deliveries;
using Post.Query.Domain.Entities.Deliveries;

namespace Post.Query.Api.Handlers.Deliveries;

public interface IDeliveryQueryHandler
{
    Task<List<DeliveryDb>> HandleAsync(GetAllDeliveriesQuery query);
    Task<List<DeliveryDb>> HandleAsync(GetDeliveriesWithOrdersQuery query);
    Task<List<DeliveryDb>> HandleAsync(GetDeliveryByIdQuery query);
}