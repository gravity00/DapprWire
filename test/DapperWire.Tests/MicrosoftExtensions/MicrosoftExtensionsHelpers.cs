using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DapperWire.MicrosoftExtensions;

public static class MicrosoftExtensionsHelpers
{
    public static IHost CreateTestHost(ITestOutputHelper output, Action<HostApplicationBuilder> config)
    {
        var builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings
        {
            EnvironmentName = Environments.Development
        });

        builder.Logging.SetMinimumLevel(LogLevel.Trace);

        config(builder);

        return builder.Build();
    }
}