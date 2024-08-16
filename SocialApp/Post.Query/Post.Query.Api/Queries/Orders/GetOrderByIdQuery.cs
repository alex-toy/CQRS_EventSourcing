using CQRS.Core.Queries;

namespace Post.Query.Api.Queries.Orders;

public class GetOrderByIdQuery : BaseQuery
{
    public Guid Id { get; set; }
}