using Post.Command.Domain.Bos;
using Post.Common.Events.Orders.Discounts;

namespace Post.Command.Domain.Rules;

internal static class OrderRules
{

    public static void CheckAuthorRule(this string author, string errorMessage)
    {
        if (!author.Equals(author, StringComparison.CurrentCultureIgnoreCase))
        {
            throw new InvalidOperationException(errorMessage);
        }
    }

    public static void CheckLabelRules(this string label, string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(label))
        {
            throw new InvalidOperationException();
        }
    }

    public static void CheckAddressRule(this string address, string errorMessage)
    {
        if (string.IsNullOrEmpty(address))
        {
            throw new InvalidOperationException(errorMessage);
        }
    }

    public static void CheckPriceRule(this double price, string errorMessage)
    {
        if (price <= 0)
        {
            throw new InvalidOperationException(errorMessage);
        }
    }

    public static void CheckDiscountUnicityRule(this DiscountBo discount)
    {
        if (discount is not null) throw new InvalidOperationException("You already have a discount!");
    }

    public static void CheckDiscountRules(this DiscountCreatedEvent discount)
    {
        CheckDiscountRules(discount.LowerThreshold, discount.UpperThreshold, discount.Percentage);
    }

    public static void CheckDiscountRules(this DiscountUpdatedEvent discount)
    {
        CheckDiscountRules(discount.LowerThreshold, discount.UpperThreshold, discount.Percentage);
    }

    private static void CheckDiscountRules(double lowerThreshold, double upperThreshold, double percentage)
    {
        if (lowerThreshold >= upperThreshold)
        {
            throw new InvalidOperationException("LowerThreshold should be less than UpperThreshold");
        }

        if (percentage < 0 || percentage > 1)
        {
            throw new InvalidOperationException("Percentage should be comprised between 0 and 1.");
        }
    }
}
