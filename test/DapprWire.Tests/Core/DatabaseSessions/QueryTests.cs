// ReSharper disable UseRawString
namespace DapprWire.Core.DatabaseSessions;

[Collection(nameof(RequireDatabase))]
public class QueryTests(DatabaseFixture fixture, ITestOutputHelper output)
{
    #region Query

    [Fact]
    public async Task QueryAsync_NoParams_ReturnsExpectedResults()
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
    public void Query_NoParams_ReturnsExpectedResults()
    {
        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        using var session = database.Connect();

        var entries = session.Query<int>(@"with
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
    Value is not null");

        Assert.Collection(entries,
            e => Assert.Equal(1, e),
            e => Assert.Equal(2, e),
            e => Assert.Equal(3, e),
            e => Assert.Equal(4, e)
        );
    }

    [Fact]
    public async Task QueryAsync_WithParams_ReturnsExpectedResults()
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
    Value in @Values", new
        {
            Values = (int[]) [2, 4]
        }, ct);

        Assert.Collection(entries,
            e => Assert.Equal(2, e),
            e => Assert.Equal(4, e)
        );
    }

    [Fact]
    public void Query_WithParams_ReturnsExpectedResults()
    {
        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        using var session = database.Connect();

        var entries = session.Query<int>(@"with
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
    Value in @Values", new
        {
            Values = (int[]) [2, 4]
        });

        Assert.Collection(entries,
            e => Assert.Equal(2, e),
            e => Assert.Equal(4, e)
        );
    }

    #endregion

    #region Single

    [Fact]
    public async Task QuerySingleAsync_NoParams_ReturnsExpectedResult()
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
    public void QuerySingle_NoParams_ReturnsExpectedResult()
    {
        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        using var session = database.Connect();

        var value = session.QuerySingle<int>(@"with
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
    Value = 2");

        Assert.Equal(2, value);
    }

    [Fact]
    public async Task QuerySingleAsync_WithParams_ReturnsExpectedResult()
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
    public void QuerySingle_WithParams_ReturnsExpectedResult()
    {
        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        using var session = database.Connect();

        var value = session.QuerySingle<int>(@"with
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
        });

        Assert.Equal(2, value);
    }

    [Fact]
    public async Task QuerySingleAsync_NoMatches_Fails()
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
    public void QuerySingle_NoMatches_Fails()
    {
        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        using var session = database.Connect();

        Assert.Throws<InvalidOperationException>(() =>
        {
            session.QuerySingle<int>(@"with
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
    Value = -1");
        });
    }

    [Fact]
    public async Task QuerySingleAsync_MultipleMatches_Fails()
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

    [Fact]
    public void QuerySingle_MultipleMatches_Fails()
    {
        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        using var session = database.Connect();

        Assert.Throws<InvalidOperationException>(() =>
        {
            session.QuerySingle<int>(@"with
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
    Value > 0");
        });
    }

    #endregion

    #region SingleOrDefault

    [Fact]
    public async Task QuerySingleOrDefaultAsync_NoParams_ReturnsExpectedResult()
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
    public void QuerySingleOrDefault_NoParams_ReturnsExpectedResult()
    {
        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        using var session = database.Connect();

        var value = session.QuerySingleOrDefault<int?>(@"with
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
    Value = 2");

        Assert.NotNull(value);
        Assert.Equal(2, value.Value);
    }

    [Fact]
    public async Task QuerySingleOrDefaultAsync_WithParams_ReturnsExpectedResult()
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
    public void QuerySingleOrDefault_WithParams_ReturnsExpectedResult()
    {
        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        using var session = database.Connect();

        var value = session.QuerySingleOrDefault<int?>(@"with
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
        });

        Assert.NotNull(value);
        Assert.Equal(2, value.Value);
    }

    [Fact]
    public async Task QuerySingleOrDefaultAsync_NoMatches_ReturnsDefault()
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
    public void QuerySingleOrDefault_NoMatches_ReturnsDefault()
    {
        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        using var session = database.Connect();

        var value = session.QuerySingleOrDefault<int?>(@"with
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
    Value = -1");

        Assert.Null(value);
    }

    [Fact]
    public async Task QuerySingleOrDefaultAsync_MultipleMatches_Fails()
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

    [Fact]
    public void QuerySingleOrDefault_MultipleMatches_Fails()
    {
        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        using var session = database.Connect();

        Assert.Throws<InvalidOperationException>(() =>
        {
            session.QuerySingleOrDefault<int?>(@"with
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
    Value > 0");
        });
    }

    #endregion

    #region First

    [Fact]
    public async Task QueryFirstAsync_MultipleMatches_NoParams_ReturnsExpectedResult()
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
    public void QueryFirst_MultipleMatches_NoParams_ReturnsExpectedResult()
    {
        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        using var session = database.Connect();

        var value = session.QueryFirst<int>(@"with
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
    Value >= 2");

        Assert.Equal(2, value);
    }

    [Fact]
    public async Task QueryFirstAsync_MultipleMatches_WithParams_ReturnsExpectedResult()
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
    public void QueryFirst_MultipleMatches_WithParams_ReturnsExpectedResult()
    {
        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        using var session = database.Connect();

        var value = session.QueryFirst<int>(@"with
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
        });

        Assert.Equal(2, value);
    }

    [Fact]
    public async Task QueryFirstAsync_NoMatches_Fails()
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

    [Fact]
    public void QueryFirst_NoMatches_Fails()
    {
        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        using var session = database.Connect();

        Assert.Throws<InvalidOperationException>(() =>
        {
            session.QueryFirst<int>(@"with
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
    Value = -1");
        });
    }

    #endregion

    #region FirstOrDefault

    [Fact]
    public async Task QueryFirstOrDefaultAsync_MultipleMatches_NoParams_ReturnsExpectedResult()
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
    public void QueryFirstOrDefault_MultipleMatches_NoParams_ReturnsExpectedResult()
    {
        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        using var session = database.Connect();

        var value = session.QueryFirstOrDefault<int?>(@"with
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
    Value >= 2");

        Assert.NotNull(value);
        Assert.Equal(2, value);
    }

    [Fact]
    public async Task QueryFirstOrDefaultAsync_MultipleMatches_WithParams_ReturnsExpectedResult()
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
    public void QueryFirstOrDefault_MultipleMatches_WithParams_ReturnsExpectedResult()
    {
        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        using var session = database.Connect();

        var value = session.QueryFirstOrDefault<int?>(@"with
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
        });

        Assert.NotNull(value);
        Assert.Equal(2, value);
    }

    [Fact]
    public async Task QueryFirstOrDefaultAsync_NoMatches_ReturnsDefault()
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

    [Fact]
    public void QueryFirstOrDefault_NoMatches_ReturnsDefault()
    {
        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        using var session = database.Connect();

        var value = session.QueryFirstOrDefault<int?>(@"with
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
    Value = -1");

        Assert.Null(value);
    }

    #endregion

    #region QueryMultiple

    [Fact]
    public async Task QueryMultipleAsync_NoParams_ReturnsExpectedResults()
    {
        var ct = CancellationToken.None;

        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        await using var session = await database.ConnectAsync(ct);

        await using var gridReader = await session.QueryMultipleAsync(@"with
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
    Value is not null;

select -1;", ct);

        var firstResults = await gridReader.ReadAsync<int>();
        Assert.Collection(firstResults,
            e => Assert.Equal(1, e),
            e => Assert.Equal(2, e),
            e => Assert.Equal(3, e),
            e => Assert.Equal(4, e)
        );

        var secondResult = await gridReader.ReadSingleAsync<int>();
        Assert.Equal(-1, secondResult);
    }

    [Fact]
    public void QueryMultiple_NoParams_ReturnsExpectedResults()
    {
        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        using var session = database.Connect();

        using var gridReader = session.QueryMultiple(@"with
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
    Value is not null;

select -1;");

        var firstResults = gridReader.Read<int>();
        Assert.Collection(firstResults,
            e => Assert.Equal(1, e),
            e => Assert.Equal(2, e),
            e => Assert.Equal(3, e),
            e => Assert.Equal(4, e)
        );

        var secondResult = gridReader.ReadSingle<int>();
        Assert.Equal(-1, secondResult);
    }

    [Fact]
    public async Task QueryMultipleAsync_WithParams_ReturnsExpectedResults()
    {
        var ct = CancellationToken.None;

        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        await using var session = await database.ConnectAsync(ct);

        await using var gridReader = await session.QueryMultipleAsync(@"with
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
    Value in @Values;

select -1;", new
        {
            Values = (int[])[2, 4]
        }, ct);

        var firstResults = await gridReader.ReadAsync<int>();
        Assert.Collection(firstResults,
            e => Assert.Equal(2, e),
            e => Assert.Equal(4, e)
        );

        var secondResult = await gridReader.ReadSingleAsync<int>();
        Assert.Equal(-1, secondResult);
    }

    [Fact]
    public void QueryMultiple_WithParams_ReturnsExpectedResults()
    {
        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        using var session = database.Connect();

        using var gridReader = session.QueryMultiple(@"with
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
    Value in @Values;

select -1;", new
        {
            Values = (int[]) [2, 4]
        });

        var firstResults = gridReader.Read<int>();
        Assert.Collection(firstResults,
            e => Assert.Equal(2, e),
            e => Assert.Equal(4, e)
        );

        var secondResult = gridReader.ReadSingle<int>();
        Assert.Equal(-1, secondResult);
    }

    #endregion
}