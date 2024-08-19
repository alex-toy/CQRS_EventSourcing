using Post.Query.Domain.Entities.Orders;

namespace Post.Query.Domain.Repositories.Orders;

public interface IDiscountRepository
{
    Task CreateAsync(DiscountDb discount);
    Task<DiscountDb> GetByIdAsync(Guid discountId);
    Task UpdateAsync(DiscountDb comment);
    Task DeleteAsync(Guid discountId);
}