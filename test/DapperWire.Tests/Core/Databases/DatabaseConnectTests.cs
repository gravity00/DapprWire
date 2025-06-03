namespace DapperWire.Core.Databases;

[Collection(nameof(RequireDatabase))]
public class DatabaseConnectTests(DatabaseFixture fixture, ITestOutputHelper output)
{
    [Fact]
    public async Task Connect_Succeed()
    {
        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        await using var session = await database.ConnectAsync(CancellationToken.None);

        Assert.NotNull(session);
    }

    [Fact]
    public async Task Options_OnConnectionOpen_Invoked()
    {
        var onConnectionOpenInvoked = false;
        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection, options =>
        {
            options.OnConnectionOpen = (session, _) =>
            {
                Assert.NotNull(session);

                onConnectionOpenInvoked = true;
                return Task.CompletedTask;
            };
        });

        await using var session = await database.ConnectAsync(CancellationToken.None);

        Assert.True(onConnectionOpenInvoked);
    }
}