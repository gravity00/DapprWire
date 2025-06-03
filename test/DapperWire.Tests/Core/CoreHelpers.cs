using System.Data;

namespace DapperWire.Core;

public static class CoreHelpers
{
    public static Database CreateTestDatabase(
        ITestOutputHelper output,
        DbConnectionFactory dbConnectionFactory,
        Action<DatabaseOptions>? config = null
    )
    {
        var options = new DatabaseOptions
        {
            DefaultIsolationLevel = IsolationLevel.ReadCommitted,
            DefaultTimeout = 5
        };
        config?.Invoke(options);
        return new Database(new TestDatabaseLogger(output), options, dbConnectionFactory);
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