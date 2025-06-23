namespace DapprWire.Core;

[Collection(nameof(RequireDatabase))]
public class DatabaseTransactionTests(DatabaseFixture fixture, ITestOutputHelper output)
{
    [Fact]
    public async Task BeginTransactionAsync_Succeed()
    {
        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
        await
#endif
        using var session = await database.ConnectAsync(CancellationToken.None);

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
        await
#endif
        using var transaction = await session.BeginTransactionAsync(CancellationToken.None);

        Assert.NotNull(transaction);
        Assert.IsType<DatabaseTransaction>(transaction);
    }

    [Fact]
    public void BeginTransaction_Succeed()
    {
        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        using var session = database.Connect();

        using var transaction = session.BeginTransaction();

        Assert.NotNull(transaction);
        Assert.IsType<DatabaseTransaction>(transaction);
    }

    [Fact]
    public async Task BeginTransactionAsync_AlreadyOpen_Fail()
    {
        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
        await
#endif
        using var session = await database.ConnectAsync(CancellationToken.None);

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
        await
#endif
        using var transaction = await session.BeginTransactionAsync(CancellationToken.None);

        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await session.BeginTransactionAsync(CancellationToken.None);
        });
    }

    [Fact]
    public void BeginTransaction_AlreadyOpen_Fail()
    {
        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        using var session = database.Connect();

        using var transaction = session.BeginTransaction();

        Assert.Throws<InvalidOperationException>(() =>
        {
            session.BeginTransaction();
        });
    }

    [Fact]
    public async Task CommitTransactionAsync_InsertIsKept()
    {
        var ct = CancellationToken.None;
        var externalId = Guid.NewGuid();

        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
        await
#endif
        using (var session = await database.ConnectAsync(ct))
        {
#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
        await
#endif
            using var transaction = await session.BeginTransactionAsync(ct);

            await session.CreateTestTableRowAsync(externalId, $"Test '{externalId}'", ct);

            await transaction.CommitAsync(ct);
        }

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
        await
#endif
        using (var session = await database.ConnectAsync(ct))
        {
            var testTableRow = await session.GetTestTableRowByExternalIdAsync(externalId, ct);

            Assert.NotNull(testTableRow);
            Assert.Equal(externalId, testTableRow.ExternalId);
        }
    }

    [Fact]
    public void CommitTransaction_InsertIsKept()
    {
        var externalId = Guid.NewGuid();

        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        using (var session = database.Connect())
        {
            using var transaction = session.BeginTransaction();

            session.CreateTestTableRow(externalId, $"Test '{externalId}'");

            transaction.Commit();
        }

        using (var session = database.Connect())
        {
            var testTableRow = session.GetTestTableRowByExternalId(externalId);

            Assert.NotNull(testTableRow);
            Assert.Equal(externalId, testTableRow.ExternalId);
        }
    }

    [Fact]
    public async Task RollbackTransactionAsync_InsertIsReverted()
    {
        var ct = CancellationToken.None;
        var externalId = Guid.NewGuid();

        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
        await
#endif
        using (var session = await database.ConnectAsync(ct))
        {
#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
        await
#endif
            using var transaction = await session.BeginTransactionAsync(ct);
            
            await session.CreateTestTableRowAsync(externalId, $"Test '{externalId}'", ct);

            await transaction.RollbackAsync(ct);
        }

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
        await
#endif
        using (var session = await database.ConnectAsync(ct))
        {
            var testTableRow = await session.GetTestTableRowByExternalIdAsync(externalId, ct);
            
            Assert.Null(testTableRow);
        }
    }

    [Fact]
    public void RollbackTransaction_InsertIsReverted()
    {
        var externalId = Guid.NewGuid();

        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        using (var session = database.Connect())
        {
            using var transaction = session.BeginTransaction();

            session.CreateTestTableRow(externalId, $"Test '{externalId}'");

            transaction.Rollback();
        }

        using (var session = database.Connect())
        {
            var testTableRow = session.GetTestTableRowByExternalId(externalId);
            
            Assert.Null(testTableRow);
        }
    }

    [Fact]
    public async Task DisposeTransactionAsync_NoCommit_InsertIsReverted()
    {
        var ct = CancellationToken.None;
        var externalId = Guid.NewGuid();

        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
        await
#endif
        using (var session = await database.ConnectAsync(ct))
        {
#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
        await
#endif
            using var transaction = await session.BeginTransactionAsync(ct);
            
            await session.CreateTestTableRowAsync(externalId, $"Test '{externalId}'", ct);
        }

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
        await
#endif
        using (var session = await database.ConnectAsync(ct))
        {
            var testTableRow = await session.GetTestTableRowByExternalIdAsync(externalId, ct);
            
            Assert.Null(testTableRow);
        }
    }

    [Fact]
    public void DisposeTransaction_NoCommit_InsertIsReverted()
    {
        var externalId = Guid.NewGuid();

        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        using (var session = database.Connect())
        {
            using var transaction = session.BeginTransaction();
            
            session.CreateTestTableRow(externalId, $"Test '{externalId}'");
        }

        using (var session = database.Connect())
        {
            var testTableRow = session.GetTestTableRowByExternalId(externalId);
            
            Assert.Null(testTableRow);
        }
    }
}