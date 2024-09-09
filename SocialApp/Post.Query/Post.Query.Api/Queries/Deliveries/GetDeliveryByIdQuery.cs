using CQRS.Core.Queries;

namespace Post.Query.Api.Queries.Deliveries;

public class GetDeliveryByIdQuery : BaseQuery
{
    public Guid Id { get; set; }
}
