// ReSharper disable UseRawString
namespace DapprWire.Core.DatabaseSessions;

[Collection(nameof(RequireDatabase))]
public class QueryTests(DatabaseFixture fixture, ITestOutputHelper output)
{
    #region Query

    [Fact]
    public async Task Query_NoParams_ReturnsExpectedResults()
    {
        var ct = CancellationToken.None;

        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        await using var session = await database.ConnectAsync(ct);

        var entries = await session.QueryAsync<int>(@"with
TestDataCte as (
    select null as Value union all

    select 1 union all
    select 2 union all
    select 3 union all
    select 4
)
select *
from TestDataCte
where
    Value is not null", ct);

        Assert.Collection(entries,
            e => Assert.Equal(1, e),
            e => Assert.Equal(2, e),
            e => Assert.Equal(3, e),
            e => Assert.Equal(4, e)
        );
    }

    [Fact]
    public async Task Query_WithParams_ReturnsExpectedResults()
    {
        var ct = CancellationToken.None;

        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        await using var session = await database.ConnectAsync(ct);

        var entries = await session.QueryAsync<int>(@"with
TestDataCte as (
    select null as Value union all

    select 1 Value union all
    select 2 union all
    select 3 union all
    select 4
)
select *
from TestDataCte
where
    Value in @Values", new
        {
            Values = (int[]) [2, 4]
        }, ct);

        Assert.Collection(entries,
            e => Assert.Equal(2, e),
            e => Assert.Equal(4, e)
        );
    }

    #endregion

    #region Single

    [Fact]
    public async Task QuerySingle_NoParams_ReturnsExpectedResult()
    {
        var ct = CancellationToken.None;

        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        await using var session = await database.ConnectAsync(ct);

        var value = await session.QuerySingleAsync<int>(@"with
TestDataCte as (
    select null as Value union all

    select 1 union all
    select 2 union all
    select 3 union all
    select 4
)
select *
from TestDataCte
where
    Value = 2", ct);

        Assert.Equal(2, value);
    }

    [Fact]
    public async Task QuerySingle_WithParams_ReturnsExpectedResult()
    {
        var ct = CancellationToken.None;

        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        await using var session = await database.ConnectAsync(ct);

        var value = await session.QuerySingleAsync<int>(@"with
TestDataCte as (
    select null as Value union all

    select 1 union all
    select 2 union all
    select 3 union all
    select 4
)
select *
from TestDataCte
where
    Value = @Value", new
        {
            Value = 2
        }, ct);

        Assert.Equal(2, value);
    }

    [Fact]
    public async Task QuerySingle_NoMatches_Fails()
    {
        var ct = CancellationToken.None;

        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        await using var session = await database.ConnectAsync(ct);

        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await session.QuerySingleAsync<int>(@"with
TestDataCte as (
    select null as Value union all

    select 1 union all
    select 2 union all
    select 3 union all
    select 4
)
select *
from TestDataCte
where
    Value = -1", ct);
        });
    }

    [Fact]
    public async Task QuerySingle_MultipleMatches_Fails()
    {
        var ct = CancellationToken.None;

        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        await using var session = await database.ConnectAsync(ct);

        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await session.QuerySingleAsync<int>(@"with
TestDataCte as (
    select null as Value union all

    select 1 union all
    select 2 union all
    select 3 union all
    select 4
)
select *
from TestDataCte
where
    Value > 0", ct);
        });
    }

    #endregion

    #region SingleOrDefault

    [Fact]
    public async Task QuerySingleOrDefault_NoParams_ReturnsExpectedResult()
    {
        var ct = CancellationToken.None;

        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        await using var session = await database.ConnectAsync(ct);

        var value = await session.QuerySingleOrDefaultAsync<int?>(@"with
TestDataCte as (
    select null as Value union all

    select 1 union all
    select 2 union all
    select 3 union all
    select 4
)
select *
from TestDataCte
where
    Value = 2", ct);

        Assert.NotNull(value);
        Assert.Equal(2, value.Value);
    }

    [Fact]
    public async Task QuerySingleOrDefault_WithParams_ReturnsExpectedResult()
    {
        var ct = CancellationToken.None;

        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        await using var session = await database.ConnectAsync(ct);

        var value = await session.QuerySingleOrDefaultAsync<int?>(@"with
TestDataCte as (
    select null as Value union all

    select 1 union all
    select 2 union all
    select 3 union all
    select 4
)
select *
from TestDataCte
where
    Value = @Value", new
        {
            Value = 2
        }, ct);

        Assert.NotNull(value);
        Assert.Equal(2, value.Value);
    }

    [Fact]
    public async Task QuerySingleOrDefault_NoMatches_ReturnsDefault()
    {
        var ct = CancellationToken.None;

        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        await using var session = await database.ConnectAsync(ct);

        var value = await session.QuerySingleOrDefaultAsync<int?>(@"with
TestDataCte as (
    select null as Value union all

    select 1 union all
    select 2 union all
    select 3 union all
    select 4
)
select *
from TestDataCte
where
    Value = -1", ct);

        Assert.Null(value);
    }

    [Fact]
    public async Task QuerySingleOrDefault_MultipleMatches_Fails()
    {
        var ct = CancellationToken.None;

        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        await using var session = await database.ConnectAsync(ct);

        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await session.QuerySingleOrDefaultAsync<int?>(@"with
TestDataCte as (
    select null as Value union all

    select 1 union all
    select 2 union all
    select 3 union all
    select 4
)
select *
from TestDataCte
where
    Value > 0", ct);
        });
    }

    #endregion

    #region First

    [Fact]
    public async Task QueryFirst_MultipleMatches_NoParams_ReturnsExpectedResult()
    {
        var ct = CancellationToken.None;

        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        await using var session = await database.ConnectAsync(ct);

        var value = await session.QueryFirstAsync<int>(@"with
TestDataCte as (
    select null as Value union all

    select 1 union all
    select 2 union all
    select 3 union all
    select 4
)
select *
from TestDataCte
where
    Value >= 2", ct);

        Assert.Equal(2, value);
    }

    [Fact]
    public async Task QueryFirst_MultipleMatches_WithParams_ReturnsExpectedResult()
    {
        var ct = CancellationToken.None;

        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        await using var session = await database.ConnectAsync(ct);

        var value = await session.QueryFirstAsync<int>(@"with
TestDataCte as (
    select null as Value union all

    select 1 union all
    select 2 union all
    select 3 union all
    select 4
)
select *
from TestDataCte
where
    Value >= @Value", new
        {
            Value = 2
        }, ct);

        Assert.Equal(2, value);
    }

    [Fact]
    public async Task QueryFirst_NoMatches_Fails()
    {
        var ct = CancellationToken.None;

        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        await using var session = await database.ConnectAsync(ct);

        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await session.QueryFirstAsync<int>(@"with
TestDataCte as (
    select null as Value union all

    select 1 union all
    select 2 union all
    select 3 union all
    select 4
)
select *
from TestDataCte
where
    Value = -1", ct);
        });
    }

    #endregion

    #region FirstOrDefault

    [Fact]
    public async Task QueryFirstOrDefault_MultipleMatches_NoParams_ReturnsExpectedResult()
    {
        var ct = CancellationToken.None;

        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        await using var session = await database.ConnectAsync(ct);

        var value = await session.QueryFirstOrDefaultAsync<int?>(@"with
TestDataCte as (
    select null as Value union all

    select 1 union all
    select 2 union all
    select 3 union all
    select 4
)
select *
from TestDataCte
where
    Value >= 2", ct);

        Assert.NotNull(value);
        Assert.Equal(2, value);
    }

    [Fact]
    public async Task QueryFirstOrDefault_MultipleMatches_WithParams_ReturnsExpectedResult()
    {
        var ct = CancellationToken.None;

        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        await using var session = await database.ConnectAsync(ct);

        var value = await session.QueryFirstOrDefaultAsync<int?>(@"with
TestDataCte as (
    select null as Value union all

    select 1 union all
    select 2 union all
    select 3 union all
    select 4
)
select *
from TestDataCte
where
    Value >= @Value", new
        {
            Value = 2
        }, ct);

        Assert.NotNull(value);
        Assert.Equal(2, value);
    }

    [Fact]
    public async Task QueryFirstOrDefault_NoMatches_ReturnsDefault()
    {
        var ct = CancellationToken.None;

        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        await using var session = await database.ConnectAsync(ct);

        var value = await session.QueryFirstOrDefaultAsync<int?>(@"with
TestDataCte as (
    select null as Value union all

    select 1 union all
    select 2 union all
    select 3 union all
    select 4
)
select *
from TestDataCte
where
    Value = -1", ct);

        Assert.Null(value);
    }

    #endregion
}