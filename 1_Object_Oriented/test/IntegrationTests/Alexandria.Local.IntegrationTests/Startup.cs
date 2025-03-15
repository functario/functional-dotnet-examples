using Microsoft.Extensions.Hosting;

namespace Alexandria.Local.IntegrationTests;

public static class Startup
{
    public static IHostBuilder CreateHostBuilder()
    {
        return Host.CreateDefaultBuilder()
            .ConfigureServices(
                (context, service) =>
                {
                    service.AddAspire(context);
                }
            );
    }
}
