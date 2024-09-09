using Post.Common.Comments;
using Post.Common.Events.Comments;
using Post.Common.Events.Posts;
using Post.Query.Domain.Entities.Posts;
using Post.Query.Domain.Repositories.Posts;

namespace Post.Query.Infrastructure.Handlers.Posts;

public class PostEventHandler : IPostEventHandler
{
    private readonly IPostRepository _postRepository;
    private readonly ICommentRepository _commentRepository;

    public PostEventHandler(IPostRepository postRepository, ICommentRepository commentRepository)
    {
        _postRepository = postRepository;
        _commentRepository = commentRepository;
    }

    public async Task On(PostCreatedEvent @event)
    {
        var post = new PostDb
        {
            PostId = @event.Id,
            Author = @event.Author,
            DatePosted = @event.DatePosted,
            Message = @event.Message
        };

        await _postRepository.CreateAsync(post);
    }

    public async Task On(PostUpdatedEvent @event)
    {
        PostDb post = await _postRepository.GetByIdAsync(@event.Id);

        if (post is null) return;

        post.Message = @event.Message;
        await _postRepository.UpdateAsync(post);
    }

    public async Task On(PostLikedEvent @event)
    {
        PostDb post = await _postRepository.GetByIdAsync(@event.Id);

        if (post is null) return;

        post.Likes++;
        await _postRepository.UpdateAsync(post);
    }

    public async Task On(CommentCreatedEvent @event)
    {
        var comment = new CommentDb
        {
            PostId = @event.Id,
            CommentId = @event.CommentId,
            CommentDate = @event.CommentDate,
            Comment = @event.Comment,
            Username = @event.Username,
            Edited = false
        };

        await _commentRepository.CreateAsync(comment);
    }

    public async Task On(CommentUpdatedEvent @event)
    {
        CommentDb? comment = await _commentRepository.GetByIdAsync(@event.CommentId);

        if (comment is null) return;

        comment.Comment = @event.Comment;
        comment.Edited = true;
        comment.CommentDate = @event.EditDate;

        await _commentRepository.UpdateAsync(comment);
    }

    public async Task On(CommentDeletedEvent @event)
    {
        await _commentRepository.DeleteAsync(@event.CommentId);
    }

    public async Task On(PostDeletedEvent @event)
    {
        await _postRepository.DeleteAsync(@event.Id);
    }
}