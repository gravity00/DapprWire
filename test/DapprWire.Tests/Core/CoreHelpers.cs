using System.Data.Common;

namespace DapprWire.Core;

public static class CoreHelpers
{
    public static DatabaseOptions CreateDatabaseOptions(
        ITestOutputHelper output,
        Action<DatabaseOptions>? config = null
    )
    {
        var options = new DatabaseOptions
        {
            Logger = new TestDatabaseLogger(output)
        };
        config?.Invoke(options);
        return options;
    }

    public static Database CreateTestDatabase(
        ITestOutputHelper output,
        Func<DbConnection> dbConnectionFactory,
        Action<DatabaseOptions>? config = null
    )
    {
        var options = CreateDatabaseOptions(output, config);
        return new Database(options, () => dbConnectionFactory());
    }

    public static Database CreateTestDatabase<TName>(
        ITestOutputHelper output,
        Func<DbConnection> dbConnectionFactory,
        Action<DatabaseOptions>? config = null
    ) where TName : IDatabaseName
    {
        var options = CreateDatabaseOptions(output, config);
        return new Database<TName>(options, () => dbConnectionFactory());
    }

    public static async Task CreateTestTableRowAsync(
        this IDatabaseSession session,
        Guid externalId,
        string name,
        CancellationToken ct
    )
    {
        var parameters = new
        {
            ExternalId = externalId,
            Name = name
        };
        await session.ExecuteAsync(@"
insert into TestTable (
    ExternalId,
    Name
)
values (
    @ExternalId,
    @Name
)", parameters, ct);
    }

    public static async Task<TestTableEntity?> GetTestTableRowByExternalIdAsync(
        this IDatabaseSession session,
        Guid externalId,
        CancellationToken ct
    )
    {
        var parameters = new
        {
            ExternalId = externalId
        };
        return await session.QuerySingleOrDefaultAsync<TestTableEntity>(@"
select
    Id,
    ExternalId,
    Name
from TestTable
where
    ExternalId = @ExternalId", parameters, ct);
    }

    private class TestDatabaseLogger(ITestOutputHelper output) : DatabaseLogger
    {
        public override void Log<T>(DatabaseLogLevel level, Exception? exception, string message, params object?[] args)
        {
            try
            {
                output.WriteLine($"{ DateTimeOffset.UtcNow:O} [{level}] {typeof(T).FullName} | {message}");
                if (args.Length > 0)
                    output.WriteLine($"  Args: {string.Join("|", args.Select(e => e ?? "<null>"))}");
                if (exception is not null)
                    output.WriteLine(exception.ToString());
            }
            catch (InvalidOperationException)
            {

            }
        }

        public override bool IsEnabled<T>(DatabaseLogLevel level) => true;
    }
}