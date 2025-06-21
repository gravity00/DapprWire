namespace DapprWire;

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
    /// <returns>A task to be awaited for the transaction to start.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Task<IDatabaseTransaction> BeginTransactionAsync(
        this IDatabaseSession databaseSession,
        CancellationToken ct
    )
    {
        EnsureNotNull(databaseSession);
        return databaseSession.BeginTransactionAsync(default, ct);
    }

    /// <summary>
    /// Starts a new database transaction.
    /// </summary>
    /// <param name="databaseSession">The database instance.</param>
    /// <returns>The transaction when started.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IDatabaseTransaction BeginTransaction(
        this IDatabaseSession databaseSession
    )
    {
        EnsureNotNull(databaseSession);
        return databaseSession.BeginTransaction(default);
    }

    private static void EnsureNotNull(IDatabaseSession databaseSession)
    {
        if (databaseSession is null)
            throw new ArgumentNullException(nameof(databaseSession));
    }
}