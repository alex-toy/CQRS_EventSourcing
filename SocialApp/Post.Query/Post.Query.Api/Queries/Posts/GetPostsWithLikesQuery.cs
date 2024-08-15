using CQRS.Core.Queries;

namespace Post.Query.Api.Queries.Posts;

public class GetPostsWithLikesQuery : BaseQuery
{
    public int NumberOfLikes { get; set; }
}