namespace DapprWire;

/// <summary>
/// Represents a strongly-typed database session.
/// </summary>
/// <typeparam name="TName">The database name.</typeparam>
/// <param name="options">The database options.</param>
/// <param name="dbConnectionFactory">The <see cref="DbConnection"/> factory.</param>
public class DatabaseSession<TName>(
    DatabaseOptions options,
    DbConnectionFactory<TName> dbConnectionFactory
) : DatabaseSqlRunner<TName>(options), IDatabaseSession<TName> where TName : IDatabaseName
{
    private readonly DatabaseOptions _options = options;
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
            isolationLevel = _options.DefaultIsolationLevel;

        _options.Logger.LogDebug<DatabaseSession<TName>>("Starting a new database transaction [IsolationLevel:{IsolationLevel}]", isolationLevel);
#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
        var transaction = await connection.BeginTransactionAsync(isolationLevel, ct).ConfigureAwait(false);
#else
        var transaction = connection.BeginTransaction(isolationLevel);
#endif

        _transaction = transaction;

        _options.Logger.LogInfo<DatabaseSession<TName>>("Database transaction started successfully", isolationLevel);

        return new DatabaseTransaction(_options, transaction, () =>
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
            isolationLevel = _options.DefaultIsolationLevel;

        _options.Logger.LogDebug<DatabaseSession<TName>>("Starting a new database transaction [IsolationLevel:{IsolationLevel}]", isolationLevel);
        var transaction = connection.BeginTransaction(isolationLevel);

        _transaction = transaction;

        _options.Logger.LogInfo<DatabaseSession<TName>>("Database transaction started successfully", isolationLevel);

        return new DatabaseTransaction(_options, transaction, () =>
        {
            _transaction = null;
        });
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

    /// <inheritdoc />
#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
    protected override async ValueTask<DbConnection> GetDbConnectionAsync(CancellationToken ct)
#else
    protected override async Task<DbConnection> GetDbConnectionAsync(CancellationToken ct)
#endif
    {
        EnsureNotDisposed();

        if (_connection is null)
        {
            _options.Logger.LogDebug<DatabaseSession<TName>>("Creating a new database connection...");
            _connection = dbConnectionFactory();
        }

        if (_connection.State is ConnectionState.Open)
            return _connection;

        _options.Logger.LogDebug<DatabaseSession<TName>>("Opening the database connection...");
        await _connection.OpenAsync(ct).ConfigureAwait(false);

        if (_options.OnConnectionOpen is not null)
            await _options.OnConnectionOpen(_connection, ct).ConfigureAwait(false);

        _options.Logger.LogInfo<DatabaseSession<TName>>("Database connection opened successfully.");

        return _connection;
    }

    /// <inheritdoc />
    protected override DbConnection GetDbConnection()
    {
        EnsureNotDisposed();

        if (_connection is null)
        {
            _options.Logger.LogDebug<DatabaseSession<TName>>("Creating a new database connection...");
            _connection = dbConnectionFactory();
        }

        if (_connection.State is ConnectionState.Open)
            return _connection;

        _options.Logger.LogDebug<DatabaseSession<TName>>("Opening the database connection...");
        _connection.Open();

        _options.OnConnectionOpen?.Invoke(_connection, CancellationToken.None)
            .ConfigureAwait(false)
            .GetAwaiter()
            .GetResult();

        _options.Logger.LogInfo<DatabaseSession<TName>>("Database connection opened successfully.");

        return _connection;
    }

    /// <inheritdoc />
    protected override DbTransaction? GetCurrentDbTransaction() => _transaction;

    private void EnsureNotDisposed()
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(DatabaseSession<TName>));
    }
}