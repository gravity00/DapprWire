using System.Data;

namespace DapperWire;

/// <summary>
/// Provides extension methods for <see cref="IDatabase"/> instances.
/// </summary>
public static class DatabaseExtensions
{
    /// <summary>
    /// Starts a new database transaction.
    /// </summary>
    /// <param name="database">The database instance.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task to be awaited for the result.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Task<IDatabaseTransaction> BeginTransactionAsync(
        this IDatabase database,
        CancellationToken ct
    )
    {
        if (database == null) throw new ArgumentNullException(nameof(database));
        return database.BeginTransactionAsync(default, ct);
    }
}