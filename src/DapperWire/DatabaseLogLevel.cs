namespace DapprWire;

/// <summary>
/// Database log levels.
/// </summary>
public enum DatabaseLogLevel
{
    /// <summary>
    /// Indicates that the log level is not defined.
    /// </summary>
    Undefined = 0,
    /// <summary>
    /// Indicates a debug log level.
    /// </summary>
    Debug,
    /// <summary>
    /// Indicates an informational log level.
    /// </summary>
    Info,
    /// <summary>
    /// Indicates a warning log level.
    /// </summary>
    Warn,
    /// <summary>
    /// Indicates an error log level.
    /// </summary>
    Error
}