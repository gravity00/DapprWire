using Microsoft.Extensions.DependencyInjection;

namespace DapperWire.MicrosoftExtensions;

[Collection(nameof(RequireDatabase))]
public class AddDatabaseTests(DatabaseFixture fixture, ITestOutputHelper output)
{
    [Fact]
    public void DatabaseFactory_Resolve_Singleton_Succeed()
    {
        using var host = MicrosoftExtensionsHelpers.CreateTestHost(output, builder =>
        {
            builder.Services.AddDatabase(_ => fixture.GetDbConnection());
        });
        
        var databaseFactory = host.Services.GetService<IDatabase>();
        
        Assert.NotNull(databaseFactory);
        Assert.IsType<MicrosoftExtensionsDatabase>(databaseFactory);
    }

    [Fact]
    public void Database_Resolve_Scoped_Succeed()
    {
        using var host = MicrosoftExtensionsHelpers.CreateTestHost(output, builder =>
        {
            builder.Services.AddDatabase(_ => fixture.GetDbConnection());
        });

        using var scope = host.Services.CreateScope();

        var database = scope.ServiceProvider.GetService<IDatabaseSession>();
        
        Assert.NotNull(database);
        Assert.IsType<MicrosoftExtensionsDatabaseSession>(database);
    }

    [Fact]
    public void Database_Resolve_NotScoped_Fail()
    {
        using var host = MicrosoftExtensionsHelpers.CreateTestHost(output, builder =>
        {
            builder.Services.AddDatabase(_ => fixture.GetDbConnection());
        });

        Assert.Throws<InvalidOperationException>(
            () => host.Services.GetService<IDatabaseSession>()
        );
    }
}