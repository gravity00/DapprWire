using System.Data;
using System.Data.Common;

namespace DapprWire;

/// <summary>
/// Represents a SQL runner for a database, allowing execution of SQL commands.
/// </summary>
public interface IDatabaseSqlRunner
{
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

    #region Query

    /// <summary>
    /// Executes a SQL command and returns a collection of results of type T.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="sql">The SQL command.</param>
    /// <param name="sqlOptions">The SQL options.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the query results.</returns>
    Task<IEnumerable<T>> QueryAsync<T>(
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
    /// <returns>The query results.</returns>
    IEnumerable<T> Query<T>(
        string sql,
        SqlOptions sqlOptions
    );

    #endregion

    #region QuerySingle

    /// <summary>
    /// Executes a SQL command and returns a single result of type T.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="sql">The SQL command.</param>
    /// <param name="sqlOptions">The SQL options.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the query result.</returns>
    Task<T> QuerySingleAsync<T>(
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
    T QuerySingle<T>(
        string sql,
        SqlOptions sqlOptions
    );

    #endregion

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

    #region QueryFirst

    /// <summary>
    /// Executes a SQL command and returns the first result of type T.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="sql">The SQL command.</param>
    /// <param name="sqlOptions">The SQL options.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the query result.</returns>
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
    /// <returns>The query result.</returns>
    T QueryFirst<T>(
        string sql,
        SqlOptions sqlOptions
    );

    #endregion

    #region QueryFirstOrDefault

    /// <summary>
    /// Executes a SQL command and returns the first result of type T.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="sql">The SQL command.</param>
    /// <param name="sqlOptions">The SQL options.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the query result.</returns>
    Task<T?> QueryFirstOrDefaultAsync<T>(
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
    /// <returns>The query result.</returns>
    T? QueryFirstOrDefault<T>(
        string sql,
        SqlOptions sqlOptions
    );

    #endregion

    #region QueryMultiple

    /// <summary>
    /// Executes a SQL command and returns a grid reader for multiple result sets.
    /// </summary>
    /// <param name="sql">The SQL command.</param>
    /// <param name="sqlOptions">The SQL options.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the database grid reader.</returns>
    Task<IDatabaseGridReader> QueryMultipleAsync(
        string sql,
        SqlOptions sqlOptions,
        CancellationToken ct
    );

    /// <summary>
    /// Executes a SQL command and returns a grid reader for multiple result sets.
    /// </summary>
    /// <param name="sql">The SQL command.</param>
    /// <param name="sqlOptions">The SQL options.</param>
    /// <returns>The database grid reader.</returns>
    IDatabaseGridReader QueryMultiple(
        string sql,
        SqlOptions sqlOptions
    );

    #endregion

    #region QueryStreamed

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER

    /// <summary>
    /// Executes a SQL command and returns a streamed collection of results of type T.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="sql">The SQL command.</param>
    /// <param name="sqlOptions">The SQL options.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The async enumerator to stream each item asynchronously.</returns>
    IAsyncEnumerable<T> QueryStreamed<T>(
        string sql,
        SqlOptions sqlOptions,
        CancellationToken ct = default
    );

#endif

    #endregion
}

/// <summary>
/// Represents a strongly-typed SQL runner for a database, allowing execution of SQL commands.
/// </summary>
/// <typeparam name="TName">The database name.</typeparam>
public interface IDatabaseSqlRunner<TName> : IDatabaseSqlRunner
    where TName : IDatabaseName;