using CQRS.Core.Queries;

namespace Post.Query.Api.Queries.Posts;

public class GetPostsByAuthorQuery : BaseQuery
{
    public string Author { get; set; }
}