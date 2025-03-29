using Alexandria.Application.Abstractions.Repositories;
using Alexandria.Persistence.Audits;
using Alexandria.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using WellKnowns.Infrastructure.SQL;
using WellKnowns.Presentation.AlexandriaWebApi;

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
            .WithUnitOfWork()
            .WithRepositories()
            .WithInterceptors()
            .WithTimeProvider()
            .ConfigureDatabase()
            .AddSingleton(TimeProvider.System);
    }

    internal static IServiceCollection WithTimeProvider(this IServiceCollection services)
    {
        services.TryAddSingleton<TimeProvider>(x => TimeProvider.System);
        return services;
    }

    internal static IServiceCollection WithInterceptors(this IServiceCollection services)
    {
        services.AddScoped<
            BaseSaveChangesInterceptor<IAuditable>,
            OnAuditableSaveChangesInterceptor
        >();
        return services;
    }

    internal static IServiceCollection WithUnitOfWork(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    internal static IServiceCollection WithRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAuthorRepository, AuthorRepository>();
        services.AddScoped<IBookRepository, BookRepository>();

        return services;
    }

    private static IServiceCollection ConfigureDatabase(this IServiceCollection services)
    {
        if (BuildType.IsOpenApiGeneratorBuild())
        {
            return services;
        }

        services.AddDbContext<AlexandriaDbContext>(
            (IServiceProvider service, DbContextOptionsBuilder x) =>
            {
                var sqlConnectionString = Environment.GetEnvironmentVariable(
                    SqldbEnvVars.SQLConnectionString
                );

                ArgumentException.ThrowIfNullOrWhiteSpace(
                    sqlConnectionString,
                    nameof(SqldbEnvVars.SQLConnectionString)
                );

                var interceptors = service.GetRequiredService<
                    BaseSaveChangesInterceptor<IAuditable>
                >();

                AlexandriaDbContextFactory.ConfigureDbContextOptionsBuilder(
                    x,
                    sqlConnectionString,
                    interceptors
                );
            }
        );

        return services;
    }
}
