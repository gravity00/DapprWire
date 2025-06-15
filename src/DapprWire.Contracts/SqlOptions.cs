using System.Data;

namespace DapprWire;

/// <summary>
/// Options for executing a SQL command.
/// </summary>
public readonly record struct SqlOptions
{
    /// <summary>
    /// Returns an empty <see cref="SqlOptions"/> value, which can be used when no options are needed.
    /// </summary>
    /// <remarks>
    /// The <see cref="SqlOptions"/> returned by this property is equivalent to use the `default` keyword,
    /// where no parameters, timeout, or command type are specified.
    /// </remarks>
    public static SqlOptions None { get; } = new();

    /// <summary>
    /// The SQL command parameters.
    /// </summary>
    public object? Parameters { get; init; }

    /// <summary>
    /// The command timeout in seconds.
    /// </summary>
    public int? Timeout { get; init; }

    /// <summary>
    /// The type of command to execute.
    /// </summary>
    public CommandType? Type { get; init; }
}