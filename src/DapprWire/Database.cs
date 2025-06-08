namespace DapprWire;

/// <summary>
/// Represents a factory for creating database connections.
/// </summary>
/// <param name="logger">The database logger.</param>
/// <param name="options">The database options.</param>
/// <param name="dbConnectionFactory">The <see cref="DbConnection"/> factory.</param>
/// <exception cref="ArgumentNullException"></exception>
public class Database(
    DatabaseLogger logger,
    DatabaseOptions options,
    DbConnectionFactory dbConnectionFactory
) : IDatabase
{
    private readonly DatabaseLogger _logger = logger.NotNull(nameof(logger));
    private readonly DatabaseOptions _options = options.NotNull(nameof(options));
    private readonly DbConnectionFactory _dbConnectionFactory = dbConnectionFactory.NotNull(nameof(dbConnectionFactory));

    /// <inheritdoc />
    public async Task<IDatabaseSession> ConnectAsync(CancellationToken ct)
    {
        _logger.LogDebug<Database>("Starting a new database session...");
        var database = new DatabaseSession(_logger, _options, _dbConnectionFactory);

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

        _logger.LogInfo<Database>("Database session started successfully.");

        return database;
    }
}

/// <summary>
/// Represents a strongly-typed factory for creating database connections.
/// </summary>
/// <typeparam name="TName">The database name.</typeparam>
public class Database<TName> : Database, IDatabase<TName>
    where TName : IDatabaseName
{
    /// <summary>
    /// Creates a new instance.
    /// </summary>
    /// <param name="logger">The database logger.</param>
    /// <param name="options">The database options.</param>
    /// <param name="dbConnectionFactory">The strongly-typed <see cref="DbConnection"/> factory.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public Database(
        DatabaseLogger logger,
        DatabaseOptions options,
        DbConnectionFactory<TName> dbConnectionFactory
    ) : base(
        logger.NotNull(nameof(logger)),
        options.NotNull(nameof(options)),
        () => dbConnectionFactory()
    )
    {
        dbConnectionFactory.NotNull(nameof(dbConnectionFactory));
    }
}