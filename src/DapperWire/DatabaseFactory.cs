using System.Data.Common;

namespace DapperWire;

/// <summary>
/// Represents a factory for creating database connections.
/// </summary>
/// <param name="dbConnectionFactory">The <see cref="DbConnection"/> factory.</param>
public class DatabaseFactory(
    DbConnectionFactory dbConnectionFactory
) : IDatabaseFactory
{
    /// <inheritdoc />
    public virtual async Task<IDatabase> CreateAsync(CancellationToken ct)
    {
        var database = new Database(dbConnectionFactory);

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
}

/// <summary>
/// Represents a strongly-typed factory for creating database connections.
/// </summary>
/// <typeparam name="TName">The database name.</typeparam>
/// <param name="dbConnectionFactory">The strongly-typed <see cref="DbConnection"/> factory.</param>
public class DatabaseFactory<TName>(
    DbConnectionFactory<TName> dbConnectionFactory
) : DatabaseFactory(() => dbConnectionFactory()), IDatabaseFactory<TName>
    where TName : IDatabaseName;