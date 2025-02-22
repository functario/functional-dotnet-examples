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

                    //Environment.SetEnvironmentVariable("LH_ENVIRONMENT", "qa");

                    //Environment.SetEnvironmentVariable(
                    //    "LH_AZURE_SQL_CONNECTION_STRING",
                    //    "Server=localhost,52425;User ID=sa;Password=Hilo1234!;TrustServerCertificate=true;Database=master"
                    //);

                    //Environment.SetEnvironmentVariable("LH_SKIP_CACHING", "true");
                }
            )
            .ConfigureServices((context, services) => services.AddSociable(context));

        return hostBuilder;
    }
}
