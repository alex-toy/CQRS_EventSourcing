using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Post.Query.Domain.Entities.Orders;

[Table("Discount", Schema = "dbo")]
public class DiscountDb : Entity
{
    [Key]
    public Guid DiscountId { get; set; }
    public double LowerThreshold { get; set; }
    public double UpperThreshold { get; set; }
    public double Percentage { get; set; }
    public Guid OrderId { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]
    public virtual OrderDb Order { get; set; }
}