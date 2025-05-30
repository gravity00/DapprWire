using System.Data.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DapperWire;

/// <summary>
/// Represents a database connection.
/// </summary>
/// <param name="logger">The database logger.</param>
/// <param name="options">The database options.</param>
/// <param name="dbConnectionFactory">The <see cref="DbConnection"/> factory.</param>
public class MicrosoftExtensionsDatabaseSession(
    ILogger<DatabaseSession> logger,
    IOptions<DatabaseOptions> options,
    DbConnectionFactory dbConnectionFactory
) : DatabaseSession(options.Value, dbConnectionFactory)
{
    /// <inheritdoc />
    protected override async Task<GetDbConnectionResult> GetDbConnectionAsync(CancellationToken ct)
    {
        logger.LogDebug("Getting the database connection...");

        var result = await base.GetDbConnectionAsync(ct).ConfigureAwait(false);

        if (result.IsNew)
            logger.LogInformation("A new connection to the database is now open");

        return result;
    }
}

/// <summary>
/// Represents a strongly-typed database connection.
/// </summary>
/// <typeparam name="TName">The database name.</typeparam>
/// <param name="logger">The database logger.</param>
/// <param name="options">The database options.</param>
/// <param name="dbConnectionFactory">The <see cref="DbConnection"/> factory.</param>
public class MicrosoftExtensionsDatabaseSession<TName>(
    ILogger<DatabaseSession<TName>> logger,
    IOptions<DatabaseOptions> options,
    DbConnectionFactory<TName> dbConnectionFactory
) : DatabaseSession<TName>(options.Value, dbConnectionFactory)
    where TName : IDatabaseName
{
    /// <inheritdoc />
    protected override async Task<GetDbConnectionResult> GetDbConnectionAsync(CancellationToken ct)
    {
        logger.LogDebug("Getting the database connection...");

        var result = await base.GetDbConnectionAsync(ct).ConfigureAwait(false);

        if (result.IsNew)
            logger.LogInformation("A new connection to the database is now open");

        return result;
    }
}