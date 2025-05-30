using System.Data.Common;

namespace DapperWire;

/// <summary>
/// Represents a factory for creating database connections.
/// </summary>
/// <param name="options">The database options.</param>
/// <param name="dbConnectionFactory">The <see cref="DbConnection"/> factory.</param>
public class Database(
    DatabaseOptions options,
    DbConnectionFactory dbConnectionFactory
) : IDatabase
{
    /// <inheritdoc />
    public virtual async Task<IDatabaseSession> ConnectAsync(CancellationToken ct)
    {
        var database = CreateDatabase();

        try
        {
            await database.ConnectAsync(ct).ConfigureAwait(false);
        }
        catch
        {
#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
            await database.DisposeAsync().ConfigureAwait(false);
#else
            database.Dispose();
#endif
            throw;
        }

        return database;
    }

    /// <summary>
    /// Creates a new database instance.
    /// </summary>
    /// <returns>The database instance.</returns>
    protected virtual DatabaseSession CreateDatabase() => new(options, dbConnectionFactory);
}

/// <summary>
/// Represents a strongly-typed factory for creating database connections.
/// </summary>
/// <typeparam name="TName">The database name.</typeparam>
/// <param name="options">The database options.</param>
/// <param name="dbConnectionFactory">The strongly-typed <see cref="DbConnection"/> factory.</param>
public class Database<TName>(
    DatabaseOptions options,
    DbConnectionFactory<TName> dbConnectionFactory
) : Database(options , () => dbConnectionFactory()), IDatabase<TName>
    where TName : IDatabaseName;