namespace DapperWire;

/// <summary>
/// Represents a database connection.
/// </summary>
public interface IDatabase : IDisposable
#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
    , IAsyncDisposable
#endif
{

}

/// <summary>
/// Represents a strongly-typed database connection.
/// </summary>
/// <typeparam name="TName">The database name.</typeparam>
public interface IDatabase<TName> : IDatabase
    where TName : IDatabaseName;

