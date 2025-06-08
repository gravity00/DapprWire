using Microsoft.Extensions.DependencyInjection;

namespace DapprWire.MicrosoftExtensions;

[Collection(nameof(RequireDatabase))]
public class AddDatabaseNamedAsDefaultTests(DatabaseFixture fixture, ITestOutputHelper output)
{
    [Fact]
    public void DatabaseFactoryNamed_Resolve_Singleton_Succeed()
    {
        using var host = MicrosoftExtensionsHelpers.CreateTestHost(output, builder =>
        {
            builder.Services.AddDatabaseAsDefault<TestDatabaseName>(_ => fixture.GetDbConnection());
        });
        
        var databaseFactory = host.Services.GetService<IDatabase<TestDatabaseName>>();
        
        Assert.NotNull(databaseFactory);
        Assert.IsType<Database<TestDatabaseName>>(databaseFactory);
    }

    [Fact]
    public void DatabaseNamed_Resolve_Scoped_Succeed()
    {
        using var host = MicrosoftExtensionsHelpers.CreateTestHost(output, builder =>
        {
            builder.Services.AddDatabaseAsDefault<TestDatabaseName>(_ => fixture.GetDbConnection());
        });

        using var scope = host.Services.CreateScope();

        var database = scope.ServiceProvider.GetService<IDatabaseSession<TestDatabaseName>>();
        
        Assert.NotNull(database);
        Assert.IsType<DatabaseSession<TestDatabaseName>>(database);
    }

    [Fact]
    public void DatabaseNamed_Resolve_NotScoped_Fail()
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
    public void DatabaseFactory_Resolve_Singleton_Succeed()
    {
        using var host = MicrosoftExtensionsHelpers.CreateTestHost(output, builder =>
        {
            builder.Services.AddDatabaseAsDefault<TestDatabaseName>(_ => fixture.GetDbConnection());
        });

        var databaseFactory = host.Services.GetService<IDatabase>();

        Assert.NotNull(databaseFactory);
        Assert.IsType<Database<TestDatabaseName>>(databaseFactory);
    }

    [Fact]
    public void Database_Resolve_Scoped_Succeed()
    {
        using var host = MicrosoftExtensionsHelpers.CreateTestHost(output, builder =>
        {
            builder.Services.AddDatabaseAsDefault<TestDatabaseName>(_ => fixture.GetDbConnection());
        });

        using var scope = host.Services.CreateScope();

        var database = scope.ServiceProvider.GetService<IDatabaseSession>();

        Assert.NotNull(database);
        Assert.IsType<DatabaseSession<TestDatabaseName>>(database);
    }
}