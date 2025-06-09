namespace DapprWire;

/// <summary>
/// Represents options for configuring the database connection.
/// </summary>
public class DatabaseOptions
{
    private DatabaseLogger _logger = DatabaseLogger.Null;

    /// <summary>
    /// Gets or sets the logger used for logging database operations.
    /// Defaults to <see cref="DatabaseLogger.Null"/>.
    /// </summary>
    public DatabaseLogger Logger
    {
        get => _logger;
        set => _logger = value.NotNull(nameof(value));
    }

    /// <summary>
    /// Gets or sets a function that is called when a database connection is opened.
    /// </summary>
    public Func<DbConnection, CancellationToken, Task>? OnConnectionOpen { get; set; }

    /// <summary>
    /// Gets or sets the default timeout for database operations, in seconds.
    /// </summary>
    public int? DefaultTimeout { get; set; }

    /// <summary>
    /// Gets or sets the default isolation level for database transactions.
    /// Defaults to <see cref="IsolationLevel.ReadCommitted"/>.
    /// </summary>
    public IsolationLevel DefaultIsolationLevel { get; set; } = IsolationLevel.ReadCommitted;
}