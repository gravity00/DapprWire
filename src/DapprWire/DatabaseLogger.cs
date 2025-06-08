namespace DapprWire;

/// <summary>
/// Represents a logger for database operations.
/// </summary>
public class DatabaseLogger
{
    /// <summary>
    /// Represents a null logger that does not log any messages.
    /// </summary>
    public static DatabaseLogger Null { get; } = new();

    /// <summary>
    /// Logs a message with the specified log level and optional exception for a given type.
    /// </summary>
    /// <typeparam name="T">The type that wrote this log.</typeparam>
    /// <param name="level">The log level.</param>
    /// <param name="exception">An optional exception.</param>
    /// <param name="message">The log message.</param>
    /// <param name="args">An optional message arguments.</param>
    public virtual void Log<T>(DatabaseLogLevel level, Exception? exception, string message, params object?[] args)
    {

    }

    /// <summary>
    /// Checks if a given log level is enabled for a specific type.
    /// </summary>
    /// <typeparam name="T">The type to check.</typeparam>
    /// <param name="level">The log level.</param>
    /// <returns></returns>
    public virtual bool IsEnabled<T>(DatabaseLogLevel level) => false;
}