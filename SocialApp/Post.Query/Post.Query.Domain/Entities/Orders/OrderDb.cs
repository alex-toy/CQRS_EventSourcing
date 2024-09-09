using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Post.Query.Domain.Entities.Orders;

[Table("Order", Schema = "dbo")]
public class OrderDb : Entity
{
    [Key]
    public Guid OrderId { get; set; }
    public string Author { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string Address { get; set; } = string.Empty;
    public bool IsEmergency { get; set; }
    public virtual ICollection<ItemDb> Items { get; set; } = new List<ItemDb>();
    public virtual DiscountDb Discount { get; set; } = new DiscountDb();
}