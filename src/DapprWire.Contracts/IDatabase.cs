namespace DapprWire;

/// <summary>
/// Represents a factory for creating database connections.
/// </summary>
public interface IDatabase
{
    /// <summary>
    /// Creates and connects to a database.
    /// </summary>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the result</returns>
    Task<IDatabaseSession> ConnectAsync(CancellationToken ct);
}

/// <summary>
/// Represents a strongly-typed factory for creating database connections.
/// </summary>
/// <typeparam name="TName">The database name.</typeparam>
public interface IDatabase<TName> : IDatabase
    where TName : IDatabaseName;