using Post.Query.Api.Queries.Deliveries;
using Post.Query.Domain.Entities.Deliveries;
using Post.Query.Infrastructure.Repositories.Deliveries;

namespace Post.Query.Api.Handlers.Deliveries;

public class DeliveryQueryHandler : IDeliveryQueryHandler
{
    private readonly IDeliveryRepository _deliveryRepository;

    public DeliveryQueryHandler(IDeliveryRepository orderRepository)
    {
        _deliveryRepository = orderRepository;
    }

    public async Task<List<DeliveryDb>> HandleAsync(GetAllDeliveriesQuery query)
    {
        return await _deliveryRepository.GetAllAsync();
    }

    public async Task<List<DeliveryDb>> HandleAsync(GetDeliveryByIdQuery query)
    {
        DeliveryDb? order = await _deliveryRepository.GetByIdAsync(query.Id);
        return order is not null ? new List<DeliveryDb> { order } : new List<DeliveryDb>();
    }

    public async Task<List<DeliveryDb>> HandleAsync(GetDeliveriesWithOrdersQuery query)
    {
        return await _deliveryRepository.GetAllWithOrdersAsync();
    }
}
