using CQRS.Core.Commands;

namespace Post.Command.Api.Commands.Discounts;

public class UpdateDiscountsCommand : BaseCommand
{
    public int DiscountId { get; set; }
    public double LowerThreshold { get; set; }
    public double UpperThreshold { get; set; }
    public double Percentage { get; set; }
}
