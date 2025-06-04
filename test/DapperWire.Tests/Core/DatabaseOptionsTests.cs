using System.Data;
using System.Data.Common;

namespace DapperWire.Core;

public class DatabaseOptionsTests
{
    [Fact]
    public void Constructor_DefaultValues()
    {
        var options = new DatabaseOptions();

        Assert.Equal(IsolationLevel.ReadCommitted, options.DefaultIsolationLevel);
        Assert.Null(options.OnConnectionOpen);
        Assert.Null(options.DefaultTimeout);
    }

    [Fact]
    public void Setters_GettersAreSame()
    {
        const IsolationLevel isolationLevel = IsolationLevel.Serializable;
        const int defaultTimeout = 30;
        var onConnectionOpen = new Func<DbConnection, CancellationToken, Task>((_, _) => Task.CompletedTask);

        var options = new DatabaseOptions
        {
            DefaultIsolationLevel = isolationLevel,
            DefaultTimeout = defaultTimeout,
            OnConnectionOpen = onConnectionOpen,
        };

        Assert.Equal(isolationLevel, options.DefaultIsolationLevel);
        
        Assert.NotNull(options.OnConnectionOpen);
        Assert.Same(onConnectionOpen, options.OnConnectionOpen);

        Assert.NotNull(options.DefaultTimeout);
        Assert.Equal(defaultTimeout, options.DefaultTimeout);
    }
}