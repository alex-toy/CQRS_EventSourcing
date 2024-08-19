using CQRS.Core.Commands;

namespace Post.Command.Api.Commands.Discounts;

public class DeleteDiscountCommand : BaseCommand
{
    public Guid DiscountId { get; set; }
}
