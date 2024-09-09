using Post.Query.Domain.Entities.Deliveries;

namespace Post.Query.Api.Dtos;

public class DeliveryDto
{
    public Guid DeliveryId { get; set; }
    public string DriverName { get; set; } = string.Empty;
    public virtual IEnumerable<OrderDto> Orders { get; set; } = new List<OrderDto>();

    public static DeliveryDto GetDto(DeliveryDb delivery)
    {
        return new DeliveryDto()
        {
            DeliveryId = delivery.DeliveryId,
            DriverName = delivery.DriverName,
            Orders = delivery.Orders.Select(order => OrderDto.GetDto(order)).ToArray()
        };
    }
}
