using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Post.Query.Domain.Entities.Posts;

[Table("Post", Schema = "dbo")]
public class PostDb : Entity
{
    [Key]
    public Guid PostId { get; set; }
    public string Author { get; set; } = string.Empty;
    public DateTime DatePosted { get; set; }
    public string Message { get; set; } = string.Empty;
    public int Likes { get; set; }
    public virtual ICollection<CommentDb> Comments { get; set; } = new List<CommentDb>();
}