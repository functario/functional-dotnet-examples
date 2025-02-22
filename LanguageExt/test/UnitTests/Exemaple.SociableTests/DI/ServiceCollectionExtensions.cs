using Example.WebApi.Supports.EndpointMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Example.SociableTests.DI;

internal static class ServiceCollectionExtensions
{
    public static void AddSociableTests(
        this IServiceCollection services,
        HostBuilderContext context
    )
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        var webApplicationBuilder = WebApiStartup.CreateWebHostBuilder([]);
        services
        //.RegisterTestRepositoriesInTestAssembly()
        .ConfigureWebApplicationBuilder(webApplicationBuilder);

        // Need to build the app to resolve dependencies
        var app = WebApiStartup.BuildWebAppAsync(webApplicationBuilder).GetAwaiter().GetResult();

        services
            .AddSingleton(app)
            .RegisterAppServicesAsSelfInterfaces(app, webApplicationBuilder, typeof(IEndpoint));
    }

    /// <summary>
    /// Register as scoped, all concrete types (self)
    /// implementing a generic interface from the WebApplication (the app under test)
    /// into the test project DI container
    /// as interface matching the pattern "INameOfClass".
    /// </summary>
    /// <param name="services">The ServiceCollection of the test project.</param>
    /// <param name="webApplication">The WebApplication.</param>
    /// <param name="webApplicationBuilder">The WebApplicationBuilder.</param>
    /// <param name="genericInterfaceType">The type of the interface</param>
    /// <returns>The ServiceCollection of the test project</returns>
    /// (ex: ServiceA : IGenericInterface; then ServiceA is registered as ServiceA and
    /// ServiceB : IGenericInterface; then ServiceB is registered as ServiceB.)
    private static IServiceCollection RegisterAppServicesAsSelfInterfaces(
        this IServiceCollection services,
        WebApplication webApplication,
        WebApplicationBuilder webApplicationBuilder,
        Type genericInterfaceType
    )
    {
        var serviceDescriptors = webApplicationBuilder
            .Services.Where(s => genericInterfaceType.IsAssignableFrom(s.ServiceType))
            .ToList();

        foreach (var serviceDescriptor in serviceDescriptors)
        {
            var match = $"I{serviceDescriptor.ServiceType.Name}";
            var endpointInterface =
                serviceDescriptor
                    .ServiceType.GetInterfaces()
                    .Where(x => x.Name == match)
                    .SingleOrDefault()
                ?? throw new InvalidOperationException(
                    $"{serviceDescriptor.ServiceType.Name} does not implement maching interface {match}"
                );

            services.AddScoped(
                endpointInterface,
                x =>
                {
                    // Force scope registration otherwise the same instance is provided
                    // even if it is registered as scoped in WebApplicaton.
                    using var scope = webApplication.Services.CreateScope();
                    return scope.ServiceProvider.GetRequiredService(serviceDescriptor.ServiceType!);
                }
            );
        }

        return services;
    }

    private static WebApplicationBuilder ConfigureWebApplicationBuilder(
        this IServiceCollection _, //services
        WebApplicationBuilder webApplicationBuilder
    )
    {
        // csharpier-ignore
        webApplicationBuilder.Services
            .RegisterWebApiEndpointsInWebApplication();

        return webApplicationBuilder;
    }
}
