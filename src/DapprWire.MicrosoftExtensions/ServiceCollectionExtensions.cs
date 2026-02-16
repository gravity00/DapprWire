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
    /// <param name="services">The service collection.</param>
    extension(IServiceCollection services)
    {
        /// <summary>
        /// Adds the strongly-typed database services to the service collection.
        /// </summary>
        /// <typeparam name="TName">The database name.</typeparam>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="config">An optional callback to configure database options.</param>
        /// <returns>The service collection after changes.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public IServiceCollection AddDatabase<TName>(
            Func<IServiceProvider, DbConnection> connectionFactory,
            Action<DatabaseOptions>? config = null
        ) where TName : IDatabaseName
        {
            if (services is null) throw new ArgumentNullException(nameof(services));
            if (connectionFactory is null) throw new ArgumentNullException(nameof(connectionFactory));

            if (config is not null)
                services.Configure<DatabaseOptions<TName>>(config);

            services.TryAddSingleton<DatabaseLogger, MicrosoftExtensionsDatabaseLogger>();
            services.TryAddSingleton(s =>
            {
                var options = s.GetRequiredService<IOptions<DatabaseOptions<TName>>>().Value;
                if (ReferenceEquals(options.Logger, DatabaseLogger.Null))
                    options.Logger = s.GetRequiredService<DatabaseLogger>();
                return options;
            });
            services.TryAddSingleton<DatabaseOptions>(s => s.GetRequiredService<DatabaseOptions<TName>>());
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
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="config">An optional callback to configure database options.</param>
        /// <returns>The service collection after changes.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public IServiceCollection AddDatabaseAsDefault<TName>(
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
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="config">An optional callback to configure database options.</param>
        /// <returns>The service collection after changes.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public IServiceCollection AddDatabase(
            Func<IServiceProvider, DbConnection> connectionFactory,
            Action<DatabaseOptions>? config = null
        ) => services.AddDatabaseAsDefault<DefaultDatabaseName>(connectionFactory, config);

        private void TryAddSingleton<TService, TImplementation>() where TService : class where TImplementation : class, TService
        {
            if (services.All(s => s.ServiceType != typeof(TService)))
                services.AddSingleton<TService, TImplementation>();
        }

        private void TryAddSingleton<TService, TImplementation>(
            Func<IServiceProvider, TImplementation> implementationFactory
        ) where TService : class where TImplementation : class, TService
        {
            if (services.All(s => s.ServiceType != typeof(TService)))
                services.AddSingleton<TService>(implementationFactory);
        }

        private void TryAddSingleton<TService>(
            Func<IServiceProvider, TService> implementationFactory
        ) where TService : class
        {
            services.TryAddSingleton<TService, TService>(implementationFactory);
        }

        private void TryAddScoped<TService, TImplementation>() where TService : class where TImplementation : class, TService
        {
            if (services.All(s => s.ServiceType != typeof(TService)))
                services.AddScoped<TService, TImplementation>();
        }

        private void TryAddScoped<TService, TImplementation>(
            Func<IServiceProvider, TImplementation> implementationFactory
        ) where TService : class where TImplementation : class, TService
        {
            if (services.All(s => s.ServiceType != typeof(TService)))
                services.AddScoped<TService>(implementationFactory);
        }

        private void TryAddScoped<TService>(
            Func<IServiceProvider, TService> implementationFactory
        ) where TService : class
        {
            services.TryAddScoped<TService, TService>(implementationFactory);
        }

        private void TryAddTransient<TService, TImplementation>(
            Func<IServiceProvider, TImplementation> implementationFactory
        ) where TService : class where TImplementation : class, TService
        {
            if (services.All(s => s.ServiceType != typeof(TService)))
                services.AddTransient<TService>(implementationFactory);
        }

        private void TryAddTransient<TService>(
            Func<IServiceProvider, TService> implementationFactory
        ) where TService : class
        {
            services.TryAddTransient<TService, TService>(implementationFactory);
        }
    }
}