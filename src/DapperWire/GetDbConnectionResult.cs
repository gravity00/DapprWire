using System.Data.Common;

namespace DapperWire;

/// <summary>
/// Represents the result of a database connection retrieval.
/// </summary>
public struct GetDbConnectionResult
{
    /// <summary>
    /// Creates a new instance.
    /// </summary>
    /// <param name="connection">The database connection.</param>
    /// <param name="isNew"></param>
    /// <param name="wasOpen"></param>
    public GetDbConnectionResult(DbConnection connection, bool isNew, bool wasOpen)
    {
        Connection = connection;
        IsNew = isNew;
        WasOpen = wasOpen;
    }

    /// <summary>
    /// Gets the database connection.
    /// </summary>
    public DbConnection Connection { get; }

    /// <summary>
    /// Was the connection created by this call?
    /// </summary>
    public bool IsNew { get; }

    /// <summary>
    /// Was the connection open before this call?
    /// </summary>
    public bool WasOpen { get; }
}