namespace DapperWire;

/// <summary>
/// Represents a database connection.
/// </summary>
/// <param name="options">The database options.</param>
/// <param name="dbConnectionFactory">The <see cref="DbConnection"/> factory.</param>
public class DatabaseSession(
    IOptions<DatabaseOptions> options,
    DbConnectionFactory dbConnectionFactory
) : IDatabaseSession
{
    private bool _disposed;
    private DbConnection? _connection;
    private DbTransaction? _transaction;

    private DatabaseOptions Options => options.Value;

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
            isolationLevel = Options.DefaultIsolationLevel;

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
        var transaction = await connection.BeginTransactionAsync(isolationLevel, ct).ConfigureAwait(false);
#else
        var transaction = connection.BeginTransaction(isolationLevel);
#endif

        _transaction = transaction;

        return new DatabaseTransaction(transaction, () =>
        {
            _transaction = null;
        });
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
    public virtual async Task ConnectAsync(CancellationToken ct) => await GetDbConnectionAsync(ct).ConfigureAwait(false);

    /// <summary>
    /// Ensures the database connection is initialized and opens it if necessary.
    /// </summary>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the result.</returns>
    /// <exception cref="ObjectDisposedException"></exception>
    protected virtual async Task<DbConnection> GetDbConnectionAsync(CancellationToken ct)
    {
        EnsureNotDisposed();

        _connection ??= dbConnectionFactory();

        if (_connection.State is not ConnectionState.Open)
        {
            await _connection.OpenAsync(ct).ConfigureAwait(false);

            if (Options.OnConnectionOpen is not null)
                await Options.OnConnectionOpen(_connection, ct).ConfigureAwait(false);

        }

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
}

/// <summary>
/// Represents a strongly-typed database connection.
/// </summary>
/// <typeparam name="TName">The database name.</typeparam>
/// <param name="options">The database options.</param>
/// <param name="dbConnectionFactory">The strongly-typed <see cref="DbConnection"/> factory.</param>
public class DatabaseSession<TName>(
    IOptions<DatabaseOptions> options,
    DbConnectionFactory<TName> dbConnectionFactory
) : DatabaseSession(options, () => dbConnectionFactory()), IDatabaseSession<TName>
    where TName : IDatabaseName;