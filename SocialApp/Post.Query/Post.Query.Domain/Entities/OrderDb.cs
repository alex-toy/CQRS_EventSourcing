using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Post.Query.Domain.Entities;

[Table("Order", Schema = "dbo")]
public class OrderDb : Entity
{
    [Key]
    public Guid OrderId { get; set; }
    public string Author { get; set; }
    public DateTime CreatedAt { get; set; }
    public virtual ICollection<ItemDb> Items { get; set; }
}