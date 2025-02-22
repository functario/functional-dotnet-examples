using Example.WebApi.Supports.EndpointMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Example.SociableTests.DI;

internal static class EndpointRegister
{
    internal static IServiceCollection RegisterWebApiEndpointsInWebApplication(
        this IServiceCollection webApplicationBuilderServices
    )
    {
        // Ensure to register Endpoint as scoped
        var minimalApiEndpoints = webApplicationBuilderServices
            .Where(s => typeof(IEndpoint).IsAssignableFrom(s.ServiceType))
            .ToList();

        foreach (var minimalApiEndpoint in minimalApiEndpoints)
        {
            webApplicationBuilderServices.Remove(minimalApiEndpoint);
            webApplicationBuilderServices.AddScoped(minimalApiEndpoint.ServiceType);
        }

        return webApplicationBuilderServices;
    }
}
