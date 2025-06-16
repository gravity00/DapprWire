using System.Data;
using System.Data.Common;

namespace DapprWire;

/// <summary>
/// Represents a database connection.
/// </summary>
public interface IDatabaseSession : IDisposable
#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
    , IAsyncDisposable
#endif
{
    #region BeginTransaction

    /// <summary>
    /// Starts a new database transaction.
    /// </summary>
    /// <param name="isolationLevel">The transaction isolation level.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the transaction to start.</returns>
    Task<IDatabaseTransaction> BeginTransactionAsync(
        IsolationLevel isolationLevel,
        CancellationToken ct
    );

    /// <summary>
    /// Starts a new database transaction.
    /// </summary>
    /// <param name="isolationLevel">The transaction isolation level.</param>
    /// <returns>The transaction when started.</returns>
    IDatabaseTransaction BeginTransaction(
        IsolationLevel isolationLevel
    );

    #endregion

    #region Execute

    /// <summary>
    /// Executes a SQL command with the specified options.
    /// </summary>
    /// <param name="sql">The SQL command.</param>
    /// <param name="sqlOptions">The SQL options.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the number of rows affected.</returns>
    Task<int> ExecuteAsync(
        string sql,
        SqlOptions sqlOptions,
        CancellationToken ct
    );

    /// <summary>
    /// Executes a SQL command with the specified options.
    /// </summary>
    /// <param name="sql">The SQL command.</param>
    /// <param name="sqlOptions">The SQL options.</param>
    /// <returns>The number of rows affected.</returns>
    int Execute(
        string sql,
        SqlOptions sqlOptions
    );

    #endregion

    #region ExecuteScalar

    /// <summary>
    /// Executes a SQL command with the specified parameters and options,
    /// that returns a single result of type T.
    /// </summary>
    /// <param name="sql">The SQL command.</param>
    /// <param name="sqlOptions">The SQL options.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the query results</returns>
    Task<T?> ExecuteScalarAsync<T>(
        string sql,
        SqlOptions sqlOptions,
        CancellationToken ct
    );

    /// <summary>
    /// Executes a SQL command with the specified parameters and options,
    /// that returns a single result of type T.
    /// </summary>
    /// <param name="sql">The SQL command.</param>
    /// <param name="sqlOptions">The SQL options.</param>
    /// <returns>The query result.</returns>
    T? ExecuteScalar<T>(
        string sql,
        SqlOptions sqlOptions
    );

    #endregion

    #region ExecuteReader

    /// <summary>
    /// Executes a SQL command and returns a data reader for the results.
    /// </summary>
    /// <param name="sql">The SQL command.</param>
    /// <param name="sqlOptions">The SQL options.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the data reader.</returns>
    Task<DbDataReader> ExecuteReaderAsync(
        string sql,
        SqlOptions sqlOptions,
        CancellationToken ct
    );

    /// <summary>
    /// Executes a SQL command and returns a data reader for the results.
    /// </summary>
    /// <param name="sql">The SQL command.</param>
    /// <param name="sqlOptions">The SQL options.</param>
    /// <returns>The data reader.</returns>
    IDataReader ExecuteReader(
        string sql,
        SqlOptions sqlOptions
    );

    #endregion

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
    Task<T> QuerySingleAsync<T>(
        string sql,
        SqlOptions sqlOptions,
        CancellationToken ct
    );

    #region QuerySingleOrDefault

    /// <summary>
    /// Executes a SQL command and returns a single result of type T.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="sql">The SQL command.</param>
    /// <param name="sqlOptions">The SQL options.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the query result.</returns>
    Task<T?> QuerySingleOrDefaultAsync<T>(
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
    /// <returns>The query result.</returns>
    T? QuerySingleOrDefault<T>(
        string sql,
        SqlOptions sqlOptions
    );

    #endregion

    /// <summary>
    /// Executes a SQL command and returns the first result of type T.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="sql">The SQL command.</param>
    /// <param name="sqlOptions">The SQL options.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the result</returns>
    Task<T> QueryFirstAsync<T>(
        string sql,
        SqlOptions sqlOptions,
        CancellationToken ct
    );

    /// <summary>
    /// Executes a SQL command and returns the first result of type T.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="sql">The SQL command.</param>
    /// <param name="sqlOptions">The SQL options.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the result</returns>
    Task<T?> QueryFirstOrDefaultAsync<T>(
        string sql,
        SqlOptions sqlOptions,
        CancellationToken ct
    );

    /// <summary>
    /// Executes a SQL command and returns a grid reader for multiple result sets.
    /// </summary>
    /// <param name="sql">The SQL command.</param>
    /// <param name="sqlOptions">The SQL options.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the result</returns>
    Task<IDatabaseGridReader> QueryMultipleAsync(
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