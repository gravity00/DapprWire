namespace DapprWire;

internal static class AssertExtensions
{
    public static T NotNull<T>(
        this T? value,
        string paramName
    ) where T : class => value ?? throw new ArgumentNullException(paramName);
}