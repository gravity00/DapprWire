// ReSharper disable UseRawString
namespace DapprWire.Core.DatabaseSessions;

[Collection(nameof(RequireDatabase))]
public class ExecuteTests(DatabaseFixture fixture, ITestOutputHelper output)
{
    #region Execute

    [Fact]
    public async Task ExecuteAsync_NoParams_ShouldCreateRows()
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
    public void Execute_NoParams_ShouldCreateRows()
    {
        var externalId = Guid.NewGuid();

        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        using var session =  database.Connect();

        var result = session.Execute($@"
insert into TestTable (ExternalId, Name)
values ('{externalId}', 'Test {externalId}')");

        Assert.Equal(1, result);
    }

    [Fact]
    public async Task ExecuteAsync_WithParams_ShouldCreateRows()
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

    [Fact]
    public void Execute_WithParams_ShouldCreateRows()
    {
        var externalId = Guid.NewGuid();

        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        using var session = database.Connect();

        var result = session.Execute(@"
insert into TestTable (ExternalId, Name)
values (@ExternalId, @Name)", new
        {
            ExternalId = externalId,
            Name = $"Test {externalId}"
        });

        Assert.Equal(1, result);
    }

    #endregion

    #region ExecuteScalar

    [Fact]
    public async Task ExecuteScalarAsync_NoParams_ShouldCreateRows()
    {
        var ct = CancellationToken.None;
        var externalId = Guid.NewGuid();

        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        await using var session = await database.ConnectAsync(ct);

        var result = await session.ExecuteScalarAsync<int>($@"
insert into TestTable (ExternalId, Name)
values ('{externalId}', 'Test {externalId}');
select cast(SCOPE_IDENTITY() as int);", ct);

        Assert.True(result > 0, "result > 0");
    }

    [Fact]
    public void ExecuteScalar_NoParams_ShouldCreateRows()
    {
        var externalId = Guid.NewGuid();

        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        using var session = database.Connect();

        var result = session.ExecuteScalar<int>($@"
insert into TestTable (ExternalId, Name)
values ('{externalId}', 'Test {externalId}');
select cast(SCOPE_IDENTITY() as int);");

        Assert.True(result > 0, "result > 0");
    }

    [Fact]
    public async Task ExecuteScalarAsync_WithParams_ShouldCreateRows()
    {
        var ct = CancellationToken.None;
        var externalId = Guid.NewGuid();

        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        await using var session = await database.ConnectAsync(ct);

        var result = await session.ExecuteScalarAsync<int>(@"
insert into TestTable (ExternalId, Name)
values (@ExternalId, @Name);
select cast(SCOPE_IDENTITY() as int);", new
        {
            ExternalId = externalId,
            Name = $"Test {externalId}"
        }, ct);

        Assert.True(result > 0, "result > 0");
    }

    [Fact]
    public void ExecuteScalar_WithParams_ShouldCreateRows()
    {
        var externalId = Guid.NewGuid();

        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        using var session = database.Connect();

        var result = session.ExecuteScalar<int>(@"
insert into TestTable (ExternalId, Name)
values (@ExternalId, @Name);
select cast(SCOPE_IDENTITY() as int);", new
        {
            ExternalId = externalId,
            Name = $"Test {externalId}"
        });

        Assert.True(result > 0, "result > 0");
    }

    #endregion

    #region ExecuteReader

    [Fact]
    public async Task ExecuteReaderAsync_NoParams_ReturnsExpectedResults()
    {
        var ct = CancellationToken.None;

        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        await using var session = await database.ConnectAsync(ct);

        await using var reader = await session.ExecuteReaderAsync(@"with
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

        Assert.NotNull(reader);

        var entries = new List<int>();
        while (await reader.ReadAsync(ct))
            entries.Add(reader.GetInt32(0));

        Assert.Collection(entries,
            e => Assert.Equal(1, e),
            e => Assert.Equal(2, e),
            e => Assert.Equal(3, e),
            e => Assert.Equal(4, e)
        );
    }

    [Fact]
    public void ExecuteReader_NoParams_ReturnsExpectedResults()
    {
        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        using var session = database.Connect();

        using var reader = session.ExecuteReader(@"with
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

        Assert.NotNull(reader);

        var entries = new List<int>();
        while (reader.Read())
            entries.Add(reader.GetInt32(0));

        Assert.Collection(entries,
            e => Assert.Equal(1, e),
            e => Assert.Equal(2, e),
            e => Assert.Equal(3, e),
            e => Assert.Equal(4, e)
        );
    }

    [Fact]
    public async Task ExecuteReaderAsync_WithParams_ReturnsExpectedResults()
    {
        var ct = CancellationToken.None;

        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        await using var session = await database.ConnectAsync(ct);

        await using var reader = await session.ExecuteReaderAsync(@"with
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
            Values = (int[])[2, 4]
        }, ct);

        Assert.NotNull(reader);

        var entries = new List<int>();
        while (await reader.ReadAsync(ct))
            entries.Add(reader.GetInt32(0));

        Assert.Collection(entries,
            e => Assert.Equal(2, e),
            e => Assert.Equal(4, e)
        );
    }

    [Fact]
    public void ExecuteReader_WithParams_ReturnsExpectedResults()
    {
        var database = CoreHelpers.CreateTestDatabase(output, fixture.GetDbConnection);

        using var session = database.Connect();

        using var reader = session.ExecuteReader(@"with
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
            Values = (int[])[2, 4]
        });

        Assert.NotNull(reader);

        var entries = new List<int>();
        while (reader.Read())
            entries.Add(reader.GetInt32(0));

        Assert.Collection(entries,
            e => Assert.Equal(2, e),
            e => Assert.Equal(4, e)
        );
    }

    #endregion
}