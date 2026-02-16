using Microsoft.Extensions.DependencyInjection;

namespace DapprWire.MicrosoftExtensions;

[Collection(nameof(RequireDatabase))]
public class AddDatabaseTests(DatabaseFixture fixture, ITestOutputHelper output)
{
    [Fact]
    public void DatabaseOptions_Resolve_Singleton_Succeed()
    {
        using var host = MicrosoftExtensionsHelpers.CreateTestHost(output, builder =>
        {
            builder.Services.AddDatabase(_ => fixture.GetDbConnection());
        });

        var databaseOptionsNamed = host.Services.GetService<DatabaseOptions<DefaultDatabaseName>>();

        Assert.NotNull(databaseOptionsNamed);
        Assert.IsType<MicrosoftExtensionsDatabaseLogger>(databaseOptionsNamed.Logger);

        var databaseOptions = host.Services.GetService<DatabaseOptions>();
        
        Assert.NotNull(databaseOptions);
        Assert.Same(databaseOptionsNamed, databaseOptions);
    }

    [Fact]
    public void Database_Resolve_Singleton_Succeed()
    {
        using var host = MicrosoftExtensionsHelpers.CreateTestHost(output, builder =>
        {
            builder.Services.AddDatabase(_ => fixture.GetDbConnection());
        });
        
        var database = host.Services.GetService<IDatabase>();
        
        Assert.NotNull(database);
        Assert.IsType<Database<DefaultDatabaseName>>(database);
    }

    [Fact]
    public void DatabaseSession_Resolve_Scoped_Succeed()
    {
        using var host = MicrosoftExtensionsHelpers.CreateTestHost(output, builder =>
        {
            builder.Services.AddDatabase(_ => fixture.GetDbConnection());
        });

        using var scope = host.Services.CreateScope();

        var databaseSession = scope.ServiceProvider.GetService<IDatabaseSession>();
        
        Assert.NotNull(databaseSession);
        Assert.IsType<DatabaseSession<DefaultDatabaseName>>(databaseSession);
    }

    [Fact]
    public void DatabaseSession_Resolve_NotScoped_Fail()
    {
        using var host = MicrosoftExtensionsHelpers.CreateTestHost(output, builder =>
        {
            builder.Services.AddDatabase(_ => fixture.GetDbConnection());
        });

        Assert.Throws<InvalidOperationException>(
            () => host.Services.GetService<IDatabaseSession>()
        );
    }

    [Fact]
    public void DatabaseSqlRunner_Resolve_Scoped_Succeed()
    {
        using var host = MicrosoftExtensionsHelpers.CreateTestHost(output, builder =>
        {
            builder.Services.AddDatabase(_ => fixture.GetDbConnection());
        });

        using var scope = host.Services.CreateScope();

        var databaseSqlRunner = scope.ServiceProvider.GetService<IDatabaseSqlRunner>();
        
        Assert.NotNull(databaseSqlRunner);
        Assert.IsType<DatabaseSession<DefaultDatabaseName>>(databaseSqlRunner);
        Assert.Same(
            scope.ServiceProvider.GetService<IDatabaseSession>(),
            databaseSqlRunner
        );
    }

    [Fact]
    public void DatabaseSqlRunner_Resolve_NotScoped_Fail()
    {
        using var host = MicrosoftExtensionsHelpers.CreateTestHost(output, builder =>
        {
            builder.Services.AddDatabase(_ => fixture.GetDbConnection());
        });

        Assert.Throws<InvalidOperationException>(
            () => host.Services.GetService<IDatabaseSqlRunner>()
        );
    }
}