using CQRS.Core.Commands;

namespace Post.Command.Api.Commands.Orders.Items;

public class DeleteItemCommand : BaseCommand
{
    public Guid ItemId { get; set; }
}
