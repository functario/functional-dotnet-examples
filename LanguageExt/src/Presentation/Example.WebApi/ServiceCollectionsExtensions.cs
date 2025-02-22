using System.Reflection;
using Example.WebApi.Supports.EndpointMapper;

namespace Example.WebApi;

internal static class ServiceCollectionsExtensions
{
    internal static IServiceCollection AddWebApi(
        this IServiceCollection services,
        HostBuilderContext _
    )
    {
        // csharpier-ignore
        return services
            .AddEndpointsApiExplorer()
            .AddEndpoints(Assembly.GetExecutingAssembly())
            .AddOpenApi(x =>
            {
                x.AddDocumentTransformer(
                    (document, context, cancellationToken) =>
                    {
                        // Configure default url to display inside OpenAPI contract
                        document.Servers.Add(
                            new Microsoft.OpenApi.Models.OpenApiServer()
                            {
                                Url = "https://localhost",
                            }
                        );

                        return Task.CompletedTask;
                    }
                );
            });
    }
}
