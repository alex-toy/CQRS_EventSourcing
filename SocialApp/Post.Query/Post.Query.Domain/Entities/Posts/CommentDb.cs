﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Post.Query.Domain.Entities.Posts;

[Table("Comment", Schema = "dbo")]
public class CommentDb : Entity
{
    [Key]
    public Guid CommentId { get; set; }
    public string Username { get; set; }
    public DateTime CommentDate { get; set; }
    public string Comment { get; set; }
    public bool Edited { get; set; }
    public Guid PostId { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]
    public virtual PostDb Post { get; set; }
}