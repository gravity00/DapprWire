namespace DapprWire;

/// <summary>
/// Represents a database.
/// </summary>
public interface IDatabase
{
    /// <summary>
    /// Connects to a database.
    /// </summary>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the database session</returns>
    Task<IDatabaseSession> ConnectAsync(CancellationToken ct);

    /// <summary>
    /// Connects to a database.
    /// </summary>
    /// <returns>The database session.</returns>
    IDatabaseSession Connect();
}

/// <summary>
/// Represents a strongly-typed database.
/// </summary>
/// <typeparam name="TName">The database name.</typeparam>
public interface IDatabase<TName> : IDatabase
    where TName : IDatabaseName;