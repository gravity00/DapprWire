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
    /// <returns>A task to be awaited for the collection of items.</returns>
    Task<IReadOnlyCollection<T>> ReadAsync<T>();

    /// <summary>
    /// Read the next grid of results.
    /// </summary>
    /// <typeparam name="T">The type to read.</typeparam>
    /// <returns>The collection of items.</returns>
    IReadOnlyCollection<T> Read<T>();

    /// <summary>
    /// Read the next grid of results, returning a single item.
    /// </summary>
    /// <typeparam name="T">The type to read.</typeparam>
    /// <returns>A task to be awaited for the query result.</returns>
    Task<T> ReadFirstAsync<T>();

    /// <summary>
    /// Read the next grid of results, returning a single item.
    /// </summary>
    /// <typeparam name="T">The type to read.</typeparam>
    /// <returns>The query result.</returns>
    T ReadFirst<T>();

    /// <summary>
    /// Read the next grid of results, returning a single item or null if no items are found.
    /// </summary>
    /// <typeparam name="T">The type to read.</typeparam>
    /// <returns>A task to be awaited for the query result.</returns>
    Task<T?> ReadFirstOrDefaultAsync<T>();

    /// <summary>
    /// Read the next grid of results, returning a single item or null if no items are found.
    /// </summary>
    /// <typeparam name="T">The type to read.</typeparam>
    /// <returns>The query result.</returns>
    T? ReadFirstOrDefault<T>();

    /// <summary>
    /// Read the next grid of results, returning a single item.
    /// </summary>
    /// <typeparam name="T">The type to read.</typeparam>
    /// <returns>A task to be awaited for the query result.</returns>
    Task<T> ReadSingleAsync<T>();

    /// <summary>
    /// Read the next grid of results, returning a single item.
    /// </summary>
    /// <typeparam name="T">The type to read.</typeparam>
    /// <returns>The query result.</returns>
    T ReadSingle<T>();

    /// <summary>
    /// Read the next grid of results, returning a single item or null if no items are found.
    /// </summary>
    /// <typeparam name="T">The type to read.</typeparam>
    /// <returns>A task to be awaited for the query result.</returns>
    Task<T?> ReadSingleOrDefaultAsync<T>();

    /// <summary>
    /// Read the next grid of results, returning a single item or null if no items are found.
    /// </summary>
    /// <typeparam name="T">The type to read.</typeparam>
    /// <returns>The query result.</returns>
    T? ReadSingleOrDefault<T>();
}