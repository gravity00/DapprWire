namespace DapprWire;

internal static class DatabaseLoggerExtensions
{
    public static void LogDebug<T>(
        this DatabaseLogger logger,
        string message,
        params object?[] args
    ) => logger.Log<T>(DatabaseLogLevel.Debug, null, message, args);

    public static void LogInfo<T>(
        this DatabaseLogger logger,
        string message,
        params object?[] args
    ) => logger.Log<T>(DatabaseLogLevel.Info, null, message, args);
}