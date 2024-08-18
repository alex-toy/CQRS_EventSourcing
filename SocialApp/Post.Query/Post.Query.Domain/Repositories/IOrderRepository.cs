using Post.Query.Domain.Entities.Orders;

namespace Post.Query.Domain.Repositories;

public interface IOrderRepository
{
    Task CreateAsync(OrderDb post);
    Task UpdateAsync(OrderDb post);
    Task DeleteAsync(Guid postId);
    Task<OrderDb> GetByIdAsync(Guid postId);
    Task<List<OrderDb>> ListAllAsync();
}