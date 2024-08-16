using CQRS.Core.Commands;

namespace Post.Command.Api.Commands.Orders;

public class CreateOrderCommand : BaseCommand
{
    public string Author { get; set; }
    public Guid ItemId { get; set; }
    public int Quantity { get; set; }
}
