using Post.Query.Domain.Entities.Orders;

namespace Post.Query.Api.Dtos;

public class OrderDto
{
    public Guid OrderId { get; set; }
    public string Author { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Address { get; set; }
    public bool IsEmergency { get; set; }
    public double TotalPrice { get; set; }
    public virtual ICollection<ItemDb> Items { get; set; }

    public static OrderDto GetDto(OrderDb orderDb)
    {
        return new OrderDto
        {
            OrderId = orderDb.OrderId,
            Author = orderDb.Author,
            CreatedAt = orderDb.CreatedAt,
            Address = orderDb.Address,
            IsEmergency = orderDb.IsEmergency,
            Items = orderDb.Items,
            TotalPrice = orderDb.Items.Sum(i => i.Quantity * i.Price)
        };
    }
}
