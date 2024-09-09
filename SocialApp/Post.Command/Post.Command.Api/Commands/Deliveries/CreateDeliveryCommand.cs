using CQRS.Core.Commands;

namespace Post.Command.Api.Commands.Deliveries;

public class CreateDeliveryCommand : BaseCommand
{
    public string DriverName { get; set; } = string.Empty;
}
