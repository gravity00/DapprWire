namespace DapprWire;

/// <summary>
/// Represents a database transaction.
/// </summary>
public interface IDatabaseTransaction : IDisposable
#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
    , IAsyncDisposable
#endif
{
    /// <summary>
    /// Commits the transaction.
    /// </summary>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the result.</returns>
    Task CommitAsync(CancellationToken ct);

    /// <summary>
    /// Rolls back the transaction.
    /// </summary>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the result.</returns>
    Task RollbackAsync(CancellationToken ct);
}