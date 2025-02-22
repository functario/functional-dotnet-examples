using Example.SociableTests.DI;
using Microsoft.Extensions.Hosting;

namespace Example.SociableTests;

internal static class Startup
{
    public static IHostBuilder CreateHostBuilder()
    {
        var hostBuilder = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(
                (_, configuration) =>
                {
                    configuration.Sources.Clear();
                }
            )
            .ConfigureServices((context, services) => services.AddSociableTests(context));

        return hostBuilder;
    }
}
