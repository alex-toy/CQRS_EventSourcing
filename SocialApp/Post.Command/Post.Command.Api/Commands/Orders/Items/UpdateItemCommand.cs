using CQRS.Core.Commands;

namespace Post.Command.Api.Commands.Orders.Items;

public class UpdateItemCommand : BaseCommand
{
    public Guid ItemId { get; set; }
    public string Label { get; set; }
    public double Price { get; set; }
    public int Quantity { get; set; }
}
