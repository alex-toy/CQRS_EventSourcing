using CQRS.Core.Commands;

namespace Post.Command.Api.Commands.Discounts;

public class DeleteDiscountsCommand : BaseCommand
{
    public int DiscountId { get; set; }
}
