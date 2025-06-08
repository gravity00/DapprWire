namespace DapprWire;

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
    /// Gets or sets a function that is called when a database connection is opened.
    /// </summary>
    public Func<DbConnection, CancellationToken, Task>? OnConnectionOpen { get; set; }

    /// <summary>
    /// Gets or sets the default isolation level for database transactions.
    /// Defaults to <see cref="IsolationLevel.ReadCommitted"/>.
    /// </summary>
    public IsolationLevel DefaultIsolationLevel { get; set; } = IsolationLevel.ReadCommitted;
}