using System.Data;

namespace DapprWire.Core;

[Collection(nameof(RequireDatabase))]
public class DatabaseTests(DatabaseFixture fixture, ITestOutputHelper output)
{
    [Fact]
    public void Constructor_NullDatabaseOptions_Fails()
    {
        Assert.Throws<ArgumentNullException>(() => new Database<DefaultDatabaseName>(
            null!,
            fixture.GetDbConnection
        ));
    }

    [Fact]
    public void Constructor_NullDbConnectionFactory_Fails()
    {
        Assert.Throws<ArgumentNullException>(() => new Database<DefaultDatabaseName>(
            CoreHelpers.CreateDatabaseOptions(output),
            null!
        ));
    }

    [Fact]
    public async Task ConnectAsync_Succeed()
    {
        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
        await
#endif
        using var session = await database.ConnectAsync(CancellationToken.None);

        Assert.NotNull(session);
        Assert.IsType<DatabaseSession<TestDatabaseName>>(session);
    }

    [Fact]
    public void Connect_Succeed()
    {
        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        using var session = database.Connect();

        Assert.NotNull(session);
        Assert.IsType<DatabaseSession<TestDatabaseName>>(session);
    }

    [Fact]
    public async Task Options_OnConnectionOpenAsync_Invoked()
    {
        var onConnectionOpenInvoked = false;
        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection, options =>
        {
            options.OnConnectionOpen = (dbConnection, _) =>
            {
                Assert.NotNull(dbConnection);
                Assert.Equal(ConnectionState.Open, dbConnection.State);

                onConnectionOpenInvoked = true;
                return Task.CompletedTask;
            };
        });

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
        await
#endif
        using var session = await database.ConnectAsync(CancellationToken.None);

        Assert.True(onConnectionOpenInvoked);
    }

    [Fact]
    public void Options_OnConnectionOpen_Invoked()
    {
        var onConnectionOpenInvoked = false;
        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection, options =>
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