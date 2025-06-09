# :pushpin: DapprWire - easy Dapper integration with Microsoft DI

**DapprWire** simplifies the integration of **Dapper** into **Microsoft's Dependency Injection (DI) container**, making database connectivity effortless and efficient. It provides an abstraction layer for **connection and transaction management**, that can be easily injected into your services or repositories, while exposing all Dapper operations so you can use it as your micro-ORM the same way you use **Dapper**.

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

This means you should be able to use it almost everywhere that needs to access a SQL database, from an ASP.NET Core 8 API to a Windows Service, command line terminal and even on older ASP.NET Core 2 applications.

## :rocket: Features

By managing database connections and transactions for you, **DapprWire** simplifies how you use **Dapper**. Here are some of the top features:

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
