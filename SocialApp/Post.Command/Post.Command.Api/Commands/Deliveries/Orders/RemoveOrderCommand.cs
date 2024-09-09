using CQRS.Core.Commands;

namespace Post.Command.Api.Commands.Deliveries.Orders;

public class RemoveOrderCommand : BaseCommand
{
    public Guid OrderId { get; set; }
}
