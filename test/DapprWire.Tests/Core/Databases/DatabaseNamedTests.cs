using System.Data;

namespace DapprWire.Core.Databases;

[Collection(nameof(RequireDatabase))]
public class DatabaseNamedTests(DatabaseFixture fixture, ITestOutputHelper output)
{
    [Fact]
    public void Constructor_NullDatabaseOptions_Fails()
    {
        Assert.Throws<ArgumentNullException>(() => new Database<TestDatabaseName>(
            null!,
            fixture.GetDbConnection
        ));
    }

    [Fact]
    public void Constructor_NullDbConnectionFactory_Fails()
    {
        Assert.Throws<ArgumentNullException>(() => new Database<TestDatabaseName>(
            CoreHelpers.CreateDatabaseOptions(output),
            null!
        ));
    }

    [Fact]
    public async Task ConnectAsync_Succeed()
    {
        var database = CoreHelpers.CreateTestDatabase<TestDatabaseName>(output, fixture.GetDbConnection);

        await using var session = await database.ConnectAsync(CancellationToken.None);

        Assert.NotNull(session);
        Assert.IsType<DatabaseSession>(session);
    }

    [Fact]
    public void Connect_Succeed()
    {
        var database = CoreHelpers.CreateTestDatabase<TestDatabaseName>(output, fixture.GetDbConnection);

        using var session = database.Connect();

        Assert.NotNull(session);
        Assert.IsType<DatabaseSession>(session);
    }

    [Fact]
    public async Task Options_OnConnectionOpenAsync_Invoked()
    {
        var onConnectionOpenInvoked = false;
        var database = CoreHelpers.CreateTestDatabase<TestDatabaseName>(output, fixture.GetDbConnection, options =>
        {
            options.OnConnectionOpen = (dbConnection, _) =>
            {
                Assert.NotNull(dbConnection);
                Assert.Equal(ConnectionState.Open, dbConnection.State);

                onConnectionOpenInvoked = true;
                return Task.CompletedTask;
            };
        });

        await using var session = await database.ConnectAsync(CancellationToken.None);

        Assert.True(onConnectionOpenInvoked);
    }

    [Fact]
    public void Options_OnConnectionOpen_Invoked()
    {
        var onConnectionOpenInvoked = false;
        var database = CoreHelpers.CreateTestDatabase<TestDatabaseName>(output, fixture.GetDbConnection, options =>
        {
            options.OnConnectionOpen = (dbConnection, _) =>
            {
                Assert.NotNull(dbConnection);
                Assert.Equal(ConnectionState.Open, dbConnection.State);

                onConnectionOpenInvoked = true;
                return Task.CompletedTask;
            };
        });

        using var session = database.Connect();

        Assert.True(onConnectionOpenInvoked);
    }
}