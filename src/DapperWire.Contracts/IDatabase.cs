namespace DapperWire;

/// <summary>
/// Represents a database connection.
/// </summary>
public interface IDatabase : IDisposable
#if NETSTANDARD2_1_OR_GREATER || NET8_0_OR_GREATER
    , IAsyncDisposable
#endif
{

}

/// <summary>
/// Represents a strongly-typed database connection.
/// </summary>
public interface IDatabase<TName> : IDatabase
    where TName : IDatabaseName;

