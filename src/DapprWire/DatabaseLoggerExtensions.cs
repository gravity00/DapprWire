namespace DapprWire;

internal static class DatabaseLoggerExtensions
{
    extension(DatabaseLogger logger)
    {
        public void LogDebug<T>(
            string message,
            params object?[] args
        ) => logger.Log<T>(DatabaseLogLevel.Debug, null, message, args);

        public void LogInfo<T>(
            string message,
            params object?[] args
        ) => logger.Log<T>(DatabaseLogLevel.Info, null, message, args);
    }
}