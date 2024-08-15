using CQRS.Core.Queries;

namespace Post.Query.Api.Queries.Posts;

public class GetPostByIdQuery : BaseQuery
{
    public Guid Id { get; set; }
}