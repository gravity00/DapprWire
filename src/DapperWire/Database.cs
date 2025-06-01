namespace DapperWire;

/// <summary>
/// Represents a factory for creating database connections.
/// </summary>
/// <param name="logger">The database logger.</param>
/// <param name="options">The database options.</param>
/// <param name="dbConnectionFactory">The <see cref="DbConnection"/> factory.</param>
public class Database(
    DatabaseLogger logger,
    DatabaseOptions options,
    DbConnectionFactory dbConnectionFactory
) : IDatabase
{
    /// <inheritdoc />
    public async Task<IDatabaseSession> ConnectAsync(CancellationToken ct)
    {
        logger.Debug<Database>("Starting a new database session...");
        var database = new DatabaseSession(logger, options, dbConnectionFactory);

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

        logger.Info<Database>("Database session started successfully.");

        return database;
    }
}

/// <summary>
/// Represents a strongly-typed factory for creating database connections.
/// </summary>
/// <typeparam name="TName">The database name.</typeparam>
/// <param name="logger">The database logger.</param>
/// <param name="options">The database options.</param>
/// <param name="dbConnectionFactory">The strongly-typed <see cref="DbConnection"/> factory.</param>
public class Database<TName>(
    DatabaseLogger logger,
    DatabaseOptions options,
    DbConnectionFactory<TName> dbConnectionFactory
) : Database(logger, options , () => dbConnectionFactory()), IDatabase<TName>
    where TName : IDatabaseName;