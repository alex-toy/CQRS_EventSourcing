using Post.Query.Domain.Entities.Deliveries;

namespace Post.Query.Infrastructure.Repositories.Deliveries
{
    public interface IDeliveryRepository
    {
        Task CreateAsync(DeliveryDb order);
        Task DeleteAsync(Guid orderId);
        Task<List<DeliveryDb>> GetAllAsync();
        Task<List<DeliveryDb>> GetAllWithOrdersAsync();
        Task<DeliveryDb?> GetByIdAsync(Guid deliveryId);
        Task UpdateAsync(DeliveryDb post);
    }
}