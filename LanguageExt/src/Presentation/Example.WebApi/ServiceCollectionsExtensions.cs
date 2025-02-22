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
            .AddEndpoints(Assembly.GetExecutingAssembly());
    }
}
