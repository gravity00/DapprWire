using System.Data;
using System.Data.Common;

namespace DapprWire;

/// <summary>
/// Provides extension methods for <see cref="IDatabaseSqlRunner"/> instances.
/// </summary>
public static class DatabaseSqlRunnerExtensions
{
    #region Execute

    /// <summary>
    /// Executes a SQL command with the specified options.
    /// </summary>
    /// <param name="databaseSqlRunner">The database SQL runner instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the number of rows affected.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Task<int> ExecuteAsync(
        this IDatabaseSqlRunner databaseSqlRunner,
        string sql,
        CancellationToken ct
    )
    {
        EnsureNotNull(databaseSqlRunner);
        return databaseSqlRunner.ExecuteAsync(sql, SqlOptions.None, ct);
    }

    /// <summary>
    /// Executes a SQL command with the specified options.
    /// </summary>
    /// <param name="databaseSqlRunner">The database SQL runner instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <returns>The number of rows affected.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static int Execute(
        this IDatabaseSqlRunner databaseSqlRunner,
        string sql
    )
    {
        EnsureNotNull(databaseSqlRunner);
        return databaseSqlRunner.Execute(sql, SqlOptions.None);
    }

    /// <summary>
    /// Executes a SQL command with the specified options.
    /// </summary>
    /// <param name="databaseSqlRunner">The database SQL runner instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <param name="parameters">The SQL command parameters.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the number of rows affected.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Task<int> ExecuteAsync(
        this IDatabaseSqlRunner databaseSqlRunner,
        string sql,
        object parameters,
        CancellationToken ct
    )
    {
        EnsureNotNull(databaseSqlRunner);
        return databaseSqlRunner.ExecuteAsync(sql, new SqlOptions
        {
            Parameters = parameters
        }, ct);
    }

    /// <summary>
    /// Executes a SQL command with the specified options.
    /// </summary>
    /// <param name="databaseSqlRunner">The database SQL runner instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <param name="parameters">The SQL command parameters.</param>
    /// <returns>The number of rows affected.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static int Execute(
        this IDatabaseSqlRunner databaseSqlRunner,
        string sql,
        object parameters
    )
    {
        EnsureNotNull(databaseSqlRunner);
        return databaseSqlRunner.Execute(sql, new SqlOptions
        {
            Parameters = parameters
        });
    }

    #endregion

    #region ExecuteScalar

    /// <summary>
    /// Executes a SQL command with the specified parameters and options,
    /// that returns a single result of type T.
    /// </summary>
    /// <param name="databaseSqlRunner">The database SQL runner instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the query result.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Task<T?> ExecuteScalarAsync<T>(
        this IDatabaseSqlRunner databaseSqlRunner,
        string sql,
        CancellationToken ct
    )
    {
        EnsureNotNull(databaseSqlRunner);
        return databaseSqlRunner.ExecuteScalarAsync<T>(sql, SqlOptions.None, ct);
    }

    /// <summary>
    /// Executes a SQL command with the specified parameters and options,
    /// that returns a single result of type T.
    /// </summary>
    /// <param name="databaseSqlRunner">The database SQL runner instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <returns>The query result.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static T? ExecuteScalar<T>(
        this IDatabaseSqlRunner databaseSqlRunner,
        string sql
    )
    {
        EnsureNotNull(databaseSqlRunner);
        return databaseSqlRunner.ExecuteScalar<T>(sql, SqlOptions.None);
    }

    /// <summary>
    /// Executes a SQL command with the specified parameters and options,
    /// that returns a single result of type T.
    /// </summary>
    /// <param name="databaseSqlRunner">The database SQL runner instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <param name="parameters">The SQL command parameters.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The query result.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Task<T?> ExecuteScalarAsync<T>(
        this IDatabaseSqlRunner databaseSqlRunner,
        string sql,
        object parameters,
        CancellationToken ct
    )
    {
        EnsureNotNull(databaseSqlRunner);
        return databaseSqlRunner.ExecuteScalarAsync<T>(sql, new SqlOptions
        {
            Parameters = parameters
        }, ct);
    }

    /// <summary>
    /// Executes a SQL command with the specified parameters and options,
    /// that returns a single result of type T.
    /// </summary>
    /// <param name="databaseSqlRunner">The database SQL runner instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <param name="parameters">The SQL command parameters.</param>
    /// <returns>The query result.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static T? ExecuteScalar<T>(
        this IDatabaseSqlRunner databaseSqlRunner,
        string sql,
        object parameters
    )
    {
        EnsureNotNull(databaseSqlRunner);
        return databaseSqlRunner.ExecuteScalar<T>(sql, new SqlOptions
        {
            Parameters = parameters
        });
    }

    #endregion

    #region ExecuteReader

    /// <summary>
    /// Executes a SQL command and returns a data reader for the results.
    /// </summary>
    /// <param name="databaseSqlRunner">The database SQL runner instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the data reader.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Task<DbDataReader> ExecuteReaderAsync(
        this IDatabaseSqlRunner databaseSqlRunner,
        string sql,
        CancellationToken ct
    )
    {
        EnsureNotNull(databaseSqlRunner);
        return databaseSqlRunner.ExecuteReaderAsync(sql, SqlOptions.None, ct);
    }

    /// <summary>
    /// Executes a SQL command and returns a data reader for the results.
    /// </summary>
    /// <param name="databaseSqlRunner">The database SQL runner instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <returns>The data reader.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IDataReader ExecuteReader(
        this IDatabaseSqlRunner databaseSqlRunner,
        string sql
    )
    {
        EnsureNotNull(databaseSqlRunner);
        return databaseSqlRunner.ExecuteReader(sql, SqlOptions.None);
    }

    /// <summary>
    /// Executes a SQL command and returns a data reader for the results.
    /// </summary>
    /// <param name="databaseSqlRunner">The database SQL runner instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <param name="parameters">The SQL command parameters.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the data reader.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Task<DbDataReader> ExecuteReaderAsync(
        this IDatabaseSqlRunner databaseSqlRunner,
        string sql,
        object parameters,
        CancellationToken ct
    )
    {
        EnsureNotNull(databaseSqlRunner);
        return databaseSqlRunner.ExecuteReaderAsync(sql, new SqlOptions
        {
            Parameters = parameters
        }, ct);
    }

    /// <summary>
    /// Executes a SQL command and returns a data reader for the results.
    /// </summary>
    /// <param name="databaseSqlRunner">The database SQL runner instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <param name="parameters">The SQL command parameters.</param>
    /// <returns>A task to be awaited for the data reader.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IDataReader ExecuteReader(
        this IDatabaseSqlRunner databaseSqlRunner,
        string sql,
        object parameters
    )
    {
        EnsureNotNull(databaseSqlRunner);
        return databaseSqlRunner.ExecuteReader(sql, new SqlOptions
        {
            Parameters = parameters
        });
    }

    #endregion

    #region Query

    /// <summary>
    /// Executes a SQL command and returns a collection of results of type T.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="databaseSqlRunner">The database SQL runner instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the query results.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Task<IReadOnlyCollection<T>> QueryAsync<T>(
        this IDatabaseSqlRunner databaseSqlRunner,
        string sql,
        CancellationToken ct
    )
    {
        EnsureNotNull(databaseSqlRunner);
        return databaseSqlRunner.QueryAsync<T>(sql, SqlOptions.None, ct);
    }

    /// <summary>
    /// Executes a SQL command and returns a collection of results of type T.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="databaseSqlRunner">The database SQL runner instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <returns>The query results.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IReadOnlyCollection<T> Query<T>(
        this IDatabaseSqlRunner databaseSqlRunner,
        string sql
    )
    {
        EnsureNotNull(databaseSqlRunner);
        return databaseSqlRunner.Query<T>(sql, SqlOptions.None);
    }

    /// <summary>
    /// Executes a SQL command and returns a collection of results of type T.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="databaseSqlRunner">The database SQL runner instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <param name="parameters">The SQL command parameters.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the query results.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Task<IReadOnlyCollection<T>> QueryAsync<T>(
        this IDatabaseSqlRunner databaseSqlRunner,
        string sql,
        object parameters,
        CancellationToken ct
    )
    {
        EnsureNotNull(databaseSqlRunner);
        return databaseSqlRunner.QueryAsync<T>(sql, new SqlOptions
        {
            Parameters = parameters
        }, ct);
    }

    /// <summary>
    /// Executes a SQL command and returns a collection of results of type T.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="databaseSqlRunner">The database SQL runner instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <param name="parameters">The SQL command parameters.</param>
    /// <returns>The query results.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IReadOnlyCollection<T> Query<T>(
        this IDatabaseSqlRunner databaseSqlRunner,
        string sql,
        object parameters
    )
    {
        EnsureNotNull(databaseSqlRunner);
        return databaseSqlRunner.Query<T>(sql, new SqlOptions
        {
            Parameters = parameters
        });
    }

    #endregion

    #region QuerySingle

    /// <summary>
    /// Executes a SQL command and returns a single result of type T.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="databaseSqlRunner">The database SQL runner instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the query result.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Task<T> QuerySingleAsync<T>(
        this IDatabaseSqlRunner databaseSqlRunner,
        string sql,
        CancellationToken ct
    )
    {
        EnsureNotNull(databaseSqlRunner);
        return databaseSqlRunner.QuerySingleAsync<T>(sql, SqlOptions.None, ct);
    }

    /// <summary>
    /// Executes a SQL command and returns a single result of type T.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="databaseSqlRunner">The database SQL runner instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <returns>The query result.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static T QuerySingle<T>(
        this IDatabaseSqlRunner databaseSqlRunner,
        string sql
    )
    {
        EnsureNotNull(databaseSqlRunner);
        return databaseSqlRunner.QuerySingle<T>(sql, SqlOptions.None);
    }

    /// <summary>
    /// Executes a SQL command and returns a single result of type T.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="databaseSqlRunner">The database SQL runner instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <param name="parameters">The SQL command parameters.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the query result.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Task<T> QuerySingleAsync<T>(
        this IDatabaseSqlRunner databaseSqlRunner,
        string sql,
        object parameters,
        CancellationToken ct
    )
    {
        EnsureNotNull(databaseSqlRunner);
        return databaseSqlRunner.QuerySingleAsync<T>(sql, new SqlOptions
        {
            Parameters = parameters
        }, ct);
    }

    /// <summary>
    /// Executes a SQL command and returns a single result of type T.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="databaseSqlRunner">The database SQL runner instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <param name="parameters">The SQL command parameters.</param>
    /// <returns>The query result.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static T QuerySingle<T>(
        this IDatabaseSqlRunner databaseSqlRunner,
        string sql,
        object parameters
    )
    {
        EnsureNotNull(databaseSqlRunner);
        return databaseSqlRunner.QuerySingle<T>(sql, new SqlOptions
        {
            Parameters = parameters
        });
    }

    #endregion

    #region QuerySingleOrDefault

    /// <summary>
    /// Executes a SQL command and returns a single result of type T.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="databaseSqlRunner">The database SQL runner instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the query result.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Task<T?> QuerySingleOrDefaultAsync<T>(
        this IDatabaseSqlRunner databaseSqlRunner,
        string sql,
        CancellationToken ct
    )
    {
        EnsureNotNull(databaseSqlRunner);
        return databaseSqlRunner.QuerySingleOrDefaultAsync<T>(sql, SqlOptions.None, ct);
    }

    /// <summary>
    /// Executes a SQL command and returns a single result of type T.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="databaseSqlRunner">The database SQL runner instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <returns>The query result.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static T? QuerySingleOrDefault<T>(
        this IDatabaseSqlRunner databaseSqlRunner,
        string sql
    )
    {
        EnsureNotNull(databaseSqlRunner);
        return databaseSqlRunner.QuerySingleOrDefault<T>(sql, SqlOptions.None);
    }

    /// <summary>
    /// Executes a SQL command and returns a single result of type T.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="databaseSqlRunner">The database SQL runner instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <param name="parameters">The SQL command parameters.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the query result.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Task<T?> QuerySingleOrDefaultAsync<T>(
        this IDatabaseSqlRunner databaseSqlRunner,
        string sql,
        object parameters,
        CancellationToken ct
    )
    {
        EnsureNotNull(databaseSqlRunner);
        return databaseSqlRunner.QuerySingleOrDefaultAsync<T>(sql, new SqlOptions
        {
            Parameters = parameters
        }, ct);
    }

    /// <summary>
    /// Executes a SQL command and returns a single result of type T.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="databaseSqlRunner">The database SQL runner instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <param name="parameters">The SQL command parameters.</param>
    /// <returns>The query result.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static T? QuerySingleOrDefault<T>(
        this IDatabaseSqlRunner databaseSqlRunner,
        string sql,
        object parameters
    )
    {
        EnsureNotNull(databaseSqlRunner);
        return databaseSqlRunner.QuerySingleOrDefault<T>(sql, new SqlOptions
        {
            Parameters = parameters
        });
    }

    #endregion

    #region QueryFirst

    /// <summary>
    /// Executes a SQL command and returns the first result of type T.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="databaseSqlRunner">The database SQL runner instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the query result.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Task<T> QueryFirstAsync<T>(
        this IDatabaseSqlRunner databaseSqlRunner,
        string sql,
        CancellationToken ct
    )
    {
        EnsureNotNull(databaseSqlRunner);
        return databaseSqlRunner.QueryFirstAsync<T>(sql, SqlOptions.None, ct);
    }

    /// <summary>
    /// Executes a SQL command and returns the first result of type T.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="databaseSqlRunner">The database SQL runner instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <returns>The query result.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static T QueryFirst<T>(
        this IDatabaseSqlRunner databaseSqlRunner,
        string sql
    )
    {
        EnsureNotNull(databaseSqlRunner);
        return databaseSqlRunner.QueryFirst<T>(sql, SqlOptions.None);
    }

    /// <summary>
    /// Executes a SQL command and returns the first result of type T.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="databaseSqlRunner">The database SQL runner instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <param name="parameters">The SQL command parameters.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the query result.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Task<T> QueryFirstAsync<T>(
        this IDatabaseSqlRunner databaseSqlRunner,
        string sql,
        object parameters,
        CancellationToken ct
    )
    {
        EnsureNotNull(databaseSqlRunner);
        return databaseSqlRunner.QueryFirstAsync<T>(sql, new SqlOptions
        {
            Parameters = parameters
        }, ct);
    }

    /// <summary>
    /// Executes a SQL command and returns the first result of type T.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="databaseSqlRunner">The database SQL runner instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <param name="parameters">The SQL command parameters.</param>
    /// <returns>The query result.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static T QueryFirst<T>(
        this IDatabaseSqlRunner databaseSqlRunner,
        string sql,
        object parameters
    )
    {
        EnsureNotNull(databaseSqlRunner);
        return databaseSqlRunner.QueryFirst<T>(sql, new SqlOptions
        {
            Parameters = parameters
        });
    }

    #endregion

    #region QueryFirstOrDefault

    /// <summary>
    /// Executes a SQL command and returns the first result of type T.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="databaseSqlRunner">The database SQL runner instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the query result.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Task<T?> QueryFirstOrDefaultAsync<T>(
        this IDatabaseSqlRunner databaseSqlRunner,
        string sql,
        CancellationToken ct
    )
    {
        EnsureNotNull(databaseSqlRunner);
        return databaseSqlRunner.QueryFirstOrDefaultAsync<T>(sql, SqlOptions.None, ct);
    }

    /// <summary>
    /// Executes a SQL command and returns the first result of type T.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="databaseSqlRunner">The database SQL runner instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <returns>The query result.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static T? QueryFirstOrDefault<T>(
        this IDatabaseSqlRunner databaseSqlRunner,
        string sql
    )
    {
        EnsureNotNull(databaseSqlRunner);
        return databaseSqlRunner.QueryFirstOrDefault<T>(sql, SqlOptions.None);
    }

    /// <summary>
    /// Executes a SQL command and returns the first result of type T.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="databaseSqlRunner">The database SQL runner instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <param name="parameters">The SQL command parameters.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the query result.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Task<T?> QueryFirstOrDefaultAsync<T>(
        this IDatabaseSqlRunner databaseSqlRunner,
        string sql,
        object parameters,
        CancellationToken ct
    )
    {
        EnsureNotNull(databaseSqlRunner);
        return databaseSqlRunner.QueryFirstOrDefaultAsync<T>(sql, new SqlOptions
        {
            Parameters = parameters
        }, ct);
    }

    /// <summary>
    /// Executes a SQL command and returns the first result of type T.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="databaseSqlRunner">The database SQL runner instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <param name="parameters">The SQL command parameters.</param>
    /// <returns>The query result.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static T? QueryFirstOrDefault<T>(
        this IDatabaseSqlRunner databaseSqlRunner,
        string sql,
        object parameters
    )
    {
        EnsureNotNull(databaseSqlRunner);
        return databaseSqlRunner.QueryFirstOrDefault<T>(sql, new SqlOptions
        {
            Parameters = parameters
        });
    }

    #endregion

    #region QueryMultiple

    /// <summary>
    /// Executes a SQL command and returns a grid reader for multiple result sets.
    /// </summary>
    /// <param name="databaseSqlRunner">The database SQL runner instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the database grid reader.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Task<IDatabaseGridReader> QueryMultipleAsync(
        this IDatabaseSqlRunner databaseSqlRunner,
        string sql,
        CancellationToken ct
    )
    {
        EnsureNotNull(databaseSqlRunner);
        return databaseSqlRunner.QueryMultipleAsync(sql, SqlOptions.None, ct);
    }

    /// <summary>
    /// Executes a SQL command and returns a grid reader for multiple result sets.
    /// </summary>
    /// <param name="databaseSqlRunner">The database SQL runner instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <returns>The database grid reader.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IDatabaseGridReader QueryMultiple(
        this IDatabaseSqlRunner databaseSqlRunner,
        string sql
    )
    {
        EnsureNotNull(databaseSqlRunner);
        return databaseSqlRunner.QueryMultiple(sql, SqlOptions.None);
    }

    /// <summary>
    /// Executes a SQL command and returns a grid reader for multiple result sets.
    /// </summary>
    /// <param name="databaseSqlRunner">The database SQL runner instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <param name="parameters">The SQL command parameters.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the database grid reader.</returns>
    public static Task<IDatabaseGridReader> QueryMultipleAsync(
        this IDatabaseSqlRunner databaseSqlRunner,
        string sql,
        object parameters,
        CancellationToken ct
    )
    {
        EnsureNotNull(databaseSqlRunner);
        return databaseSqlRunner.QueryMultipleAsync(sql, new SqlOptions
        {
            Parameters = parameters
        }, ct);
    }

    /// <summary>
    /// Executes a SQL command and returns a grid reader for multiple result sets.
    /// </summary>
    /// <param name="databaseSqlRunner">The database SQL runner instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <param name="parameters">The SQL command parameters.</param>
    /// <returns>The database grid reader.</returns>
    public static IDatabaseGridReader QueryMultiple(
        this IDatabaseSqlRunner databaseSqlRunner,
        string sql,
        object parameters
    )
    {
        EnsureNotNull(databaseSqlRunner);
        return databaseSqlRunner.QueryMultiple(sql, new SqlOptions
        {
            Parameters = parameters
        });
    }

    #endregion

    #region QueryStreamed

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER

    /// <summary>
    /// Executes a SQL command and returns a streamed collection of results of type T.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="databaseSqlRunner">The database SQL runner instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The async enumerator to stream each item asynchronously.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static async IAsyncEnumerable<T> QueryStreamed<T>(
        this IDatabaseSqlRunner databaseSqlRunner,
        string sql,
        [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct = default
    )
    {
        EnsureNotNull(databaseSqlRunner);

        var sqlOptions = SqlOptions.None;
        await foreach (var item in databaseSqlRunner.QueryStreamed<T>(sql, sqlOptions, ct).ConfigureAwait(false))
            yield return item;
    }

    /// <summary>
    /// Executes a SQL command and returns a streamed collection of results of type T.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="databaseSqlRunner">The database SQL runner instance.</param>
    /// <param name="sql">The SQL command.</param>
    /// <param name="parameters">The SQL command parameters.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The async enumerator to stream each item asynchronously.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static async IAsyncEnumerable<T> QueryStreamed<T>(
        this IDatabaseSqlRunner databaseSqlRunner,
        string sql,
        object parameters,
        [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct = default
    )
    {
        EnsureNotNull(databaseSqlRunner);

        var sqlOptions = new SqlOptions
        {
            Parameters = parameters
        };
        await foreach (var item in databaseSqlRunner.QueryStreamed<T>(sql, sqlOptions, ct))
            yield return item;
    }

#endif

    #endregion

    private static void EnsureNotNull(IDatabaseSqlRunner databaseSqlRunner)
    {
        if (databaseSqlRunner is null)
            throw new ArgumentNullException(nameof(databaseSqlRunner));
    }
}