using System.Data;

namespace DapperWire;

/// <summary>
/// Options for executing a SQL command.
/// </summary>
public readonly record struct SqlOptions
{
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