using Post.Query.Domain.Entities;

namespace Post.Query.Domain.Repositories;

public interface IPostRepository
{
    Task CreateAsync(PostDb post);
    Task UpdateAsync(PostDb post);
    Task DeleteAsync(Guid postId);
    Task<PostDb> GetByIdAsync(Guid postId);
    Task<List<PostDb>> ListAllAsync();
    Task<List<PostDb>> ListByAuthorAsync(string author);
    Task<List<PostDb>> ListWithLikesAsync(int numberOfLikes);
    Task<List<PostDb>> ListWithCommentsAsync();
}