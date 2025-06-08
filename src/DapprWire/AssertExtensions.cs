namespace DapprWire;

internal static class AssertExtensions
{
    public static T NotNull<T>(this T? value, string? paramName)
        where T : class
    {
        if (value is null)
            throw new ArgumentNullException(paramName);
        return value;
    }
}