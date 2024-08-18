using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Post.Query.Domain.Entities.Orders;

[Table("Item", Schema = "dbo")]
public class ItemDb : Entity
{
    [Key]
    public Guid ItemId { get; set; }
    public string Label { get; set; }
    public double Price { get; set; }
    public int Quantity { get; set; }
    public Guid OrderId { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]
    public virtual OrderDb Order { get; set; }
}