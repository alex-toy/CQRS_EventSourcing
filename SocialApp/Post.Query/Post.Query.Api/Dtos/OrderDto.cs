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
    public DiscountDto Discount { get; set; }

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
            Discount = DiscountDto.GetDto(orderDb.Discount),
            TotalPrice = GetTotalPrice(orderDb)
        };
    }

    private static double GetTotalPrice(OrderDb orderDb)
    {
        double priceBeforeDiscount = orderDb.Items.Sum(i => i.Quantity * i.Price);

        if (orderDb.Discount is null) return priceBeforeDiscount;

        if (priceBeforeDiscount < orderDb.Discount.LowerThreshold) return priceBeforeDiscount;

        if (priceBeforeDiscount > orderDb.Discount.LowerThreshold && priceBeforeDiscount < orderDb.Discount.LowerThreshold)
        {
            double delta = priceBeforeDiscount - orderDb.Discount.LowerThreshold;
            return priceBeforeDiscount - delta * orderDb.Discount.Percentage;
        }

        return priceBeforeDiscount - (orderDb.Discount.UpperThreshold - orderDb.Discount.LowerThreshold) * orderDb.Discount.Percentage; ;
    }
}
