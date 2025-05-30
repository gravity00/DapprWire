namespace DapperWire;

/// <summary>
/// Provides extension methods for <see cref="IDatabaseSession"/> instances.
/// </summary>
public static class DatabaseSessionExtensions
{
    /// <summary>
    /// Starts a new database transaction.
    /// </summary>
    /// <param name="databaseSession">The database instance.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the result.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Task<IDatabaseTransaction> BeginTransactionAsync(
        this IDatabaseSession databaseSession,
        CancellationToken ct
    )
    {
        if (databaseSession == null) throw new ArgumentNullException(nameof(databaseSession));
        return databaseSession.BeginTransactionAsync(default, ct);
    }
}