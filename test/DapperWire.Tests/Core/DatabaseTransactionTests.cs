namespace DapperWire.Core;

[Collection(nameof(RequireDatabase))]
public class DatabaseTransactionTests(DatabaseFixture fixture, ITestOutputHelper output)
{
    [Fact]
    public async Task BeginTransaction_Succeed()
    {
        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        await using var session = await database.ConnectAsync(CancellationToken.None);

        await using var transaction = await session.BeginTransactionAsync(CancellationToken.None);

        Assert.NotNull(transaction);
        Assert.IsType<DatabaseTransaction>(transaction);
    }

    [Fact]
    public async Task BeginTransaction_AlreadyOpen_Fail()
    {
        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);
        
        await using var session = await database.ConnectAsync(CancellationToken.None);
        
        await using var transaction = await session.BeginTransactionAsync(CancellationToken.None);
        
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await session.BeginTransactionAsync(CancellationToken.None);
        });
    }
}