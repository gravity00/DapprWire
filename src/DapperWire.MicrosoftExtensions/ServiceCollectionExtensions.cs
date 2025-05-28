using System.Data.Common;
using DapperWire;

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
        Func<IServiceProvider, Task<DbConnection>> connectionFactory,
        Action<DatabaseOptions>? config = null
    )
    {
        if (services == null) throw new ArgumentNullException(nameof(services));
        if (connectionFactory == null) throw new ArgumentNullException(nameof(connectionFactory));

        if (config is not null)
            services.Configure(config);

        services.AddSingleton<DbConnectionFactory>(s => () => connectionFactory(s));
        services.AddSingleton<IDatabaseFactory, MicrosoftExtensionsDatabaseFactory>();
        services.AddScoped<IDatabase, MicrosoftExtensionsDatabase>();

        return services;
    }

    /// <summary>
    /// Adds the strongly-typed database services to the service collection.
    /// </summary>
    /// <typeparam name="TName"></typeparam>
    /// <param name="services">The service collection.</param>
    /// <param name="connectionFactory">The connection factory.</param>
    /// <param name="config">An optional callback to configure database options.</param>
    /// <param name="registerAsDefault">Should this database be considered the default database?</param>
    /// <returns>The service collection after changes.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddDatabase<TName>(
        this IServiceCollection services,
        Func<IServiceProvider, Task<DbConnection>> connectionFactory,
        Action<DatabaseOptions>? config = null,
        bool registerAsDefault = false
    ) where TName : IDatabaseName
    {
        if (services == null) throw new ArgumentNullException(nameof(services));
        if (connectionFactory == null) throw new ArgumentNullException(nameof(connectionFactory));

        if (config is not null)
            services.Configure(config);

        services.AddSingleton<DbConnectionFactory<TName>>(s => () => connectionFactory(s));
        services.AddSingleton<IDatabaseFactory<TName>, MicrosoftExtensionsDatabaseFactory<TName>>();
        services.AddScoped<IDatabase<TName>, MicrosoftExtensionsDatabase<TName>>();

        if (registerAsDefault)
        {
            services.AddSingleton<IDatabaseFactory>(s => s.GetRequiredService<IDatabaseFactory<TName>>());
            services.AddScoped<IDatabase>(s => s.GetRequiredService<IDatabase<TName>>());
        }

        return services;
    }
}