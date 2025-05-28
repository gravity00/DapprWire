using System.Data.Common;

namespace DapperWire;

/// <summary>
/// Delegate for creating <see cref="DbConnection"/> instances.
/// </summary>
/// <returns>The database connection.</returns>
public delegate Task<DbConnection> DbConnectionFactory();

/// <summary>
/// Delegate for creating strongly-typed <see cref="DbConnection"/> instances.
/// </summary>
/// <typeparam name="TName">The database name.</typeparam>
/// <returns>The database connection.</returns>
public delegate Task<DbConnection> DbConnectionFactory<TName>()
    where TName : IDatabaseName;