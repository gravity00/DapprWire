using System.Data;

namespace DapprWire;

/// <summary>
/// Represents a database connection.
/// </summary>
public interface IDatabaseSession : IDisposable
#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
    , IAsyncDisposable
#endif
{
    /// <summary>
    /// Starts a new database transaction.
    /// </summary>
    /// <param name="isolationLevel">The transaction isolation level.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the result.</returns>
    Task<IDatabaseTransaction> BeginTransactionAsync(
        IsolationLevel isolationLevel,
        CancellationToken ct
    );

    /// <summary>
    /// Executes a SQL command with the specified options.
    /// </summary>
    /// <param name="sql">The SQL command.</param>
    /// <param name="sqlOptions">The SQL options.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the result</returns>
    Task<int> ExecuteAsync(
        string sql,
        SqlOptions sqlOptions,
        CancellationToken ct
    );

    /// <summary>
    /// Executes a SQL command and returns a collection of results of type T.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="sql">The SQL command.</param>
    /// <param name="sqlOptions">The SQL options.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the result</returns>
    Task<IEnumerable<T>> QueryAsync<T>(
        string sql,
        SqlOptions sqlOptions,
        CancellationToken ct
    );

    /// <summary>
    /// Executes a SQL command and returns a single result of type T.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="sql">The SQL command.</param>
    /// <param name="sqlOptions">The SQL options.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the result</returns>
    Task<T?> QuerySingleOrDefaultAsync<T>(
        string sql,
        SqlOptions sqlOptions,
        CancellationToken ct
    );
}

/// <summary>
/// Represents a strongly-typed database connection.
/// </summary>
/// <typeparam name="TName">The database name.</typeparam>
public interface IDatabaseSession<TName> : IDatabaseSession
    where TName : IDatabaseName;