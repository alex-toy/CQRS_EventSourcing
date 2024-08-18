using CQRS.Core.Commands;

namespace Post.Command.Api.Commands.Orders;

public class DeleteOrderCommand : BaseCommand
{
    public string Author { get; set; }
}
