using System.Data.Common;

namespace DapperWire;

/// <summary>
/// Represents options for configuring the database connection.
/// </summary>
public class DatabaseOptions
{
    /// <summary>
    /// Gets or sets the default timeout for database operations, in seconds.
    /// </summary>
    public int? DefaultTimeout { get; set; }

    /// <summary>
    /// Callback function that is invoked when a database connection is opened.
    /// </summary>
    public Func<DbConnection, CancellationToken, Task>? OnConnectionOpened { get; set; }
}