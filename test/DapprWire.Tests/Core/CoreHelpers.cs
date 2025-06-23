using System.Collections.Concurrent;
using System.Data.Common;
using System.Text;
using System.Text.RegularExpressions;

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

    public static Database<TestDatabaseName> CreateTestDatabase(
        ITestOutputHelper output,
        Func<DbConnection> dbConnectionFactory,
        Action<DatabaseOptions>? config = null
    )
    {
        var options = CreateDatabaseOptions(output, config);
        return new Database<TestDatabaseName>(options, () => dbConnectionFactory());
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

    public static void CreateTestTableRow(
        this IDatabaseSession session,
        Guid externalId,
        string name
    )
    {
        var parameters = new
        {
            ExternalId = externalId,
            Name = name
        };
        session.Execute(@"
insert into TestTable (
    ExternalId,
    Name
)
values (
    @ExternalId,
    @Name
)", parameters);
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

    public static TestTableEntity? GetTestTableRowByExternalId(
        this IDatabaseSession session,
        Guid externalId
    )
    {
        var parameters = new
        {
            ExternalId = externalId
        };
        return session.QuerySingleOrDefault<TestTableEntity>(@"
select
    Id,
    ExternalId,
    Name
from TestTable
where
    ExternalId = @ExternalId", parameters);
    }

    private class TestDatabaseLogger(ITestOutputHelper output) : DatabaseLogger
    {
        private static readonly Regex LogParameterRegex = new(@"\{[A-Za-z0-9]+\}", RegexOptions.Compiled);
        private static readonly ConcurrentDictionary<Type, string> LogNameCache = new();

        public override void Log<T>(DatabaseLogLevel level, Exception? exception, string message, params object?[] args)
        {
            try
            {
                if (args.Length > 0)
                {
                    var idx = 0;
                    message = LogParameterRegex.Replace(message, match =>
                    {
                        if (idx < args.Length)
                            return args[idx++]?.ToString() ?? "<null>";
                        return match.Value;
                    });
                }

                var logName = LogNameCache.GetOrAdd(typeof(T), type =>
                {
                    var sb = new StringBuilder();

                    if (!string.IsNullOrWhiteSpace(type.Namespace))
                        sb.Append(type.Namespace).Append('.');

                    if (type.IsGenericType)
                    {
                        sb.Append(type.Name, 0, type.Name.Length - 2);
                        sb.Append('<');
                        sb.Append(string.Join(", ", type.GenericTypeArguments.Select(arg => arg.Name)));
                        sb.Append('>');
                    }
                    else
                    {
                        sb.Append(type.Name);
                    }

                    return sb.ToString();
                });

                output.WriteLine($"{DateTimeOffset.UtcNow:O} [{level}] {logName} | {message}");
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