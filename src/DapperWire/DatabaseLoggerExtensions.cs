namespace DapperWire;

internal static class DatabaseLoggerExtensions
{
    public static void Debug<T>(
        this DatabaseLogger logger,
        string message,
        params object?[] args
    ) => logger.Log<T>(DatabaseLogLevel.Debug, null, message, args);

    public static void Info<T>(
        this DatabaseLogger logger,
        string message,
        params object?[] args
    ) => logger.Log<T>(DatabaseLogLevel.Info, null, message, args);
}