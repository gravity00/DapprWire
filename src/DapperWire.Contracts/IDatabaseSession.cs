using System.Data;

namespace DapperWire;

/// <summary>
/// Represents a database connection.
/// </summary>
public interface IDatabaseSession : IDisposable
#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
    , IAsyncDisposable
#endif
{
    /// <summary>
    /// Starts a new database transaction.
    /// </summary>
    /// <param name="isolationLevel">The transaction isolation level.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the result.</returns>
    Task<IDatabaseTransaction> BeginTransactionAsync(
        IsolationLevel isolationLevel,
        CancellationToken ct
    );
}

/// <summary>
/// Represents a strongly-typed database connection.
/// </summary>
/// <typeparam name="TName">The database name.</typeparam>
public interface IDatabaseSession<TName> : IDatabaseSession
    where TName : IDatabaseName;