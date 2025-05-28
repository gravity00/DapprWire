using System.Data;
using System.Data.Common;

namespace DapperWire;

/// <summary>
/// Represents a database connection.
/// </summary>
/// <param name="options">The database options.</param>
/// <param name="dbConnectionFactory">The <see cref="DbConnection"/> factory.</param>
public class Database(
    DatabaseOptions options,
    DbConnectionFactory dbConnectionFactory
) : IDatabase
{
    private bool _disposed;
    private DbConnection? _connection;

    #region Disposables

    ~Database() => Dispose(false);

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
            _connection?.Dispose();

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
        if (_connection is not null)
            await _connection.DisposeAsync().ConfigureAwait(false);

        _connection = null;
        _disposed = true;
    }

#endif

    #endregion

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
    protected virtual async Task<GetDbConnectionResult> GetDbConnectionAsync(CancellationToken ct)
    {
        EnsureNotDisposed();

        bool isNew;
        if (_connection == null)
        {
            isNew = true;
            _connection = await dbConnectionFactory();
        }
        else
            isNew = false;

        bool wasOpen;
        if (_connection.State is not ConnectionState.Open)
        {
            wasOpen = false;
            await _connection.OpenAsync(ct).ConfigureAwait(false);
        }
        else
            wasOpen = true;

        return new GetDbConnectionResult(_connection, isNew, wasOpen);
    }

    /// <summary>
    /// Ensures that the database connection has not been disposed.
    /// </summary>
    /// <exception cref="ObjectDisposedException"></exception>
    protected void EnsureNotDisposed()
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(Database));
    }
}

/// <summary>
/// Represents a strongly-typed database connection.
/// </summary>
/// <typeparam name="TName">The database name.</typeparam>
/// <param name="options">The database options.</param>
/// <param name="dbConnectionFactory">The strongly-typed <see cref="DbConnection"/> factory.</param>
public class Database<TName>(
    DatabaseOptions options,
    DbConnectionFactory<TName> dbConnectionFactory
) : Database(options , () => dbConnectionFactory()), IDatabase<TName>
    where TName : IDatabaseName;