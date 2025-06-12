namespace DapprWire;

/// <summary>
/// Represents a reader for a grid of results from a database query.
/// </summary>
public interface IDatabaseGridReader : IDisposable
#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
    , IAsyncDisposable
#endif
{
    /// <summary>
    /// Read the next grid of results.
    /// </summary>
    /// <typeparam name="T">The type to read.</typeparam>
    /// <returns>A task to be awaited for the result.</returns>
    Task<IEnumerable<T>> ReadAsync<T>();

    /// <summary>
    /// Read the next grid of results, returning a single item.
    /// </summary>
    /// <typeparam name="T">The type to read.</typeparam>
    /// <returns>A task to be awaited for the result.</returns>
    Task<T> ReadFirstAsync<T>();

    /// <summary>
    /// Read the next grid of results, returning a single item or null if no items are found.
    /// </summary>
    /// <typeparam name="T">The type to read.</typeparam>
    /// <returns>A task to be awaited for the result.</returns>
    Task<T?> ReadFirstOrDefaultAsync<T>();

    /// <summary>
    /// Read the next grid of results, returning a single item.
    /// </summary>
    /// <typeparam name="T">The type to read.</typeparam>
    /// <returns>A task to be awaited for the result.</returns>
    Task<T> ReadSingleAsync<T>();

    /// <summary>
    /// Read the next grid of results, returning a single item or null if no items are found.
    /// </summary>
    /// <typeparam name="T">The type to read.</typeparam>
    /// <returns>A task to be awaited for the result.</returns>
    Task<T?> ReadSingleOrDefaultAsync<T>();
}