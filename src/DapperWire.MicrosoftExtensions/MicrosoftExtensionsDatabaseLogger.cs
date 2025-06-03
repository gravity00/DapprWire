using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace DapperWire;

/// <summary>
/// Database logger implementation that uses Microsoft.Extensions.Logging.
/// </summary>
/// <param name="loggerFactory">The logger factory.</param>
public class MicrosoftExtensionsDatabaseLogger(
    ILoggerFactory loggerFactory
) : DatabaseLogger
{
    private readonly ConcurrentDictionary<Type, ILogger>  _loggerCache = new();

    /// <inheritdoc />
    public override void Log<T>(DatabaseLogLevel level, Exception? exception, string message, params object?[] args)
    {
        Map<T>(level, out var logger, out var logLevel);
        if (!logger.IsEnabled(logLevel)) 
            return;

        if (logLevel == LogLevel.Debug)
            logger.LogDebug(0, exception, message, args);
        else if (logLevel == LogLevel.Information)
            logger.LogInformation(0, exception, message, args);
        else if (logLevel == LogLevel.Warning)
            logger.LogWarning(0, exception, message, args);
        else if (logLevel == LogLevel.Error)
            logger.LogError(0, exception, message, args);
    }

    /// <inheritdoc />
    public override bool IsEnabled<T>(DatabaseLogLevel level)
    {
        Map<T>(level, out var logger, out var logLevel);
        return logger.IsEnabled(logLevel);
    }

    private void Map<T>(
        DatabaseLogLevel level,
        out ILogger logger,
        out LogLevel logLevel
    )
    {
        logger = _loggerCache.GetOrAdd(typeof(T), loggerFactory.CreateLogger);
        logLevel = level switch
        {
            DatabaseLogLevel.Debug => LogLevel.Debug,
            DatabaseLogLevel.Info => LogLevel.Information,
            DatabaseLogLevel.Warn => LogLevel.Warning,
            DatabaseLogLevel.Error => LogLevel.Error,
            _ => LogLevel.None
        };
    }
}