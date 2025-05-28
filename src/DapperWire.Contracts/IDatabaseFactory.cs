namespace DapperWire;

/// <summary>
/// Represents a factory for creating database connections.
/// </summary>
public interface IDatabaseFactory
{
    /// <summary>
    /// Creates and connects to a database.
    /// </summary>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the result</returns>
    Task<IDatabase> CreateAsync(CancellationToken ct);
}

/// <summary>
/// Represents a strongly-typed factory for creating database connections.
/// </summary>
/// <typeparam name="TName">The database name.</typeparam>
public interface IDatabaseFactory<TName> : IDatabaseFactory
    where TName : IDatabaseName;