namespace DapprWire.Core;

public class DatabaseLoggerTests
{
    [Theory]
    [InlineData(DatabaseLogLevel.Undefined)]
    [InlineData(DatabaseLogLevel.Debug)]
    [InlineData(DatabaseLogLevel.Info)]
    [InlineData(DatabaseLogLevel.Warn)]
    [InlineData(DatabaseLogLevel.Error)]
    public void DefaultBehavior_IsEnabled_ReturnsFalse(DatabaseLogLevel level)
    {
        var logger = new DatabaseLogger();

        var isEnabled = logger.IsEnabled<DatabaseLoggerTests>(level);
        Assert.False(isEnabled);
    }

    [Theory]
    [InlineData(DatabaseLogLevel.Undefined)]
    [InlineData(DatabaseLogLevel.Debug)]
    [InlineData(DatabaseLogLevel.Info)]
    [InlineData(DatabaseLogLevel.Warn)]
    [InlineData(DatabaseLogLevel.Error)]
    public void DefaultBehavior_Log_NothingHappens(DatabaseLogLevel level)
    {
        var logger = new DatabaseLogger();

        logger.Log<DatabaseLoggerTests>(level, null, "This is a message");
    }


    [Fact]
    public void NullLogger_NotNull()
    {
        var logger = DatabaseLogger.Null;
        Assert.NotNull(logger);
    }
}