using Post.Query.Domain.Entities.Orders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Post.Query.Domain.Entities.Deliveries;

[Table("Delivery", Schema = "dbo")]
public class DeliveryDb : Entity
{
    [Key]
    public Guid DeliveryId { get; set; }
    public string DriverName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public virtual ICollection<OrderDb> Orders { get; set; } = new List<OrderDb>();
}
