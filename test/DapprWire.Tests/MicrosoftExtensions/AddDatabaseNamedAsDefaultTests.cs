using Microsoft.Extensions.DependencyInjection;

namespace DapprWire.MicrosoftExtensions;

[Collection(nameof(RequireDatabase))]
public class AddDatabaseNamedAsDefaultTests(DatabaseFixture fixture, ITestOutputHelper output)
{
    [Fact]
    public void DatabaseNamed_Resolve_Singleton_Succeed()
    {
        using var host = MicrosoftExtensionsHelpers.CreateTestHost(output, builder =>
        {
            builder.Services.AddDatabaseAsDefault<TestDatabaseName>(_ => fixture.GetDbConnection());
        });
        
        var database = host.Services.GetService<IDatabase<TestDatabaseName>>();
        
        Assert.NotNull(database);
        Assert.IsType<Database<TestDatabaseName>>(database);
    }

    [Fact]
    public void DatabaseSessionNamed_Resolve_Scoped_Succeed()
    {
        using var host = MicrosoftExtensionsHelpers.CreateTestHost(output, builder =>
        {
            builder.Services.AddDatabaseAsDefault<TestDatabaseName>(_ => fixture.GetDbConnection());
        });

        using var scope = host.Services.CreateScope();

        var databaseSession = scope.ServiceProvider.GetService<IDatabaseSession<TestDatabaseName>>();
        
        Assert.NotNull(databaseSession);
        Assert.IsType<DatabaseSession<TestDatabaseName>>(databaseSession);
    }

    [Fact]
    public void DatabaseSessionNamed_Resolve_NotScoped_Fail()
    {
        using var host = MicrosoftExtensionsHelpers.CreateTestHost(output, builder =>
        {
            builder.Services.AddDatabaseAsDefault<TestDatabaseName>(_ => fixture.GetDbConnection());
        });

        Assert.Throws<InvalidOperationException>(
            () => host.Services.GetService<IDatabaseSession<TestDatabaseName>>()
        );
    }

    [Fact]
    public void DatabaseSqlRunnerNamed_Resolve_Scoped_Succeed()
    {
        using var host = MicrosoftExtensionsHelpers.CreateTestHost(output, builder =>
        {
            builder.Services.AddDatabaseAsDefault<TestDatabaseName>(_ => fixture.GetDbConnection());
        });

        using var scope = host.Services.CreateScope();

        var databaseSqlRunner = scope.ServiceProvider.GetService<IDatabaseSqlRunner<TestDatabaseName>>();
        
        Assert.NotNull(databaseSqlRunner);
        Assert.IsType<DatabaseSession<TestDatabaseName>>(databaseSqlRunner);
        Assert.Same(
            scope.ServiceProvider.GetService<IDatabaseSession<TestDatabaseName>>(),
            databaseSqlRunner
        );
    }

    [Fact]
    public void DatabaseSqlRunnerNamed_Resolve_NotScoped_Fail()
    {
        using var host = MicrosoftExtensionsHelpers.CreateTestHost(output, builder =>
        {
            builder.Services.AddDatabaseAsDefault<TestDatabaseName>(_ => fixture.GetDbConnection());
        });

        Assert.Throws<InvalidOperationException>(
            () => host.Services.GetService<IDatabaseSqlRunner<TestDatabaseName>>()
        );
    }

    [Fact]
    public void Database_Resolve_Singleton_Succeed()
    {
        using var host = MicrosoftExtensionsHelpers.CreateTestHost(output, builder =>
        {
            builder.Services.AddDatabaseAsDefault<TestDatabaseName>(_ => fixture.GetDbConnection());
        });

        var database = host.Services.GetService<IDatabase>();

        Assert.NotNull(database);
        Assert.IsType<Database<TestDatabaseName>>(database);
    }

    [Fact]
    public void DatabaseSession_Resolve_Scoped_Succeed()
    {
        using var host = MicrosoftExtensionsHelpers.CreateTestHost(output, builder =>
        {
            builder.Services.AddDatabaseAsDefault<TestDatabaseName>(_ => fixture.GetDbConnection());
        });

        using var scope = host.Services.CreateScope();

        var databaseSession = scope.ServiceProvider.GetService<IDatabaseSession>();

        Assert.NotNull(databaseSession);
        Assert.IsType<DatabaseSession<TestDatabaseName>>(databaseSession);
    }

    [Fact]
    public void DatabaseSqlRunner_Resolve_Scoped_Succeed()
    {
        using var host = MicrosoftExtensionsHelpers.CreateTestHost(output, builder =>
        {
            builder.Services.AddDatabaseAsDefault<TestDatabaseName>(_ => fixture.GetDbConnection());
        });

        using var scope = host.Services.CreateScope();

        var databaseSqlRunner = scope.ServiceProvider.GetService<IDatabaseSqlRunner>();

        Assert.NotNull(databaseSqlRunner);
        Assert.IsType<DatabaseSession<TestDatabaseName>>(databaseSqlRunner);
        Assert.Same(
            scope.ServiceProvider.GetService<IDatabaseSession<TestDatabaseName>>(),
            databaseSqlRunner
        );
        Assert.Same(
            scope.ServiceProvider.GetService<IDatabaseSqlRunner<TestDatabaseName>>(),
            databaseSqlRunner
        );
        Assert.Same(
            scope.ServiceProvider.GetService<IDatabaseSession>(),
            databaseSqlRunner
        );
    }
}