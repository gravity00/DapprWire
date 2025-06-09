using System.Data.Common;
using DapprWire;
using Microsoft.Extensions.Options;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods for <see cref="IServiceCollection"/> instances.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the database services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="connectionFactory">The connection factory.</param>
    /// <param name="config">An optional callback to configure database options.</param>
    /// <returns>The service collection after changes.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        Func<IServiceProvider, DbConnection> connectionFactory,
        Action<DatabaseOptions>? config = null
    )
    {
        if (services is null) throw new ArgumentNullException(nameof(services));
        if (connectionFactory is null) throw new ArgumentNullException(nameof(connectionFactory));

        if (config is not null)
            services.Configure(config);

        services.AddSingleton<DatabaseLogger, MicrosoftExtensionsDatabaseLogger>();
        services.AddSingleton(s =>
        {
            var options = s.GetRequiredService<IOptions<DatabaseOptions>>().Value;
            if (options.Logger == DatabaseLogger.Null) 
                options.Logger = s.GetRequiredService<DatabaseLogger>();
            return options;
        });
        services.AddSingleton<DbConnectionFactory>(s => () => connectionFactory(s));
        services.AddSingleton<IDatabase, Database>();
        services.AddScoped<IDatabaseSession, DatabaseSession>();

        return services;
    }

    /// <summary>
    /// Adds the strongly-typed database services to the service collection.
    /// </summary>
    /// <typeparam name="TName">The database name.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <param name="connectionFactory">The connection factory.</param>
    /// <param name="config">An optional callback to configure database options.</param>
    /// <returns>The service collection after changes.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddDatabase<TName>(
        this IServiceCollection services,
        Func<IServiceProvider, DbConnection> connectionFactory,
        Action<DatabaseOptions>? config = null
    ) where TName : IDatabaseName
    {
        if (services is null) throw new ArgumentNullException(nameof(services));
        if (connectionFactory is null) throw new ArgumentNullException(nameof(connectionFactory));

        if (config is not null)
            services.Configure(config);

        services.AddSingleton<DatabaseLogger, MicrosoftExtensionsDatabaseLogger>();
        services.AddSingleton(s =>
        {
            var options = s.GetRequiredService<IOptions<DatabaseOptions>>().Value;
            if (options.Logger == DatabaseLogger.Null)
                options.Logger = s.GetRequiredService<DatabaseLogger>();
            return options;
        });
        services.AddSingleton<DbConnectionFactory<TName>>(s => () => connectionFactory(s));
        services.AddSingleton<IDatabase<TName>, Database<TName>>();
        services.AddScoped<IDatabaseSession<TName>, DatabaseSession<TName>>();

        return services;
    }

    /// <summary>
    /// Adds the strongly-typed database services to the service collection while also
    /// being considered the default database for resolution purposes.
    /// </summary>
    /// <typeparam name="TName">The database name.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <param name="connectionFactory">The connection factory.</param>
    /// <param name="config">An optional callback to configure database options.</param>
    /// <returns>The service collection after changes.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddDatabaseAsDefault<TName>(
        this IServiceCollection services,
        Func<IServiceProvider, DbConnection> connectionFactory,
        Action<DatabaseOptions>? config = null
    ) where TName : IDatabaseName
    {
        services.AddDatabase<TName>(connectionFactory, config);

        services.AddSingleton<IDatabase>(s => s.GetRequiredService<IDatabase<TName>>());
        services.AddScoped<IDatabaseSession>(s => s.GetRequiredService<IDatabaseSession<TName>>());

        return services;
    }
}