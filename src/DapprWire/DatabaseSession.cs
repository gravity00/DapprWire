using Dapper;

namespace DapprWire;

/// <summary>
/// Represents a database connection.
/// </summary>
/// <param name="options">The database options.</param>
/// <param name="dbConnectionFactory">The <see cref="DbConnection"/> factory.</param>
public class DatabaseSession(
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

    #region BeginTransaction

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

        options.Logger.LogDebug<DatabaseSession>("Starting a new database transaction [IsolationLevel:{IsolationLevel}]", isolationLevel);
#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
        var transaction = await connection.BeginTransactionAsync(isolationLevel, ct).ConfigureAwait(false);
#else
        var transaction = connection.BeginTransaction(isolationLevel);
#endif

        _transaction = transaction;

        options.Logger.LogInfo<DatabaseSession>("Database transaction started successfully", isolationLevel);

        return new DatabaseTransaction(options, transaction, () =>
        {
            _transaction = null;
        });
    }

    /// <inheritdoc />
    public IDatabaseTransaction BeginTransaction(
        IsolationLevel isolationLevel
    )
    {
        EnsureNotDisposed();

        if (_transaction is not null)
            throw new InvalidOperationException("A transaction is already in progress.");

        var connection = GetDbConnection();

        if (isolationLevel == default)
            isolationLevel = options.DefaultIsolationLevel;

        options.Logger.LogDebug<DatabaseSession>("Starting a new database transaction [IsolationLevel:{IsolationLevel}]", isolationLevel);
        var transaction = connection.BeginTransaction(isolationLevel);

        _transaction = transaction;

        options.Logger.LogInfo<DatabaseSession>("Database transaction started successfully", isolationLevel);

        return new DatabaseTransaction(options, transaction, () =>
        {
            _transaction = null;
        });
    }

    #endregion

    #region Execute

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
    public int Execute(
        string sql,
        SqlOptions sqlOptions
    )
    {
        EnsureNotDisposed();

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
        EnsureNotDisposed();

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
        EnsureNotDisposed();

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
        EnsureNotDisposed();

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
        EnsureNotDisposed();

        var command = CreateCommandDefinition(sql, sqlOptions, CancellationToken.None);
        LogCommandDefinition(command);

        var connection = GetDbConnection();
        return connection.ExecuteReader(command);
    }

    #endregion

    #region Query

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
    public IEnumerable<T> Query<T>(
        string sql,
        SqlOptions sqlOptions
    )
    {
        EnsureNotDisposed();

        var command = CreateCommandDefinition(sql, sqlOptions, CancellationToken.None);
        LogCommandDefinition(command);

        var connection = GetDbConnection();
        return connection.Query<T>(command);
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
        EnsureNotDisposed();

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
        EnsureNotDisposed();

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
        EnsureNotDisposed();

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
        EnsureNotDisposed();

        var command = CreateCommandDefinition(sql, sqlOptions, CancellationToken.None);
        LogCommandDefinition(command);

        var connection = GetDbConnection();
        return connection.QuerySingleOrDefault<T>(command);
    }

    #endregion

    /// <inheritdoc />
    public async Task<T> QueryFirstAsync<T>(
        string sql,
        SqlOptions sqlOptions,
        CancellationToken ct
    )
    {
        EnsureNotDisposed();

        var command = CreateCommandDefinition(sql, sqlOptions, ct);
        LogCommandDefinition(command);

        var connection = await GetDbConnectionAsync(ct).ConfigureAwait(false);
        return await connection.QueryFirstAsync<T>(command).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<T?> QueryFirstOrDefaultAsync<T>(
        string sql,
        SqlOptions sqlOptions,
        CancellationToken ct
    )
    {
        EnsureNotDisposed();

        var command = CreateCommandDefinition(sql, sqlOptions, ct);
        LogCommandDefinition(command);

        var connection = await GetDbConnectionAsync(ct).ConfigureAwait(false);
        return await connection.QueryFirstOrDefaultAsync<T>(command).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IDatabaseGridReader> QueryMultipleAsync(
        string sql,
        SqlOptions sqlOptions,
        CancellationToken ct
    )
    {
        EnsureNotDisposed();

        var command = CreateCommandDefinition(sql, sqlOptions, ct);
        LogCommandDefinition(command);

        var connection = await GetDbConnectionAsync(ct).ConfigureAwait(false);
        var gridReader = await connection.QueryMultipleAsync(command).ConfigureAwait(false);
        return new DatabaseGridReader(gridReader);
    }

    /// <summary>
    /// Connects to the database.
    /// </summary>
    /// <remarks>
    /// If the connection is already open, this method does nothing.
    /// </remarks>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the connection to be open.</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    public async Task ConnectAsync(CancellationToken ct) =>
        await GetDbConnectionAsync(ct).ConfigureAwait(false);

    /// <summary>
    /// Connects to the database.
    /// </summary>
    /// <remarks>
    /// If the connection is already open, this method does nothing.
    /// </remarks>
    /// <exception cref="ObjectDisposedException"></exception>
    public void Connect() => GetDbConnection();

    /// <summary>
    /// Ensures the database connection is initialized and opens it if necessary.
    /// </summary>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the database connection.</returns>
    /// <exception cref="ObjectDisposedException"></exception>
#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
    protected async ValueTask<DbConnection> GetDbConnectionAsync(CancellationToken ct)
#else
    protected async Task<DbConnection> GetDbConnectionAsync(CancellationToken ct)
#endif
    {
        EnsureNotDisposed();

        if (_connection is null)
        {
            options.Logger.LogDebug<DatabaseSession>("Creating a new database connection...");
            _connection = dbConnectionFactory();
        }

        if (_connection.State is ConnectionState.Open)
            return _connection;

        options.Logger.LogDebug<DatabaseSession>("Opening the database connection...");
        await _connection.OpenAsync(ct).ConfigureAwait(false);

        if (options.OnConnectionOpen is not null)
            await options.OnConnectionOpen(_connection, ct).ConfigureAwait(false);

        options.Logger.LogInfo<DatabaseSession>("Database connection opened successfully.");

        return _connection;
    }

    /// <summary>
    /// Ensures the database connection is initialized and opens it if necessary.
    /// </summary>
    /// <returns>The database connection.</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    protected DbConnection GetDbConnection()
    {
        EnsureNotDisposed();

        if (_connection is null)
        {
            options.Logger.LogDebug<DatabaseSession>("Creating a new database connection...");
            _connection = dbConnectionFactory();
        }

        if (_connection.State is ConnectionState.Open)
            return _connection;

        options.Logger.LogDebug<DatabaseSession>("Opening the database connection...");
        _connection.Open();

        options.OnConnectionOpen?.Invoke(_connection, CancellationToken.None)
            .ConfigureAwait(false)
            .GetAwaiter()
            .GetResult();

        options.Logger.LogInfo<DatabaseSession>("Database connection opened successfully.");

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
        if (options.Logger.IsEnabled<DatabaseSession>(DatabaseLogLevel.Debug))
        {
            options.Logger.LogDebug<DatabaseSession>(@"Executing SQL command
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
/// <param name="options">The database options.</param>
/// <param name="dbConnectionFactory">The strongly-typed <see cref="DbConnection"/> factory.</param>
public class DatabaseSession<TName>(
    DatabaseOptions options,
    DbConnectionFactory<TName> dbConnectionFactory
) : DatabaseSession(options, () => dbConnectionFactory()), IDatabaseSession<TName>
    where TName : IDatabaseName;