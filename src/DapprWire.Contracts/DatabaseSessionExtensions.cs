using System.Data.Common;

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
    /// <returns>A task to be awaited for the transaction when started.</returns>
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
        return databaseSession.ExecuteAsync(sql, SqlOptions.None, ct);
    }

    /// <summary>
    /// Executes a SQL command with the specified options.
    /// </summary>
    /// <param name="databaseSession">The database instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <param name="parameters">The SQL command parameters.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the result</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Task<int> ExecuteAsync(
        this IDatabaseSession databaseSession,
        string sql,
        object parameters,
        CancellationToken ct
    )
    {
        EnsureNotNull(databaseSession);
        return databaseSession.ExecuteAsync(sql, new SqlOptions
        {
            Parameters = parameters
        }, ct);
    }

    /// <summary>
    /// Executes a SQL command with the specified parameters and options,
    /// that returns a single result of type T.
    /// </summary>
    /// <param name="databaseSession">The database instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the result</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Task<T?> ExecuteScalarAsync<T>(
        this IDatabaseSession databaseSession,
        string sql,
        CancellationToken ct
    )
    {
        EnsureNotNull(databaseSession);
        return databaseSession.ExecuteScalarAsync<T>(sql, SqlOptions.None, ct);
    }

    /// <summary>
    /// Executes a SQL command with the specified parameters and options,
    /// that returns a single result of type T.
    /// </summary>
    /// <param name="databaseSession">The database instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <param name="parameters">The SQL command parameters.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the result</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Task<T?> ExecuteScalarAsync<T>(
        this IDatabaseSession databaseSession,
        string sql,
        object parameters,
        CancellationToken ct
    )
    {
        EnsureNotNull(databaseSession);
        return databaseSession.ExecuteScalarAsync<T>(sql, new SqlOptions
        {
            Parameters = parameters
        }, ct);
    }

    /// <summary>
    /// Executes a SQL command and returns a data reader for the results.
    /// </summary>
    /// <param name="databaseSession">The database instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the result</returns>
    public static Task<DbDataReader> ExecuteReaderAsync(
        this IDatabaseSession databaseSession,
        string sql,
        CancellationToken ct
    )
    {
        EnsureNotNull(databaseSession);
        return databaseSession.ExecuteReaderAsync(sql, SqlOptions.None, ct);
    }

    /// <summary>
    /// Executes a SQL command and returns a data reader for the results.
    /// </summary>
    /// <param name="databaseSession">The database instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <param name="parameters">The SQL command parameters.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the result</returns>
    public static Task<DbDataReader> ExecuteReaderAsync(
        this IDatabaseSession databaseSession,
        string sql,
        object parameters,
        CancellationToken ct
    )
    {
        EnsureNotNull(databaseSession);
        return databaseSession.ExecuteReaderAsync(sql, new SqlOptions
        {
            Parameters = parameters
        }, ct);
    }

    /// <summary>
    /// Executes a SQL command and returns a collection of results of type T.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="databaseSession">The database instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the result</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Task<IEnumerable<T>> QueryAsync<T>(
        this IDatabaseSession databaseSession,
        string sql,
        CancellationToken ct
    )
    {
        EnsureNotNull(databaseSession);
        return databaseSession.QueryAsync<T>(sql, SqlOptions.None, ct);
    }

    /// <summary>
    /// Executes a SQL command and returns a collection of results of type T.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="databaseSession">The database instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <param name="parameters">The SQL command parameters.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the result</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Task<IEnumerable<T>> QueryAsync<T>(
        this IDatabaseSession databaseSession,
        string sql,
        object parameters,
        CancellationToken ct
    )
    {
        EnsureNotNull(databaseSession);
        return databaseSession.QueryAsync<T>(sql, new SqlOptions
        {
            Parameters = parameters
        }, ct);
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
    public static Task<T> QuerySingleAsync<T>(
        this IDatabaseSession databaseSession,
        string sql,
        CancellationToken ct
    )
    {
        EnsureNotNull(databaseSession);
        return databaseSession.QuerySingleAsync<T>(sql, SqlOptions.None, ct);
    }

    /// <summary>
    /// Executes a SQL command and returns a single result of type T.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="databaseSession">The database instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <param name="parameters">The SQL command parameters.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the result</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Task<T> QuerySingleAsync<T>(
        this IDatabaseSession databaseSession,
        string sql,
        object parameters,
        CancellationToken ct
    )
    {
        EnsureNotNull(databaseSession);
        return databaseSession.QuerySingleAsync<T>(sql, new SqlOptions
        {
            Parameters = parameters
        }, ct);
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
        return databaseSession.QuerySingleOrDefaultAsync<T>(sql, SqlOptions.None, ct);
    }

    /// <summary>
    /// Executes a SQL command and returns a single result of type T.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="databaseSession">The database instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <param name="parameters">The SQL command parameters.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the result</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Task<T?> QuerySingleOrDefaultAsync<T>(
        this IDatabaseSession databaseSession,
        string sql,
        object parameters,
        CancellationToken ct
    )
    {
        EnsureNotNull(databaseSession);
        return databaseSession.QuerySingleOrDefaultAsync<T>(sql, new SqlOptions
        {
            Parameters = parameters
        }, ct);
    }

    /// <summary>
    /// Executes a SQL command and returns the first result of type T.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="databaseSession">The database instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the result</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Task<T> QueryFirstAsync<T>(
        this IDatabaseSession databaseSession,
        string sql,
        CancellationToken ct
    )
    {
        EnsureNotNull(databaseSession);
        return databaseSession.QueryFirstAsync<T>(sql, SqlOptions.None, ct);
    }

    /// <summary>
    /// Executes a SQL command and returns the first result of type T.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="databaseSession">The database instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <param name="parameters">The SQL command parameters.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the result</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Task<T> QueryFirstAsync<T>(
        this IDatabaseSession databaseSession,
        string sql,
        object parameters,
        CancellationToken ct
    )
    {
        EnsureNotNull(databaseSession);
        return databaseSession.QueryFirstAsync<T>(sql, new SqlOptions
        {
            Parameters = parameters
        }, ct);
    }

    /// <summary>
    /// Executes a SQL command and returns the first result of type T.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="databaseSession">The database instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the result</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Task<T?> QueryFirstOrDefaultAsync<T>(
        this IDatabaseSession databaseSession,
        string sql,
        CancellationToken ct
    )
    {
        EnsureNotNull(databaseSession);
        return databaseSession.QueryFirstOrDefaultAsync<T>(sql, SqlOptions.None, ct);
    }

    /// <summary>
    /// Executes a SQL command and returns the first result of type T.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="databaseSession">The database instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <param name="parameters">The SQL command parameters.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the result</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Task<T?> QueryFirstOrDefaultAsync<T>(
        this IDatabaseSession databaseSession,
        string sql,
        object parameters,
        CancellationToken ct
    )
    {
        EnsureNotNull(databaseSession);
        return databaseSession.QueryFirstOrDefaultAsync<T>(sql, new SqlOptions
        {
            Parameters = parameters
        }, ct);
    }

    /// <summary>
    /// Executes a SQL command and returns a grid reader for multiple result sets.
    /// </summary>
    /// <param name="databaseSession">The database instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the result</returns>
    public static Task<IDatabaseGridReader> QueryMultipleAsync(
        this IDatabaseSession databaseSession,
        string sql,
        CancellationToken ct
    )
    {
        EnsureNotNull(databaseSession);
        return databaseSession.QueryMultipleAsync(sql, SqlOptions.None, ct);
    }

    /// <summary>
    /// Executes a SQL command and returns a grid reader for multiple result sets.
    /// </summary>
    /// <param name="databaseSession">The database instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <param name="parameters">The SQL command parameters.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the result</returns>
    public static Task<IDatabaseGridReader> QueryMultipleAsync(
        this IDatabaseSession databaseSession,
        string sql,
        object parameters,
        CancellationToken ct
    )
    {
        EnsureNotNull(databaseSession);
        return databaseSession.QueryMultipleAsync(sql, new SqlOptions
        {
            Parameters = parameters
        }, ct);
    }

    private static void EnsureNotNull(IDatabaseSession databaseSession)
    {
        if (databaseSession is null)
            throw new ArgumentNullException(nameof(databaseSession));
    }
}