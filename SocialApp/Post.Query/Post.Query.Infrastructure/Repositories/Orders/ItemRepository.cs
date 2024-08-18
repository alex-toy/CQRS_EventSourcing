using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities.Orders;
using Post.Query.Domain.Repositories.Orders;
using Post.Query.Infrastructure.Data;

namespace Post.Query.Infrastructure.Repositories.Orders
{
    public class ItemRepository : IItemRepository
    {
        private readonly DatabaseContextFactory _contextFactory;

        public ItemRepository(DatabaseContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<ItemDb> GetByIdAsync(Guid itemId)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();

            return await context.Items.FirstOrDefaultAsync(x => x.ItemId == itemId);
        }

        public async Task UpdateAsync(ItemDb item)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            context.Items.Update(item);

            _ = await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid commentId)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            ItemDb? item = await GetByIdAsync(commentId);

            if (item is null) return;

            context.Items.Remove(item);
            _ = await context.SaveChangesAsync();
        }

        public async Task CreateAsync(ItemDb comment)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            context.Items.Add(comment);

            _ = await context.SaveChangesAsync();
        }
    }
}