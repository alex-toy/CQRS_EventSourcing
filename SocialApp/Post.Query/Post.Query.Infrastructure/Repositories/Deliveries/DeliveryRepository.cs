using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities.Deliveries;
using Post.Query.Domain.Entities.Orders;
using Post.Query.Infrastructure.Data;

namespace Post.Query.Infrastructure.Repositories.Deliveries;

public class DeliveryRepository : IDeliveryRepository
{
    private readonly DatabaseContextFactory _contextFactory;

    public DeliveryRepository(DatabaseContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task CreateAsync(DeliveryDb order)
    {
        using DatabaseContext context = _contextFactory.CreateDbContext();
        context.Deliveries.Add(order);

        _ = await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid orderId)
    {
        using DatabaseContext context = _contextFactory.CreateDbContext();
        DeliveryDb? order = await GetByIdAsync(orderId);

        if (order is null) return;

        context.Deliveries.Remove(order);
        _ = await context.SaveChangesAsync();
    }

    public async Task<DeliveryDb?> GetByIdAsync(Guid deliveryId)
    {
        using DatabaseContext context = _contextFactory.CreateDbContext();
        return await context.Deliveries
                .Include(i => i.Orders)
                .FirstOrDefaultAsync(x => x.DeliveryId == deliveryId);
    }

    public async Task<List<DeliveryDb>> GetAllAsync()
    {
        using DatabaseContext context = _contextFactory.CreateDbContext();
        return await context.Deliveries.AsNoTracking()
                .Include(i => i.Orders).AsNoTracking()
                .ToListAsync();
    }

    public async Task<List<DeliveryDb>> GetAllWithOrdersAsync()
    {
        using DatabaseContext context = _contextFactory.CreateDbContext();
        return await context.Deliveries.AsNoTracking()
                .Include(i => i.Orders).AsNoTracking()
                .Where(x => x.Orders != null && x.Orders.Any())
                .ToListAsync();
    }

    public async Task UpdateAsync(DeliveryDb post)
    {
        using DatabaseContext context = _contextFactory.CreateDbContext();
        context.Deliveries.Update(post);

        _ = await context.SaveChangesAsync();
    }
}
