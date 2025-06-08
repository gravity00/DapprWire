namespace DapprWire.Core;

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

    [Fact]
    public async Task CommitTransaction_InsertIsKept()
    {
        var ct = CancellationToken.None;
        var externalId = Guid.NewGuid();

        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        await using (var session = await database.ConnectAsync(ct))
        {
            await using var transaction = await session.BeginTransactionAsync(ct);

            await session.CreateTestTableRowAsync(externalId, $"Test '{externalId}'", ct);

            await transaction.CommitAsync(ct);
        }

        await using (var session = await database.ConnectAsync(ct))
        {
            var testTableRow = await session.GetTestTableRowByExternalIdAsync(externalId, ct);

            Assert.NotNull(testTableRow);
            Assert.Equal(externalId, testTableRow.ExternalId);
        }
    }

    [Fact]
    public async Task RollbackTransaction_InsertIsReverted()
    {
        var ct = CancellationToken.None;
        var externalId = Guid.NewGuid();

        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        await using (var session = await database.ConnectAsync(ct))
        {
            await using var transaction = await session.BeginTransactionAsync(ct);
            
            await session.CreateTestTableRowAsync(externalId, $"Test '{externalId}'", ct);

            await transaction.RollbackAsync(ct);
        }

        await using (var session = await database.ConnectAsync(ct))
        {
            var testTableRow = await session.GetTestTableRowByExternalIdAsync(externalId, ct);
            
            Assert.Null(testTableRow);
        }
    }

    [Fact]
    public async Task DisposeTransaction_NoCommit_InsertIsReverted()
    {
        var ct = CancellationToken.None;
        var externalId = Guid.NewGuid();

        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        await using (var session = await database.ConnectAsync(ct))
        {
            await using var transaction = await session.BeginTransactionAsync(ct);
            
            await session.CreateTestTableRowAsync(externalId, $"Test '{externalId}'", ct);
        }

        await using (var session = await database.ConnectAsync(ct))
        {
            var testTableRow = await session.GetTestTableRowByExternalIdAsync(externalId, ct);
            
            Assert.Null(testTableRow);
        }
    }
}