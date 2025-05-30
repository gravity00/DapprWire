using System.Data.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DapperWire;

/// <summary>
/// Represents a factory for creating database connections.
/// </summary>
/// <param name="logger">The database factory logger.</param>
/// <param name="databaseLogger">The database logger.</param>
/// <param name="options">The database options.</param>
/// <param name="dbConnectionFactory">The <see cref="DbConnection"/> factory.</param>
public class MicrosoftExtensionsDatabase(
    ILogger<Database> logger,
    ILogger<DatabaseSession> databaseLogger,
    IOptions<DatabaseOptions> options,
    DbConnectionFactory dbConnectionFactory
) : Database(options.Value, dbConnectionFactory)
{
    private readonly DbConnectionFactory _dbConnectionFactory = dbConnectionFactory;

    /// <inheritdoc />
    public override async Task<IDatabaseSession> ConnectAsync(CancellationToken ct)
    {
        logger.LogDebug("Creating a new database connection...");

        var database = await base.ConnectAsync(ct);

        logger.LogInformation("A new database connection has been created.");

        return database;
    }

    /// <inheritdoc />
    protected override DatabaseSession CreateDatabase() => new MicrosoftExtensionsDatabaseSession(
        databaseLogger,
        options,
        _dbConnectionFactory
    );
}

/// <summary>
/// Represents a strongly-typed factory for creating database connections.
/// </summary>
/// <typeparam name="TName">The database name.</typeparam>
/// <param name="logger">The database factory logger.</param>
/// <param name="databaseLogger">The database logger.</param>
/// <param name="options">The database options.</param>
/// <param name="dbConnectionFactory">The strongly-typed <see cref="DbConnection"/> factory.</param>
public class MicrosoftExtensionsDatabase<TName>(
    ILogger<Database> logger,
    ILogger<DatabaseSession<TName>> databaseLogger,
    IOptions<DatabaseOptions> options,
    DbConnectionFactory<TName> dbConnectionFactory
) : Database<TName>(options.Value, dbConnectionFactory)
    where TName : IDatabaseName
{
    private readonly DbConnectionFactory<TName> _dbConnectionFactory = dbConnectionFactory;

    /// <inheritdoc />
    public override async Task<IDatabaseSession> ConnectAsync(CancellationToken ct)
    {
        logger.LogDebug("Creating a new database connection...");

        var database = await base.ConnectAsync(ct);

        logger.LogInformation("A new database connection has been created.");

        return database;
    }

    /// <inheritdoc />
    protected override DatabaseSession CreateDatabase() => new MicrosoftExtensionsDatabaseSession<TName>(
        databaseLogger,
        options,
        _dbConnectionFactory
    );
}