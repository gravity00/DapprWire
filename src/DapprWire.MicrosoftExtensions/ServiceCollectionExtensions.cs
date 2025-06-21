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

        services.TryAddSingleton<DatabaseLogger, MicrosoftExtensionsDatabaseLogger>();
        services.TryAddSingleton(s =>
        {
            var options = s.GetRequiredService<IOptions<DatabaseOptions>>().Value;
            if (ReferenceEquals(options.Logger, DatabaseLogger.Null))
                options.Logger = s.GetRequiredService<DatabaseLogger>();
            return options;
        });
        services.TryAddTransient<DbConnectionFactory<TName>>(s => () => connectionFactory(s));
        services.TryAddSingleton<IDatabase<TName>, Database<TName>>();
        services.TryAddScoped<IDatabaseSession<TName>, DatabaseSession<TName>>();
        services.TryAddScoped<IDatabaseSqlRunner<TName>>(s => s.GetRequiredService<IDatabaseSession<TName>>());

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

        services.TryAddSingleton<IDatabase>(s => s.GetRequiredService<IDatabase<TName>>());
        services.TryAddScoped<IDatabaseSession>(s => s.GetRequiredService<IDatabaseSession<TName>>());
        services.TryAddScoped<IDatabaseSqlRunner>(s => s.GetRequiredService<IDatabaseSession>());

        return services;
    }

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
    ) => services.AddDatabaseAsDefault<DefaultDatabaseName>(connectionFactory, config);

    private static void TryAddSingleton<TService, TImplementation>(
        this IServiceCollection services
    ) where TService : class where TImplementation : class, TService
    {
        if (services.All(s => s.ServiceType != typeof(TService)))
            services.AddSingleton<TService, TImplementation>();
    }

    private static void TryAddSingleton<TService, TImplementation>(
        this IServiceCollection services,
        Func<IServiceProvider, TImplementation> implementationFactory
    ) where TService : class where TImplementation : class, TService
    {
        if (services.All(s => s.ServiceType != typeof(TService)))
            services.AddSingleton<TService>(implementationFactory);
    }

    private static void TryAddSingleton<TService>(
        this IServiceCollection services,
        Func<IServiceProvider, TService> implementationFactory
    ) where TService : class
    {
        services.TryAddSingleton<TService, TService>(implementationFactory);
    }

    private static void TryAddScoped<TService, TImplementation>(
        this IServiceCollection services
    ) where TService : class where TImplementation : class, TService
    {
        if (services.All(s => s.ServiceType != typeof(TService)))
            services.AddScoped<TService, TImplementation>();
    }

    private static void TryAddScoped<TService, TImplementation>(
        this IServiceCollection services,
        Func<IServiceProvider, TImplementation> implementationFactory
    ) where TService : class where TImplementation : class, TService
    {
        if (services.All(s => s.ServiceType != typeof(TService)))
            services.AddScoped<TService>(implementationFactory);
    }

    private static void TryAddScoped<TService>(
        this IServiceCollection services,
        Func<IServiceProvider, TService> implementationFactory
    ) where TService : class
    {
        services.TryAddScoped<TService, TService>(implementationFactory);
    }

    private static void TryAddTransient<TService, TImplementation>(
        this IServiceCollection services,
        Func<IServiceProvider, TImplementation> implementationFactory
    ) where TService : class where TImplementation : class, TService
    {
        if (services.All(s => s.ServiceType != typeof(TService)))
            services.AddTransient<TService>(implementationFactory);
    }

    private static void TryAddTransient<TService>(
        this IServiceCollection services,
        Func<IServiceProvider, TService> implementationFactory
    ) where TService : class
    {
        services.TryAddTransient<TService, TService>(implementationFactory);
    }
}