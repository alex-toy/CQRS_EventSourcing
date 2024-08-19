using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities.Orders;
using Post.Query.Domain.Repositories.Orders;
using Post.Query.Infrastructure.Data;

namespace Post.Query.Infrastructure.Repositories.Orders
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly DatabaseContextFactory _contextFactory;

        public DiscountRepository(DatabaseContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<DiscountDb> GetByIdAsync(Guid discountId)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();

            return await context.Discounts.FirstOrDefaultAsync(x => x.DiscountId == discountId);
        }

        public async Task UpdateAsync(DiscountDb item)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            context.Discounts.Update(item);

            _ = await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid commentId)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            DiscountDb? item = await GetByIdAsync(commentId);

            if (item is null) return;

            context.Discounts.Remove(item);
            _ = await context.SaveChangesAsync();
        }

        public async Task CreateAsync(DiscountDb comment)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            context.Discounts.Add(comment);

            _ = await context.SaveChangesAsync();
        }
    }
}