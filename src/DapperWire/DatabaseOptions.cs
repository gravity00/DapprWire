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
}