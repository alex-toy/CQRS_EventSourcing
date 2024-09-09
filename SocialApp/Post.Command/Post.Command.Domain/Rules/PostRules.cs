namespace Post.Command.Domain.Rules;

internal static class PostRules
{
    public static void CheckAuthorRule(this string author, string username, string errorMessage)
    {
        if (!author.Equals(username, StringComparison.CurrentCultureIgnoreCase))
        {
            throw new InvalidOperationException(errorMessage);
        }
    }

    public static void CheckCommentRule(this string comment, string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(comment))
        {
            throw new InvalidOperationException(errorMessage);
        }
    }

    public static void CheckActiveRule(this bool active, string errorMessage)
    {
        if (!active)
        {
            throw new InvalidOperationException(errorMessage);
        }
    }
}
