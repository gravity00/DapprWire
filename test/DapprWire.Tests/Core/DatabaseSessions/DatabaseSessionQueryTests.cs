namespace DapprWire.Core.DatabaseSessions;

[Collection(nameof(RequireDatabase))]
public class DatabaseSessionQueryTests(DatabaseFixture fixture, ITestOutputHelper output)
{
    [Fact]
    public async Task QueryAsync_ReturnsExpectedResults()
    {
        var ct = CancellationToken.None;

        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        await using var session = await database.ConnectAsync(ct);

        var entries = AsReadOnlyCollection(await session.QueryAsync<int>(@"
select 1 union all 
select 2 union all
select 3", ct));

        Assert.NotNull(entries);
        Assert.NotEmpty(entries);
        Assert.Equal(3, entries.Count);
        Assert.Collection(entries,
            e => Assert.Equal(1, e),
            e => Assert.Equal(2, e),
            e => Assert.Equal(3, e)
        );
    }

    private static IReadOnlyCollection<T>? AsReadOnlyCollection<T>(
        IEnumerable<T>? items
    ) => items?.ToArray();
}