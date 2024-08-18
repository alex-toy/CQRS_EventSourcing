using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities.Orders;
using Post.Query.Domain.Entities.Posts;

namespace Post.Query.Infrastructure.Data;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<PostDb> Posts { get; set; }
    public DbSet<CommentDb> Comments { get; set; }

    public DbSet<OrderDb> Orders { get; set; }
    public DbSet<ItemDb> Items { get; set; }
}