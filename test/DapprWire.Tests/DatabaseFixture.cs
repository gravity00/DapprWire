using System.Data.Common;
using Dapper;
using Microsoft.Data.SqlClient;
using Testcontainers.MsSql;

namespace DapprWire;

public sealed class DatabaseFixture : IAsyncLifetime
{
    private MsSqlContainer? _container;

    public async Task InitializeAsync()
    {
        _container = new MsSqlBuilder().Build();

        await _container.StartAsync();
        
#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
        await
#endif
        using var connection = GetDbConnection();
        await connection.OpenAsync();

        await connection.ExecuteAsync(@"
create table TestTable
(
    Id int identity(1,1) primary key,
    ExternalId uniqueidentifier not null unique,
    Name nvarchar(100) not null
)");
    }

    public async Task DisposeAsync()
    {
        if (_container is not null)
            await _container.DisposeAsync();

        _container = null;
    }

    public DbConnection GetDbConnection()
    {
        if (_container is null)
            throw new InvalidOperationException("DatabaseSession container is not initialized.");

        var connectionString = _container.GetConnectionString();
        return new SqlConnection(connectionString);
    }
}