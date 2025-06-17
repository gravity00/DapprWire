namespace DapprWire;

/// <summary>
/// Represents a strongly-typed database.
/// </summary>
/// <typeparam name="TName">The database name.</typeparam>
/// <param name="options">The database options.</param>
/// <param name="dbConnectionFactory">The <see cref="DbConnection"/> factory.</param>
/// <exception cref="ArgumentNullException"></exception>
public class Database<TName>(
    DatabaseOptions options,
    DbConnectionFactory<TName> dbConnectionFactory
) : IDatabase<TName> where TName : IDatabaseName
{
    private readonly DatabaseOptions _options = options.NotNull(nameof(options));
    private readonly DbConnectionFactory<TName> _dbConnectionFactory = dbConnectionFactory.NotNull(nameof(dbConnectionFactory));

    /// <inheritdoc />
    public async Task<IDatabaseSession> ConnectAsync(CancellationToken ct)
    {
        options.Logger.LogDebug<Database<TName>>("Starting a new database session...");
        var database = new DatabaseSession<TName>(_options, _dbConnectionFactory);

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

        options.Logger.LogInfo<Database<TName>>("Database session started successfully.");

        return database;
    }

    /// <inheritdoc />
    public IDatabaseSession Connect()
    {
        options.Logger.LogDebug<Database<TName>>("Starting a new database session...");
        var database = new DatabaseSession<TName>(_options, _dbConnectionFactory);

        try
        {
            database.Connect();
        }
        catch
        {
            database.Dispose();
            throw;
        }

        options.Logger.LogInfo<Database<TName>>("Database session started successfully.");

        return database;
    }
}