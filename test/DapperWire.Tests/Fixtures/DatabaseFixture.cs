using System.Data.Common;
using Microsoft.Data.SqlClient;
using Testcontainers.MsSql;

namespace DapperWire.Fixtures;

public sealed class DatabaseFixture : IAsyncLifetime
{
    private MsSqlContainer? _container;

    public async Task InitializeAsync()
    {
        _container = new MsSqlBuilder().Build();

        await _container.StartAsync();
    }

    public async Task DisposeAsync()
    {
        if (_container is not null)
            await _container.DisposeAsync();
    }

    public DbConnection GetDbConnection()
    {
        if (_container is null)
            throw new InvalidOperationException("Database container is not initialized.");

        var connectionString = _container.GetConnectionString();
        return new SqlConnection(connectionString);
    }
}