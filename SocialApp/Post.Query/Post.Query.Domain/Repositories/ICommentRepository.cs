using Post.Query.Domain.Entities.Posts;

namespace Post.Query.Domain.Repositories;

public interface ICommentRepository
{
    Task CreateAsync(CommentDb comment);
    Task<CommentDb> GetByIdAsync(Guid commentId);
    Task UpdateAsync(CommentDb comment);
    Task DeleteAsync(Guid commentId);
}