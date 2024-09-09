using CQRS.Core.Commands;

namespace Post.Command.Api.Commands.Deliveries;

public class DeleteDeliveryCommand : BaseCommand
{
    public Guid DeliveryId { get; set; }
}
