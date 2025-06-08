using Dapper;

namespace DapprWire;

/// <summary>
/// Represents a database connection.
/// </summary>
/// <param name="logger">The database logger.</param>
/// <param name="options">The database options.</param>
/// <param name="dbConnectionFactory">The <see cref="DbConnection"/> factory.</param>
public class DatabaseSession(
    DatabaseLogger logger,
    DatabaseOptions options,
    DbConnectionFactory dbConnectionFactory
) : IDatabaseSession
{
    private bool _disposed;
    private DbConnection? _connection;
    private DbTransaction? _transaction;

    #region Disposables

    /// <summary>
    /// Finalizes the database connection.
    /// </summary>
    ~DatabaseSession() => Dispose(false);

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes the database connection.
    /// </summary>
    /// <param name="disposing">Is the connection explicitly being disposed</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _transaction?.Dispose();
            _connection?.Dispose();
        }

        _transaction = null;
        _connection = null;
        _disposed = true;
    }

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore().ConfigureAwait(false);
        Dispose(false);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes the database connection asynchronously.
    /// </summary>
    /// <returns>A task to be awaited for the result.</returns>
    protected virtual async ValueTask DisposeAsyncCore()
    {
        if (_transaction is not null)
            await _transaction.DisposeAsync().ConfigureAwait(false);

        if (_connection is not null)
            await _connection.DisposeAsync().ConfigureAwait(false);

        _transaction = null;
        _connection = null;
        _disposed = true;
    }

#endif

    #endregion

    /// <inheritdoc />
    public async Task<IDatabaseTransaction> BeginTransactionAsync(
        IsolationLevel isolationLevel,
        CancellationToken ct
    )
    {
        EnsureNotDisposed();

        if (_transaction is not null)
            throw new InvalidOperationException("A transaction is already in progress.");

        var connection = await GetDbConnectionAsync(ct).ConfigureAwait(false);

        if (isolationLevel == default)
            isolationLevel = options.DefaultIsolationLevel;

        logger.LogDebug<DatabaseSession>("Starting a new database transaction [IsolationLevel:{IsolationLevel}]", isolationLevel);
#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
        var transaction = await connection.BeginTransactionAsync(isolationLevel, ct).ConfigureAwait(false);
#else
        var transaction = connection.BeginTransaction(isolationLevel);
#endif

        _transaction = transaction;

        logger.LogInfo<DatabaseSession>("Database transaction started successfully", isolationLevel);

        return new DatabaseTransaction(logger, transaction, () => { _transaction = null; });
    }

    /// <inheritdoc />
    public async Task<int> ExecuteAsync(
        string sql,
        SqlOptions sqlOptions,
        CancellationToken ct
    )
    {
        EnsureNotDisposed();

        var command = CreateCommandDefinition(sql, sqlOptions, ct);
        LogCommandDefinition(command);

        var connection = await GetDbConnectionAsync(ct).ConfigureAwait(false);
        return await connection.ExecuteAsync(command).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<T>> QueryAsync<T>(
        string sql,
        SqlOptions sqlOptions,
        CancellationToken ct
    )
    {
        EnsureNotDisposed();

        var command = CreateCommandDefinition(sql, sqlOptions, ct);
        LogCommandDefinition(command);

        var connection = await GetDbConnectionAsync(ct).ConfigureAwait(false);
        return await connection.QueryAsync<T>(command).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<T?> QuerySingleOrDefaultAsync<T>(
        string sql,
        SqlOptions sqlOptions,
        CancellationToken ct
    )
    {
        EnsureNotDisposed();

        var command = CreateCommandDefinition(sql, sqlOptions, ct);
        LogCommandDefinition(command);

        var connection = await GetDbConnectionAsync(ct).ConfigureAwait(false);
        return await connection.QuerySingleOrDefaultAsync<T>(command).ConfigureAwait(false);
    }

    /// <summary>
    /// Connects to the database asynchronously.
    /// </summary>
    /// <remarks>
    /// If the connection is already open, this method does nothing.
    /// </remarks>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the result.</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    public async Task ConnectAsync(CancellationToken ct) =>
        await GetDbConnectionAsync(ct).ConfigureAwait(false);

    /// <summary>
    /// Ensures the database connection is initialized and opens it if necessary.
    /// </summary>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the result.</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    protected async Task<DbConnection> GetDbConnectionAsync(CancellationToken ct)
    {
        EnsureNotDisposed();

        if (_connection is null)
        {
            logger.LogDebug<DatabaseSession>("Creating a new database connection...");
            _connection = dbConnectionFactory();
        }

        if (_connection.State is ConnectionState.Open)
            return _connection;

        logger.LogDebug<DatabaseSession>("Opening the database connection...");
        await _connection.OpenAsync(ct).ConfigureAwait(false);

        if (options.OnConnectionOpen is not null)
            await options.OnConnectionOpen(_connection, ct).ConfigureAwait(false);

        logger.LogInfo<DatabaseSession>("Database connection opened successfully.");

        return _connection;
    }

    /// <summary>
    /// Ensures that the database connection has not been disposed.
    /// </summary>
    /// <exception cref="ObjectDisposedException"></exception>
    protected void EnsureNotDisposed()
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(DatabaseSession));
    }

    private CommandDefinition CreateCommandDefinition(
        string sql,
        SqlOptions sqlOptions,
        CancellationToken ct
    ) => new(
        sql.NotNull(nameof(sql)),
        sqlOptions.Parameters,
        _transaction,
        sqlOptions.Timeout ?? options.DefaultTimeout,
        sqlOptions.Type,
        cancellationToken: ct
    );

    private void LogCommandDefinition(CommandDefinition command)
    {
        if (logger.IsEnabled<DatabaseSession>(DatabaseLogLevel.Debug))
        {
            logger.LogDebug<DatabaseSession>(@"Executing SQL command
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

/// <summary>
/// Represents a strongly-typed database connection.
/// </summary>
/// <typeparam name="TName">The database name.</typeparam>
/// <param name="logger">The database logger.</param>
/// <param name="options">The database options.</param>
/// <param name="dbConnectionFactory">The strongly-typed <see cref="DbConnection"/> factory.</param>
public class DatabaseSession<TName>(
    DatabaseLogger logger,
    DatabaseOptions options,
    DbConnectionFactory<TName> dbConnectionFactory
) : DatabaseSession(logger, options, () => dbConnectionFactory()), IDatabaseSession<TName>
    where TName : IDatabaseName;