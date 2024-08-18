using Post.Query.Api.Queries.Posts;
using Post.Query.Domain.Entities.Posts;
using Post.Query.Domain.Repositories.Posts;

namespace Post.Query.Api.Handlers.Posts;

public class PostQueryHandler : IPostQueryHandler
{
    private readonly IPostRepository _postRepository;

    public PostQueryHandler(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<List<PostDb>> HandleAsync(GetAllPostsQuery query)
    {
        return await _postRepository.GetAllAsync();
    }

    public async Task<List<PostDb>> HandleAsync(GetPostByIdQuery query)
    {
        var post = await _postRepository.GetByIdAsync(query.Id);
        return new List<PostDb> { post };
    }

    public async Task<List<PostDb>> HandleAsync(GetPostsByAuthorQuery query)
    {
        return await _postRepository.ListByAuthorAsync(query.Author);
    }

    public async Task<List<PostDb>> HandleAsync(GetPostsWithCommentsQuery query)
    {
        return await _postRepository.ListWithCommentsAsync();
    }

    public async Task<List<PostDb>> HandleAsync(GetPostsWithLikesQuery query)
    {
        return await _postRepository.ListWithLikesAsync(query.NumberOfLikes);
    }
}