# :pushpin: DapprWire - easy Dapper integration with Microsoft DI

DapprWire simplifies the integration of **Dapper** into **Microsoft's Dependency Injection (DI) container**, making database connectivity effortless and efficient. It provides an abstraction layer for **connection and transaction management**, that can be easily injected into your services or repositories, while exposing all Dapper operations so you can use it as your micro-ORM.

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

### Automatic Dependency Injection Registration

If your application uses `Microsoft.Extensions.DependencyInjection`, simply install the `DapprWire.MicrosoftExtensions` package:

```sh
dotnet add package DapprWire.MicrosoftExtensions
```

Then use the extension method `AddDatabase` to register services into the container. You must providing a function that creates the `DbConnection`:

```cs
var connectionString = builder.Configuration.GetConnectionString("ProductsDatabase");
builder.Services.AddDatabase(_ => new SqlConnection(connectionString));
```
