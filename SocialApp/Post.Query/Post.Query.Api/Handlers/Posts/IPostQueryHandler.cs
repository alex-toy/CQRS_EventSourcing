using Post.Query.Api.Queries.Posts;
using Post.Query.Domain.Entities.Posts;

namespace Post.Query.Api.Handlers.Posts;

public interface IPostQueryHandler
{
    Task<List<PostDb>> HandleAsync(GetAllPostsQuery query);
    Task<List<PostDb>> HandleAsync(GetPostByIdQuery query);
    Task<List<PostDb>> HandleAsync(GetPostsByAuthorQuery query);
    Task<List<PostDb>> HandleAsync(GetPostsWithCommentsQuery query);
    Task<List<PostDb>> HandleAsync(GetPostsWithLikesQuery query);
}