# :pushpin: DapprWire - Dapper + Microsoft DI made easy

**DapprWire** simplifies the integration of **Dapper** into **Microsoft's Dependency Injection (DI) container**, making database connectivity effortless and efficient. It provides an abstraction layer for **connection and transaction management**, that can be easily injected into your services or repositories, while exposing all Dapper operations so you can use it as your micro-ORM the same way you use **Dapper**.

If you're tired of manually wiring `IDbConnection` into Microsoft DI, reminding yourself to pass `IDbTransaction` as an argument, or simply have some logs to help analyzing production problems, **DapprWire** provides a clean and simple solution for that.

## :briefcase: Packages

| Package | NuGet | Downloads |
| ------- | ----- | --------- |
| [DapprWire.Contracts](https://www.nuget.org/packages/DapprWire.Contracts) | [![DapprWire.Contracts](https://img.shields.io/nuget/v/DapprWire.Contracts.svg)](https://www.nuget.org/packages/DapprWire.Contracts) | [![DapprWire.Contracts](https://img.shields.io/nuget/dt/DapprWire.Contracts.svg)](https://www.nuget.org/packages/DapprWire.Contracts) |
| [DapprWire](https://www.nuget.org/packages/DapprWire) | [![DapprWire](https://img.shields.io/nuget/v/DapprWire.svg)](https://www.nuget.org/packages/DapprWire) | [![DapprWire](https://img.shields.io/nuget/dt/DapprWire.svg)](https://www.nuget.org/packages/DapprWire) |
| [DapprWire.MicrosoftExtensions](https://www.nuget.org/packages/DapprWire.MicrosoftExtensions) | [![DapprWire.MicrosoftExtensions](https://img.shields.io/nuget/v/DapprWire.MicrosoftExtensions.svg)](https://www.nuget.org/packages/DapprWire.MicrosoftExtensions) | [![DapprWire.MicrosoftExtensions](https://img.shields.io/nuget/dt/DapprWire.MicrosoftExtensions.svg)](https://www.nuget.org/packages/DapprWire.MicrosoftExtensions) |

Package Purposes:

* **DapprWire.Contracts** - library that only contains contracts like `IDatabase`, `IDatabaseSession`, `IDatabaseTransaction` or `SqlOptions`, usefull if you want to decouple from implementation details; 
* **DapprWire** - library with core implementations like `Database`, `DatabaseSession` or `DatabaseTransaction`;
* **DapprWire.MicrosoftExtensions** - extension library for `Microsoft.Extension.*` packages that helps with container registration or logging SQL statements into `ILogger` instances;

### Compatibility

All packages directly target the following frameworks:

* .NET Framework 4.6.1;
* .NET Standard 2.0;
* .NET Standard 2.1;
* .NET 5.0

This means you should be able to use it almost everywhere that needs to access a SQL database, from an ASP.NET Core 8 application to a Windows Service, command line terminal and even on older ASP.NET Core 2 applications.

## :rocket: Features

By managing database connections and transactions for you, **DapprWire** simplifies how you use **Dapper**. Here are some of the top features that you can read in more detail on the [Wiki documentation](https://github.com/gravity00/DapprWire/wiki):

### :white_check_mark: Automatic Dependency Injection Registration

If your application uses `Microsoft.Extensions.DependencyInjection`, simply install the `DapprWire.MicrosoftExtensions` package:

```sh
dotnet add package DapprWire.MicrosoftExtensions
```

Then use the extension method `AddDatabase` to register the required services into the container - you only need to provide a function that creates the `DbConnection` using any database driver you may prefer:

```cs
var connectionString = builder.Configuration.GetConnectionString("ProductsDatabase");
builder.Services.AddDatabase(_ => new Microsoft.Data.SqlClient.SqlConnection(connectionString));
```

Instead of using `DbConnection` directly, you can now inject instances of `IDatabaseSession`, which offer the same methods as Dapper but behave like a unit of work, with a **scoped lifetime**.
If you need to manage directly the lifetime of a `IDatabaseSession`, you can instead inject the `IDatabase` **singleton** and create as many database sessions as you need.

```cs
[Route("products")]
public class ProductsController(IDatabaseSession databaseSession) : ControllerBase
{
  [HttpGet]
  public async Task<IEnumerable<ProductModel> GetAllAsync(CancellationToken ct)
  {
    return await databaseSession.QueryAsync<ProductModel>(
      "select Id, Code, Name from Products",
      ct
    );
  }
}
```

### :white_check_mark: Multiple Database Support

If your application needs to connect to multiple databases, like a service that synchronizes data between multiple systems, you can easily define multiple `IDatabaseName`, one for each database you need to connect.

```cs
public sealed class ProductsDatabase : IDatabaseName;

public sealed class ClientsDatabase : IDatabaseName;
```

Then use the `AddDatabase<TName>` or, if you want one of the databases to be the fallback when no name is specified, `AddDatabaseAsDefault<TName>` methods to register the required services, while providing a `DbConnection` factory for each:

```cs
var productsDatabaseConnectionString = builder.Configuration.GetConnectionString("ProductsDatabase");
builder.Services.AddDatabaseAsDefault<ProductsDatabase>(
  _ => new Microsoft.Data.SqlClient.SqlConnection(productsDatabaseConnectionString)
);

var clientsDatabaseConnectionString = builder.Configuration.GetConnectionString("ClientsDatabase");
builder.Services.AddDatabase<ClientsDatabase>(
  _ => new Oracle.ManagedDataAccess.Client.OracleConnection(clientsDatabaseConnectionString)
);
```

You can now inject generic instances of `IDatabaseSession<TName>` or `IDatabase<TName>` into your services or repositories and use them like you would with a single database.

```cs
[Route("products")]
public class ProductsController(
  IDatabaseSession<ProductsDatabase> productsDatabaseSession,
  IDatabaseSession<ClientsDatabase> clientsDatabaseSession,
  IDatabaseSession databaseSession // 'ProductsDatabase' was defined as default, so this is the same instance as 'productsDatabaseSession'
) : ControllerBase
{
  // ...
}
```

### :white_check_mark: Database Transactions

When you need to manage database transactions, after starting a new transaction **DapprWire** will automatically pass it to **Dapper** when executing any database operation until it is disposed, preventing transactional problems:

```cs
await using var tx = await databaseSession.BeginTransactionAsync(ct);

var productId = await databaseSession.ExecuteScalarAsync<long>(
  "insert into Products(Code, Name) values (@Code, @Name);select cast(SCOPE_IDENTITY() as bigint);",
  new
  {
    Code = "12345",
    Name = "Product 12345"
  },
  ct
);

await databaseSession.ExecuteAsync(
  "insert into Events(Description) values (@Description)",
  new
  {
    Description = $"Product {productId} was created!"
  },
  ct
);

await tx.CommitAsync(ct);
```

Keep in mind that, when using multiple databases, **DapprWire** only manages the transaction for each session individually so you need so solve distributed transaction problems on your own.

### :white_check_mark: Asynchronously Stream Large Result Sets

When dealing with large result sets, loading everything into memory at once may not be the best approach.
**DapprWire** has built-in support for `IAsyncEnumerable<T>` so you can stream and map results directly from the database without loading everything into memory first:

```cs
var productsStream = databaseSession.QueryStreamed<ProductModel>(
  "select Id, Code, Name from Products",
  ct
);
await foreach (var product in productsStream)
{
  // Process each product as it is received from the database
}
```

### :white_check_mark: Logging and Default Options

**DapprWire** has native support for logging, which means that everytime you open a new database session, transaction or execute an SQL command it will be logged, if enabled.
By default, logging is disabled except when using the `DapprWire.MicrosoftExtensions` package since it will use Microsoft `ILogger` façade so you can manage logging configurations via application settings.

```sh
2025-06-09T14:40:41.3136668+00:00 [Debug] DapprWire.Database | Starting a new database session...
2025-06-09T14:40:41.3137026+00:00 [Debug] DapprWire.DatabaseSession | Creating a new database connection...
2025-06-09T14:40:41.3138825+00:00 [Debug] DapprWire.DatabaseSession | Opening the database connection...
2025-06-09T14:40:41.3139286+00:00 [Info] DapprWire.DatabaseSession | Database connection opened successfully.
2025-06-09T14:40:41.3139347+00:00 [Info] DapprWire.Database | Database session started successfully.
2025-06-09T14:40:41.3140091+00:00 [Debug] DapprWire.DatabaseSession | Starting a new database transaction [IsolationLevel:ReadCommitted]
2025-06-09T14:40:41.3157917+00:00 [Info] DapprWire.DatabaseSession | Database transaction started successfully
2025-06-09T14:40:41.3158548+00:00 [Debug] DapprWire.DatabaseSession | Executing SQL command
[HasParameters:True IsTransactional:True Timeout:<null> Type:<null>]
insert into Products(Code, Name) values (@Code, @Name);select cast(SCOPE_IDENTITY() as bigint);
2025-06-09T14:40:41.3168732+00:00 [Debug] DapprWire.DatabaseSession | Executing SQL command
[HasParameters:True IsTransactional:True Timeout:<null> Type:<null>]
insert into Events(Description) values (@Description)
2025-06-09T14:40:41.3180452+00:00 [Debug] DapprWire.DatabaseTransaction | Committing the database transaction...
2025-06-09T14:40:41.3239587+00:00 [Info] DapprWire.DatabaseTransaction | Database transaction committed successfully.
```

You can assign your own `DatabaseLogger` and other options, like default command timeout, transaction isolation level and even a callback when a new `DbConnection` is open by configuring the available `DatabaseOptions`.
When using `DapprWire.MicrosoftExtensions` package you can either configure just like any other `IOptions` using `Configure<DatabaseOptions>(...)` method or pass a configuration callback to the `AddDatabase` methods:

```cs
var connectionString = builder.Configuration.GetConnectionString("ProductsDatabase");
builder.Services.AddDatabase(_ => new SqlConnection(connectionString), options =>
{
  options.Logger = DatabaseLogger.Null;
  options.DefaultTimeout = 5;
  options.DefaultIsolationLevel = IsolationLevel.ReadCommitted;
});
```

## :hammer: Build

You should be able to easily build this solution as long you have installed both SDKs for .NET 8.0 and .NET Framework 4.8, but more recent versions should also work.

```sh
git clone https://github.com/gravity00/DapprWire.git DapprWire
cd .\DapprWire
dotnet build
```

## :construction: Test

This library is focused on interacting with SQL databases and unit tests currently require a running SQL Server instance.
To make the setup easier, the library [Testcontainers](https://testcontainers.com/) is used so you only need Docker running on your local machine and everything should run as expected.

```sh
git clone https://github.com/gravity00/DapprWire.git DapprWire
cd .\DapprWire
dotnet test
```

Keep in mind the first execution may take some time because images may need to be automatically downloaded first.

If you prefer to use another database, either using Testcontainers or not, simply open the [DatabaseFixture.cs](https://github.com/gravity00/DapprWire/blob/main/test/DapprWire.Tests/DatabaseFixture.cs) file and change to your needs.
