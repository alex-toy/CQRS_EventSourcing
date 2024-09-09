using CQRS.Core.Commands;

namespace Post.Command.Api.Commands.Deliveries.Orders;

public class AddOrderCommand : BaseCommand
{
    public Guid OrderId { get; set; }
    public Guid DeliveryId { get; set; }
}
