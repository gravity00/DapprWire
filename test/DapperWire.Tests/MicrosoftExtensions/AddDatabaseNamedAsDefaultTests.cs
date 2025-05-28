using Microsoft.Extensions.DependencyInjection;

namespace DapperWire.MicrosoftExtensions;

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
        
        var databaseFactory = host.Services.GetService<IDatabaseFactory<TestDatabaseName>>();
        
        Assert.NotNull(databaseFactory);
        Assert.IsType<MicrosoftExtensionsDatabaseFactory<TestDatabaseName>>(databaseFactory);
    }

    [Fact]
    public void DatabaseNamed_Resolve_Scoped_Succeed()
    {
        using var host = MicrosoftExtensionsHelpers.CreateTestHost(output, builder =>
        {
            builder.Services.AddDatabaseAsDefault<TestDatabaseName>(_ => fixture.GetDbConnection());
        });

        using var scope = host.Services.CreateScope();

        var database = scope.ServiceProvider.GetService<IDatabase<TestDatabaseName>>();
        
        Assert.NotNull(database);
        Assert.IsType<MicrosoftExtensionsDatabase<TestDatabaseName>>(database);
    }

    [Fact]
    public void DatabaseNamed_Resolve_NotScoped_Fail()
    {
        using var host = MicrosoftExtensionsHelpers.CreateTestHost(output, builder =>
        {
            builder.Services.AddDatabaseAsDefault<TestDatabaseName>(_ => fixture.GetDbConnection());
        });

        Assert.Throws<InvalidOperationException>(
            () => host.Services.GetService<IDatabase<TestDatabaseName>>()
        );
    }

    [Fact]
    public void DatabaseFactory_Resolve_Singleton_Succeed()
    {
        using var host = MicrosoftExtensionsHelpers.CreateTestHost(output, builder =>
        {
            builder.Services.AddDatabaseAsDefault<TestDatabaseName>(_ => fixture.GetDbConnection());
        });

        var databaseFactory = host.Services.GetService<IDatabaseFactory>();

        Assert.NotNull(databaseFactory);
        Assert.IsType<MicrosoftExtensionsDatabaseFactory<TestDatabaseName>>(databaseFactory);
    }

    [Fact]
    public void Database_Resolve_Scoped_Succeed()
    {
        using var host = MicrosoftExtensionsHelpers.CreateTestHost(output, builder =>
        {
            builder.Services.AddDatabaseAsDefault<TestDatabaseName>(_ => fixture.GetDbConnection());
        });

        using var scope = host.Services.CreateScope();

        var database = scope.ServiceProvider.GetService<IDatabase>();

        Assert.NotNull(database);
        Assert.IsType<MicrosoftExtensionsDatabase<TestDatabaseName>>(database);
    }
}