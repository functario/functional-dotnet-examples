using Alexandria.Application.AuthorUseCases.AddAuthor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Alexandria.Application;

public static class ServiceCollectionsExtensions
{
    public static IServiceCollection AddAlexandriaApplication(
        this IServiceCollection services,
        HostBuilderContext _
    )
    {
        return services.WithApplicationServices();
    }

    internal static IServiceCollection WithApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAddAuthorService, AddAuthorService>();

        return services;
    }
}
