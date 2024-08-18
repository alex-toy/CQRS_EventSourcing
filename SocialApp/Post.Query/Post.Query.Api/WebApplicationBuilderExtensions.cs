using CQRS.Core.Queries;
using Microsoft.EntityFrameworkCore;
using Post.Query.Api.Handlers.Orders;
using Post.Query.Api.Handlers.Posts;
using Post.Query.Api.Queries.Orders;
using Post.Query.Api.Queries.Posts;
using Post.Query.Domain.Entities.Orders;
using Post.Query.Domain.Entities.Posts;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.Data;
using Post.Query.Infrastructure.Dispatchers;
using Post.Query.Infrastructure.Handlers.Orders;
using Post.Query.Infrastructure.Handlers.Posts;
using Post.Query.Infrastructure.Repositories;

namespace Post.Query.Api;

public static class WebApplicationBuilderExtensions
{
    public static void ConfigureDatabase(this WebApplicationBuilder builder)
    {
        string SQLConnectionString = builder.Configuration.GetConnectionString("SqlServer")!;
        Action<DbContextOptionsBuilder> configureDbContext = o => o.UseLazyLoadingProxies().UseSqlServer(SQLConnectionString);
        builder.Services.AddDbContext<DatabaseContext>(configureDbContext);
        builder.Services.AddSingleton<DatabaseContextFactory>(new DatabaseContextFactory(configureDbContext));

        // create database and tables
        DatabaseContext dataContext = builder.Services.BuildServiceProvider().GetRequiredService<DatabaseContext>();
        dataContext.Database.EnsureCreated();
    }

    public static void ConfigurePosts(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IPostRepository, PostRepository>();
        builder.Services.AddScoped<ICommentRepository, CommentRepository>();

        builder.Services.AddScoped<IPostQueryHandler, PostQueryHandler>();

        builder.Services.AddScoped<IPostEventHandler, PostEventHandler>();

        QueryDispatcher<PostDb> postDispatcher = new();

        IPostQueryHandler postQueryHandler = builder.Services.BuildServiceProvider().GetRequiredService<IPostQueryHandler>();

        postDispatcher.RegisterHandler<GetAllPostsQuery>(postQueryHandler.HandleAsync);
        postDispatcher.RegisterHandler<GetPostByIdQuery>(postQueryHandler.HandleAsync);
        postDispatcher.RegisterHandler<GetPostsByAuthorQuery>(postQueryHandler.HandleAsync);
        postDispatcher.RegisterHandler<GetPostsWithCommentsQuery>(postQueryHandler.HandleAsync);
        postDispatcher.RegisterHandler<GetPostsWithLikesQuery>(postQueryHandler.HandleAsync);

        builder.Services.AddSingleton<IQueryDispatcher<PostDb>>(_ => postDispatcher);
    }

    public static void ConfigureOrders(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IOrderRepository, OrderRepository>();

        builder.Services.AddScoped<IOrderQueryHandler, OrderQueryHandler>();

        builder.Services.AddScoped<IOrderEventHandler, OrderEventHandler>();

        QueryDispatcher<OrderDb> orderDispatcher = new();

        IOrderQueryHandler orderQueryHandler = builder.Services.BuildServiceProvider().GetRequiredService<IOrderQueryHandler>();

        orderDispatcher.RegisterHandler<GetAllOrdersQuery>(orderQueryHandler.HandleAsync);
        orderDispatcher.RegisterHandler<GetOrderByIdQuery>(orderQueryHandler.HandleAsync);

        builder.Services.AddSingleton<IQueryDispatcher<OrderDb>>(_ => orderDispatcher);
    }

    public static void Configure(this WebApplicationBuilder builder)
    {
    }
}
