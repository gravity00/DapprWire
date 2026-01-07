using Dapper;

namespace DapprWire;

/// <summary>
/// Represents a reader for a grid of results from a database query.
/// </summary>
/// <param name="gridReader">The Dapper grid reader</param>
public class DatabaseGridReader(
    SqlMapper.GridReader gridReader
) : IDatabaseGridReader
{
    private SqlMapper.GridReader? _gridReader = gridReader;

    #region Disposables

    /// <summary>
    /// Finalizes the database grid reader.
    /// </summary>
    ~DatabaseGridReader() => Dispose(false);

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
            _gridReader?.Dispose();

        _gridReader = null;
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
#if NET5_0_OR_GREATER
    protected virtual async ValueTask DisposeAsyncCore()
    {
        if (_gridReader is not null)
            await _gridReader.DisposeAsync().ConfigureAwait(false);

        _gridReader = null;
    }
#else
    protected virtual ValueTask DisposeAsyncCore()
    {
        _gridReader?.Dispose();

        _gridReader = null;
        return default;
    }
#endif

#endif

    #endregion

    /// <inheritdoc />
    public async Task<IReadOnlyCollection<T>> ReadAsync<T>()
    {
        EnsureNotDisposed();
        var entries = await _gridReader!.ReadAsync<T>();
        return entries.AsList();
    }

    /// <inheritdoc />
    public IReadOnlyCollection<T> Read<T>()
    {
        EnsureNotDisposed();
        var entries = _gridReader!.Read<T>();
        return entries.AsList();
    }

    /// <inheritdoc />
    public Task<T> ReadFirstAsync<T>()
    {
        EnsureNotDisposed();
        return _gridReader!.ReadFirstAsync<T>();
    }

    /// <inheritdoc />
    public T ReadFirst<T>()
    {
        EnsureNotDisposed();
        return _gridReader!.ReadFirst<T>();
    }

    /// <inheritdoc />
    public Task<T?> ReadFirstOrDefaultAsync<T>()
    {
        EnsureNotDisposed();
        return _gridReader!.ReadFirstOrDefaultAsync<T>();
    }

    /// <inheritdoc />
    public T? ReadFirstOrDefault<T>()
    {
        EnsureNotDisposed();
        return _gridReader!.ReadFirstOrDefault<T>();
    }

    /// <inheritdoc />
    public Task<T> ReadSingleAsync<T>()
    {
        EnsureNotDisposed();
        return _gridReader!.ReadSingleAsync<T>();
    }

    /// <inheritdoc />
    public T ReadSingle<T>()
    {
        EnsureNotDisposed();
        return _gridReader!.ReadSingle<T>();
    }

    /// <inheritdoc />
    public Task<T?> ReadSingleOrDefaultAsync<T>()
    {
        EnsureNotDisposed();
        return _gridReader!.ReadSingleOrDefaultAsync<T>();
    }

    /// <inheritdoc />
    public T? ReadSingleOrDefault<T>()
    {
        EnsureNotDisposed();
        return _gridReader!.ReadSingleOrDefault<T>();
    }

    private void EnsureNotDisposed()
    {
        if (_gridReader is null)
            throw new ObjectDisposedException(nameof(DatabaseGridReader));
    }
}