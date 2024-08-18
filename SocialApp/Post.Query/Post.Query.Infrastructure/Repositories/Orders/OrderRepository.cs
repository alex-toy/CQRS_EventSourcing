using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities.Orders;
using Post.Query.Domain.Repositories.Orders;
using Post.Query.Infrastructure.Data;

namespace Post.Query.Infrastructure.Repositories.Orders
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DatabaseContextFactory _contextFactory;

        public OrderRepository(DatabaseContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task CreateAsync(OrderDb order)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            context.Orders.Add(order);

            _ = await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid orderId)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            OrderDb order = await GetByIdAsync(orderId);

            if (order is null) return;

            context.Orders.Remove(order);
            _ = await context.SaveChangesAsync();
        }

        public async Task<List<OrderDb>> ListByAuthorAsync(string author)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            return await context.Orders.AsNoTracking()
                    .Include(i => i.Items).AsNoTracking()
                    .Where(x => x.Author.Contains(author))
                    .ToListAsync();
        }

        public async Task<OrderDb> GetByIdAsync(Guid postId)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            return await context.Orders
                    .Include(i => i.Items)
                    .FirstOrDefaultAsync(x => x.OrderId == postId);
        }

        public async Task<List<OrderDb>> ListAllAsync()
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            return await context.Orders.AsNoTracking()
                    .Include(i => i.Items).AsNoTracking()
                    .ToListAsync();
        }

        public async Task<List<OrderDb>> ListWithCommentsAsync()
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            return await context.Orders.AsNoTracking()
                    .Include(i => i.Items).AsNoTracking()
                    .Where(x => x.Items != null && x.Items.Any())
                    .ToListAsync();
        }

        //public async Task<List<OrderDb>> ListWithLikesAsync(int numberOfLikes)
        //{
        //    using DatabaseContext context = _contextFactory.CreateDbContext();
        //    return await context.Orders.AsNoTracking()
        //            .Include(i => i.Items).AsNoTracking()
        //            .Where(x => x.Likes >= numberOfLikes)
        //            .ToListAsync();
        //}

        public async Task UpdateAsync(OrderDb post)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            context.Orders.Update(post);

            _ = await context.SaveChangesAsync();
        }
    }
}