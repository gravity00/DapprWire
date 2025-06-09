// ReSharper disable UseRawString
namespace DapprWire.Core.DatabaseSessions;

[Collection(nameof(RequireDatabase))]
public class ExecuteTests(DatabaseFixture fixture, ITestOutputHelper output)
{
    #region Execute

    [Fact]
    public async Task Execute_NoParams_ShouldCreateRows()
    {
        var ct = CancellationToken.None;
        var externalId = Guid.NewGuid();

        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        await using var session = await database.ConnectAsync(ct);

        var result = await session.ExecuteAsync($@"
insert into TestTable (ExternalId, Name)
values ('{externalId}', 'Test {externalId}')", ct);

        Assert.Equal(1, result);
    }

    [Fact]
    public async Task Execute_WithParams_ShouldCreateRows()
    {
        var ct = CancellationToken.None;
        var externalId = Guid.NewGuid();

        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        await using var session = await database.ConnectAsync(ct);

        var result = await session.ExecuteAsync(@"
insert into TestTable (ExternalId, Name)
values (@ExternalId, @Name)", new
        {
            ExternalId = externalId,
            Name = $"Test {externalId}"
        }, ct);

        Assert.Equal(1, result);
    }

    #endregion
}