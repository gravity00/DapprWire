namespace DapprWire;

/// <summary>
/// Provides extension methods for <see cref="IDatabaseSession"/> instances.
/// </summary>
public static class DatabaseSessionExtensions
{
    /// <param name="databaseSession">The database instance.</param>
    extension(IDatabaseSession databaseSession)
    {
        /// <summary>
        /// Starts a new database transaction.
        /// </summary>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>A task to be awaited for the transaction to start.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task<IDatabaseTransaction> BeginTransactionAsync(
            CancellationToken ct
        )
        {
            databaseSession.EnsureNotNull();
            return databaseSession.BeginTransactionAsync(default, ct);
        }

        /// <summary>
        /// Starts a new database transaction.
        /// </summary>
        /// <returns>The transaction when started.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public IDatabaseTransaction BeginTransaction()
        {
            databaseSession.EnsureNotNull();
            return databaseSession.BeginTransaction(default);
        }

        private void EnsureNotNull()
        {
            if (databaseSession is null)
                throw new ArgumentNullException(nameof(databaseSession));
        }
    }
}