using System.Reflection;
using Alexandria.Application;
using Alexandria.Persistence;
using Alexandria.WebApi.Supports.EndpointMapper;
using WellKnowns.Exceptions;
using WellKnowns.Presentation.AlexandriaWebApi;

namespace Alexandria.WebApi;

internal static class ServiceCollectionsExtensions
{
    internal static IServiceCollection AddWebApi(
        this IServiceCollection services,
        HostBuilderContext context
    )
    {
        return services
            .AddLighthousePersistence(context)
            .AddAlexandriaApplication(context)
            .AddEndpointsApiExplorer()
            // AddEndpoints must be called after persistence and application
            .AddEndpoints(Assembly.GetExecutingAssembly())
            .WithOpenApi();
    }

    internal static IServiceCollection WithOpenApi(this IServiceCollection services)
    {
        return services.AddOpenApi(x =>
        {
            var openApiDeaultUrl =
                Environment.GetEnvironmentVariable(EnvVars.OpenApiDefaultUrl)
                ?? throw new EnvironmentNotFoundException(EnvVars.OpenApiDefaultUrl);

            x.AddDocumentTransformer(
                (document, context, cancellationToken) =>
                {
                    // Configure default url to display inside OpenAPI contract
                    document.Servers.Add(
                        new Microsoft.OpenApi.Models.OpenApiServer() { Url = openApiDeaultUrl }
                    );

                    return Task.CompletedTask;
                }
            );
        });
    }
}
