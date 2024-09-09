using CQRS.Core.Commands;

namespace Post.Command.Api.Commands.Orders;

public class CreateOrderCommand : BaseCommand
{
    public string Author { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public bool IsEmergency { get; set; }
}
