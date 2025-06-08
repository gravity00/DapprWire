using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DapprWire.MicrosoftExtensions;

public static class MicrosoftExtensionsHelpers
{
    public static IHost CreateTestHost(ITestOutputHelper output, Action<HostApplicationBuilder> config)
    {
        var builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings
        {
            EnvironmentName = Environments.Development
        });

        builder.Logging.AddProvider(new TestOutputHelperLoggerProvider(output));
        builder.Logging.SetMinimumLevel(LogLevel.Trace);

        config(builder);

        return builder.Build();
    }

    private class TestOutputHelperLoggerProvider(
        ITestOutputHelper output
    ) : ILoggerProvider
    {
        public void Dispose() { }

        public ILogger CreateLogger(string categoryName) => new TestOutputHelperLogger(output, categoryName);
    }

    private class TestOutputHelperLogger(
        ITestOutputHelper output,
        string categoryName
    ) : ILogger
    {
        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter
        )
        {
            var message = formatter(state, exception);
            if (string.IsNullOrEmpty(message) && exception is null)
                return;

            try
            {
                output.WriteLine($"[{logLevel}] {categoryName} {message}");
                if (exception is not null)
                    output.WriteLine(exception.ToString());
            }
            catch (InvalidOperationException)
            {

            }
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public IDisposable BeginScope<TState>(TState state)
            where TState : notnull => TestOutputHelperLoggerScope.Instance;
    }

    private class TestOutputHelperLoggerScope : IDisposable
    {
        public static readonly TestOutputHelperLoggerScope Instance = new();

        public void Dispose() { }
    }
}