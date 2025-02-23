using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WellKnowns.Infrastructure.AlexandriaSqldb;

namespace Alexandria.Persistence;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLighthousePersistence(
        this IServiceCollection services,
        HostBuilderContext context
    )
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        // csharpier-ignore
        return services
            .ConfigureDatabase();
    }

    private static IServiceCollection ConfigureDatabase(this IServiceCollection services)
    {
        var sqlConnectionString = Environment.GetEnvironmentVariable(EnvVars.SQLConnectionString);

        ArgumentNullException.ThrowIfNull(sqlConnectionString, nameof(EnvVars.SQLConnectionString));

        services.AddDbContext<AlexandriaDbContext>(x =>
        {
            AlexandriaDbContextFactory.ConfigureDbContextOptionsBuilder(x, sqlConnectionString);
        });

        return services;
    }
}
