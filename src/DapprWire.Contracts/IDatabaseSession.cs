using System.Data;

namespace DapprWire;

/// <summary>
/// Represents a database session.
/// </summary>
public interface IDatabaseSession : IDatabaseSqlRunner, IDisposable
#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
    , IAsyncDisposable
#endif
{
    /// <summary>
    /// Starts a new database transaction.
    /// </summary>
    /// <param name="isolationLevel">The transaction isolation level.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the transaction to start.</returns>
    Task<IDatabaseTransaction> BeginTransactionAsync(
        IsolationLevel isolationLevel,
        CancellationToken ct
    );

    /// <summary>
    /// Starts a new database transaction.
    /// </summary>
    /// <param name="isolationLevel">The transaction isolation level.</param>
    /// <returns>The transaction when started.</returns>
    IDatabaseTransaction BeginTransaction(
        IsolationLevel isolationLevel
    );
}

/// <summary>
/// Represents a strongly-typed database session.
/// </summary>
/// <typeparam name="TName">The database name.</typeparam>
public interface IDatabaseSession<TName> : IDatabaseSqlRunner<TName>, IDatabaseSession
    where TName : IDatabaseName;