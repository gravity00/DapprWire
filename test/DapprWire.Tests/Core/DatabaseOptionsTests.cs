using System.Data;
using System.Data.Common;

namespace DapprWire.Core;

public class DatabaseOptionsTests
{
    [Fact]
    public void Constructor_DefaultValues()
    {
        var options = new DatabaseOptions();

        Assert.Same(DatabaseLogger.Null, options.Logger);
        Assert.Null(options.OnConnectionOpen);
        Assert.Null(options.DefaultTimeout);
        Assert.Equal(IsolationLevel.ReadCommitted, options.DefaultIsolationLevel);
    }

    [Fact]
    public void Setters_GettersAreSame()
    {
        var logger = new DatabaseLogger();
        var onConnectionOpen = new Func<DbConnection, CancellationToken, Task>((_, _) => Task.CompletedTask);
        const IsolationLevel isolationLevel = IsolationLevel.Serializable;
        const int defaultTimeout = 30;

        var options = new DatabaseOptions
        {
            Logger = logger,
            OnConnectionOpen = onConnectionOpen,
            DefaultIsolationLevel = isolationLevel,
            DefaultTimeout = defaultTimeout,
        };

        Assert.NotNull(options.Logger);
        Assert.Same(logger, options.Logger);

        Assert.NotNull(options.OnConnectionOpen);
        Assert.Same(onConnectionOpen, options.OnConnectionOpen);

        Assert.NotNull(options.DefaultTimeout);
        Assert.Equal(defaultTimeout, options.DefaultTimeout);

        Assert.Equal(isolationLevel, options.DefaultIsolationLevel);
    }

    [Fact]
    public void Logger_SetNull_ThrowsArgumentNullException()
    {
        var options = new DatabaseOptions();

        Assert.Throws<ArgumentNullException>(() => options.Logger = null!);
    }
}