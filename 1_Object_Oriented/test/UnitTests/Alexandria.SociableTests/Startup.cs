using dotenv.net;
using Microsoft.Extensions.Hosting;

namespace Alexandria.SociableTests;

internal static class Startup
{
    public static IHostBuilder CreateHostBuilder()
    {
        DotEnv.Fluent().WithTrimValues().WithOverwriteExistingVars().Load();
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
