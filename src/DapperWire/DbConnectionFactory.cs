using System.Data.Common;

namespace DapperWire;

/// <summary>
/// Delegate for creating <see cref="DbConnection"/> instances.
/// </summary>
/// <returns>The database connection.</returns>
public delegate DbConnection DbConnectionFactory();

/// <summary>
/// Delegate for creating strongly-typed <see cref="DbConnection"/> instances.
/// </summary>
/// <typeparam name="TName">The database name.</typeparam>
/// <returns>The database connection.</returns>
public delegate DbConnection DbConnectionFactory<TName>()
    where TName : IDatabaseName;