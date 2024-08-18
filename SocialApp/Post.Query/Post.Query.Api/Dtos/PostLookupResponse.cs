using Post.Common.DTOs;
using Post.Query.Domain.Entities.Posts;

namespace Post.Query.Api.DTOs;

public class PostLookupResponse : BaseResponse
{
    public List<PostDb> Posts { get; set; }
}