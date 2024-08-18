using Post.Query.Domain.Entities.Orders;

namespace Post.Query.Domain.Repositories.Orders;

public interface IItemRepository
{
    Task CreateAsync(ItemDb comment);
    Task<ItemDb> GetByIdAsync(Guid commentId);
    Task UpdateAsync(ItemDb comment);
    Task DeleteAsync(Guid commentId);
}