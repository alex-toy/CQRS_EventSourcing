using Post.Query.Domain.Entities.Orders;

namespace Post.Query.Api.Dtos;

public class DiscountDto
{
    public Guid DiscountId { get; set; }
    public double LowerThreshold { get; set; }
    public double UpperThreshold { get; set; }
    public double Percentage { get; set; }

    public static DiscountDto GetDto(DiscountDb discountDb)
    {
        return new DiscountDto
        {
            DiscountId = discountDb.DiscountId,
            LowerThreshold = discountDb.LowerThreshold,
            UpperThreshold = discountDb.UpperThreshold,
            Percentage = discountDb.Percentage
        };
    }
}
