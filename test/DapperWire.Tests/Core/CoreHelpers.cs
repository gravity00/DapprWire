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
        return new Database(Log, options, dbConnectionFactory);

        void Log(
            Type type,
            DatabaseLogLevel logLevel,
            Exception? exception,
            string message,
            params object?[] args
        )
        {
            try
            {
                output.WriteLine($"[{logLevel}] {type.Name} {message}");
                if (args.Length > 0)
                    output.WriteLine($"  Args: {string.Join("|", args)}");
                if (exception is not null)
                    output.WriteLine(exception.ToString());
            }
            catch (InvalidOperationException)
            {

            }
        }
    }
}