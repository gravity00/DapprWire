namespace DapperWire;

/// <summary>
/// Delegate for logging database operations.
/// </summary>
/// <param name="type">The type that wrote this log.</param>
/// <param name="level">The log level.</param>
/// <param name="message">The log message.</param>
/// <param name="args">An optional message arguments.</param>
/// <param name="exception">An optional exception.</param>
public delegate void DatabaseLogger(
    Type type,
    DatabaseLogLevel level,
    Exception? exception,
    string message,
    params object?[] args
);