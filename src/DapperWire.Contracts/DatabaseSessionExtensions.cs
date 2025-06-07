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
        EnsureNotNull(databaseSession);
        return databaseSession.BeginTransactionAsync(default, ct);
    }

    /// <summary>
    /// Executes a SQL command with the specified options.
    /// </summary>
    /// <param name="databaseSession">The database instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the result</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Task<int> ExecuteAsync(
        this IDatabaseSession databaseSession,
        string sql,
        CancellationToken ct
    )
    {
        EnsureNotNull(databaseSession);
        return databaseSession.ExecuteAsync(sql, default, ct);
    }

    /// <summary>
    /// Executes a SQL command and returns a single result of type T.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="databaseSession">The database instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the result</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Task<T?> QuerySingleOrDefaultAsync<T>(
        this IDatabaseSession databaseSession,
        string sql,
        CancellationToken ct
    )
    {
        EnsureNotNull(databaseSession);
        return databaseSession.QuerySingleOrDefaultAsync<T>(sql, default, ct);
    }

    private static void EnsureNotNull(IDatabaseSession databaseSession)
    {
        if (databaseSession is null)
            throw new ArgumentNullException(nameof(databaseSession));
    }
}