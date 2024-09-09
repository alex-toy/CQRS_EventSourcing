namespace Post.Command.Domain.Rules;

public static class DeliveryRules
{

    public static void CheckDriverNameRule(this string driverName, string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(driverName))
        {
            throw new InvalidOperationException(errorMessage);
        }
    }
}
