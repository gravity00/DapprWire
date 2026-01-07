using Dapper;

namespace DapprWire;

/// <summary>
/// Represents a strongly-typed SQL runner for a database, allowing execution of SQL commands.
/// </summary>
/// <typeparam name="TName">The database name.</typeparam>
/// <param name="options">The database options.</param>
public abstract class DatabaseSqlRunner<TName>(
    DatabaseOptions options
) : IDatabaseSqlRunner<TName> where TName : IDatabaseName
{
    #region Execute

    /// <inheritdoc />
    public async Task<int> ExecuteAsync(
        string sql,
        SqlOptions sqlOptions,
        CancellationToken ct
    )
    {
        var command = CreateCommandDefinition(sql, sqlOptions, ct);
        LogCommandDefinition(command);

        var connection = await GetDbConnectionAsync(ct).ConfigureAwait(false);
        return await connection.ExecuteAsync(command).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public int Execute(
        string sql,
        SqlOptions sqlOptions
    )
    {
        var command = CreateCommandDefinition(sql, sqlOptions, CancellationToken.None);
        LogCommandDefinition(command);

        var connection = GetDbConnection();
        return connection.Execute(command);
    }

    #endregion

    #region ExecuteScalar

    /// <inheritdoc />
    public async Task<T?> ExecuteScalarAsync<T>(
        string sql,
        SqlOptions sqlOptions,
        CancellationToken ct
    )
    {
        var command = CreateCommandDefinition(sql, sqlOptions, ct);
        LogCommandDefinition(command);

        var connection = await GetDbConnectionAsync(ct).ConfigureAwait(false);
        return await connection.ExecuteScalarAsync<T>(command).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public T? ExecuteScalar<T>(
        string sql,
        SqlOptions sqlOptions
    )
    {
        var command = CreateCommandDefinition(sql, sqlOptions, CancellationToken.None);
        LogCommandDefinition(command);

        var connection = GetDbConnection();
        return connection.ExecuteScalar<T>(command);
    }

    #endregion

    #region ExecuteReader

    /// <inheritdoc />
    public async Task<DbDataReader> ExecuteReaderAsync(
        string sql,
        SqlOptions sqlOptions,
        CancellationToken ct
    )
    {
        var command = CreateCommandDefinition(sql, sqlOptions, ct);
        LogCommandDefinition(command);

        var connection = await GetDbConnectionAsync(ct).ConfigureAwait(false);
        return await connection.ExecuteReaderAsync(command).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public IDataReader ExecuteReader(
        string sql,
        SqlOptions sqlOptions
    )
    {
        var command = CreateCommandDefinition(sql, sqlOptions, CancellationToken.None);
        LogCommandDefinition(command);

        var connection = GetDbConnection();
        return connection.ExecuteReader(command);
    }

    #endregion

    #region Query

    /// <inheritdoc />
    public async Task<IReadOnlyCollection<T>> QueryAsync<T>(
        string sql,
        SqlOptions sqlOptions,
        CancellationToken ct
    )
    {
        var command = CreateCommandDefinition(sql, sqlOptions, ct);
        LogCommandDefinition(command);

        var connection = await GetDbConnectionAsync(ct).ConfigureAwait(false);
        var entries = await connection.QueryAsync<T>(command).ConfigureAwait(false);
        return entries.AsList();
    }

    /// <inheritdoc />
    public IReadOnlyCollection<T> Query<T>(
        string sql,
        SqlOptions sqlOptions
    )
    {
        var command = CreateCommandDefinition(sql, sqlOptions, CancellationToken.None);
        LogCommandDefinition(command);

        var connection = GetDbConnection();
        var entries = connection.Query<T>(command);
        return entries.AsList();
    }

    #endregion

    #region QuerySingle

    /// <inheritdoc />
    public async Task<T> QuerySingleAsync<T>(
        string sql,
        SqlOptions sqlOptions,
        CancellationToken ct
    )
    {
        var command = CreateCommandDefinition(sql, sqlOptions, ct);
        LogCommandDefinition(command);

        var connection = await GetDbConnectionAsync(ct).ConfigureAwait(false);
        return await connection.QuerySingleAsync<T>(command).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public T QuerySingle<T>(
        string sql,
        SqlOptions sqlOptions
    )
    {
        var command = CreateCommandDefinition(sql, sqlOptions, CancellationToken.None);
        LogCommandDefinition(command);

        var connection = GetDbConnection();
        return connection.QuerySingle<T>(command);
    }

    #endregion

    #region QuerySingleOrDefault

    /// <inheritdoc />
    public async Task<T?> QuerySingleOrDefaultAsync<T>(
        string sql,
        SqlOptions sqlOptions,
        CancellationToken ct
    )
    {
        var command = CreateCommandDefinition(sql, sqlOptions, ct);
        LogCommandDefinition(command);

        var connection = await GetDbConnectionAsync(ct).ConfigureAwait(false);
        return await connection.QuerySingleOrDefaultAsync<T>(command).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public T? QuerySingleOrDefault<T>(
        string sql,
        SqlOptions sqlOptions
    )
    {
        var command = CreateCommandDefinition(sql, sqlOptions, CancellationToken.None);
        LogCommandDefinition(command);

        var connection = GetDbConnection();
        return connection.QuerySingleOrDefault<T>(command);
    }

    #endregion

    #region QueryFirst

    /// <inheritdoc />
    public async Task<T> QueryFirstAsync<T>(
        string sql,
        SqlOptions sqlOptions,
        CancellationToken ct
    )
    {
        var command = CreateCommandDefinition(sql, sqlOptions, ct);
        LogCommandDefinition(command);

        var connection = await GetDbConnectionAsync(ct).ConfigureAwait(false);
        return await connection.QueryFirstAsync<T>(command).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public T QueryFirst<T>(
        string sql,
        SqlOptions sqlOptions
    )
    {
        var command = CreateCommandDefinition(sql, sqlOptions, CancellationToken.None);
        LogCommandDefinition(command);

        var connection = GetDbConnection();
        return connection.QueryFirst<T>(command);
    }

    #endregion

    #region QueryFirstOrDefault

    /// <inheritdoc />
    public async Task<T?> QueryFirstOrDefaultAsync<T>(
        string sql,
        SqlOptions sqlOptions,
        CancellationToken ct
    )
    {
        var command = CreateCommandDefinition(sql, sqlOptions, ct);
        LogCommandDefinition(command);

        var connection = await GetDbConnectionAsync(ct).ConfigureAwait(false);
        return await connection.QueryFirstOrDefaultAsync<T>(command).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public T? QueryFirstOrDefault<T>(
        string sql,
        SqlOptions sqlOptions
    )
    {
        var command = CreateCommandDefinition(sql, sqlOptions, CancellationToken.None);
        LogCommandDefinition(command);

        var connection = GetDbConnection();
        return connection.QueryFirstOrDefault<T>(command);
    }

    #endregion

    #region QueryMultiple

    /// <inheritdoc />
    public async Task<IDatabaseGridReader> QueryMultipleAsync(
        string sql,
        SqlOptions sqlOptions,
        CancellationToken ct
    )
    {
        var command = CreateCommandDefinition(sql, sqlOptions, ct);
        LogCommandDefinition(command);

        var connection = await GetDbConnectionAsync(ct).ConfigureAwait(false);
        var gridReader = await connection.QueryMultipleAsync(command).ConfigureAwait(false);
        return new DatabaseGridReader(gridReader);
    }

    /// <inheritdoc />
    public IDatabaseGridReader QueryMultiple(
        string sql,
        SqlOptions sqlOptions
    )
    {
        var command = CreateCommandDefinition(sql, sqlOptions, CancellationToken.None);
        LogCommandDefinition(command);

        var connection = GetDbConnection();
        var gridReader = connection.QueryMultiple(command);
        return new DatabaseGridReader(gridReader);
    }

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
    public async IAsyncEnumerable<T> QueryStreamed<T>(
        string sql,
        SqlOptions sqlOptions,
        [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct = default
    )
    {
        await using var reader = await ExecuteReaderAsync(sql, sqlOptions, ct).ConfigureAwait(false);
        var parser = reader.GetRowParser<T>();
        while (await reader.ReadAsync(ct).ConfigureAwait(false))
            yield return parser(reader);
    }

#endif

    #endregion

    /// <summary>
    /// Returns a database connection, that must be open.
    /// </summary>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the DbConnection instance.</returns>
#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
    protected abstract ValueTask<DbConnection> GetDbConnectionAsync(CancellationToken ct);
#else
    protected abstract Task<DbConnection> GetDbConnectionAsync(CancellationToken ct);
#endif

    /// <summary>
    /// Returns a database connection, that must be open.
    /// </summary>
    /// <returns>The DbConnection instance.</returns>
    protected abstract DbConnection GetDbConnection();

    /// <summary>
    /// Returns the current database transaction, if any.
    /// </summary>
    /// <returns>The DbTransaction instance.</returns>
    protected abstract DbTransaction? GetCurrentDbTransaction();

    private CommandDefinition CreateCommandDefinition(
        string sql,
        SqlOptions sqlOptions,
        CancellationToken ct
    ) => new(
        sql.NotNull(nameof(sql)),
        sqlOptions.Parameters,
        GetCurrentDbTransaction(),
        sqlOptions.Timeout ?? options.DefaultTimeout,
        sqlOptions.Type,
        cancellationToken: ct
    );

    private void LogCommandDefinition(CommandDefinition command)
    {
        if (options.Logger.IsEnabled<DatabaseSqlRunner<TName>>(DatabaseLogLevel.Debug))
        {
            options.Logger.LogDebug<DatabaseSqlRunner<TName>>(@"Executing SQL command
[HasParameters:{HasParameters} IsTransactional:{IsTransactional} Timeout:{CommandTimeout} Type:{CommandType}]
{CommandText}",
                command.Parameters is not null,
                command.Transaction is not null,
                command.CommandTimeout,
                command.CommandType,
                command.CommandText
            );
        }
    }
}