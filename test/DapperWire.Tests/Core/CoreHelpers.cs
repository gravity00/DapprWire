using System.Data.Common;

namespace DapperWire.Core;

public static class CoreHelpers
{
    public static DatabaseOptions CreateDatabaseOptions(Action<DatabaseOptions>? config = null)
    {
        var options = new DatabaseOptions();
        config?.Invoke(options);
        return options;
    }

    public static DatabaseLogger CreateDatabaseLogger(ITestOutputHelper output) => new TestDatabaseLogger(output);

    public static Database CreateTestDatabase(
        ITestOutputHelper output,
        Func<DbConnection> dbConnectionFactory,
        Action<DatabaseOptions>? config = null
    )
    {
        var options = CreateDatabaseOptions(config);
        var logger = CreateDatabaseLogger(output);
        return new Database(logger, options, () => dbConnectionFactory());
    }

    public static Database CreateTestDatabase<TName>(
        ITestOutputHelper output,
        Func<DbConnection> dbConnectionFactory,
        Action<DatabaseOptions>? config = null
    ) where TName : IDatabaseName
    {
        var options = CreateDatabaseOptions(config);
        var logger = CreateDatabaseLogger(output);
        return new Database<TName>(logger, options, () => dbConnectionFactory());
    }

    private class TestDatabaseLogger(ITestOutputHelper output) : DatabaseLogger
    {
        public override void Log<T>(DatabaseLogLevel level, Exception? exception, string message, params object?[] args)
        {
            try
            {
                output.WriteLine($"{DateTimeOffset.UtcNow:O} [{level}] {typeof(T).FullName} | {message}");
                if (args.Length > 0)
                    output.WriteLine($"  Args: {string.Join("|", args)}");
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