﻿using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities.Orders;
using Post.Query.Domain.Repositories.Orders;
using Post.Query.Infrastructure.Data;

namespace Post.Query.Infrastructure.Repositories.Orders;

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
        OrderDb? order = await GetByIdAsync(orderId);

        if (order is null) return;

        context.Orders.Remove(order);
        _ = await context.SaveChangesAsync();
    }

    public async Task<OrderDb?> GetByIdAsync(Guid postId)
    {
        using DatabaseContext context = _contextFactory.CreateDbContext();
        return await context.Orders
                .Include(i => i.Items)
                .Include(i => i.Discount)
                .FirstOrDefaultAsync(x => x.OrderId == postId);
    }

    public async Task<List<OrderDb>> GetAllAsync()
    {
        using DatabaseContext context = _contextFactory.CreateDbContext();
        return await context.Orders.AsNoTracking()
                .Include(i => i.Items).AsNoTracking()
                .Include(i => i.Discount).AsNoTracking()
                .ToListAsync();
    }

    public async Task<List<OrderDb>> GetAllWithItemsAsync()
    {
        using DatabaseContext context = _contextFactory.CreateDbContext();
        return await context.Orders.AsNoTracking()
                .Include(i => i.Items).AsNoTracking()
                .Include(i => i.Discount)
                .Where(x => x.Items != null && x.Items.Any())
                .ToListAsync();
    }

    public async Task UpdateAsync(OrderDb post)
    {
        using DatabaseContext context = _contextFactory.CreateDbContext();
        context.Orders.Update(post);

        _ = await context.SaveChangesAsync();
    }
}