namespace DapperWire;

internal static class DatabaseLoggerExtensions
{
    public static void Debug<T>(
        this DatabaseLogger logger,
        string message,
        params object?[] args
    ) => logger(typeof(T), DatabaseLogLevel.Debug, null, message, args);

    public static void Info<T>(
        this DatabaseLogger logger,
        string message,
        params object?[] args
    ) => logger(typeof(T), DatabaseLogLevel.Info, null, message, args);

    public static void Warn<T>(
        this DatabaseLogger logger,
        string message,
        params object?[] args
    ) => logger(typeof(T), DatabaseLogLevel.Warn, null, message, args);

    public static void Error<T>(
        this DatabaseLogger logger,
        Exception? exception,
        string message,
        params object?[] args
    ) => logger(typeof(T), DatabaseLogLevel.Error, exception, message, args);
}