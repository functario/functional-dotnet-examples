using System.Reflection;
using Example.WebApi.Supports.EndpointMapper;
using WellKnowns.Exceptions;
using WellKnowns.Presentation.ExampleWebApi;

namespace Example.WebApi;

internal static class ServiceCollectionsExtensions
{
    internal static IServiceCollection AddWebApi(
        this IServiceCollection services,
        HostBuilderContext _
    )
    {
        return services
            .AddEndpointsApiExplorer()
            .AddEndpoints(Assembly.GetExecutingAssembly())
            .AddOpenApi(x =>
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
