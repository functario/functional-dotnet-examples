using Example.WebApi.Supports.EndpointMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Example.SociableTests.DI;

internal static class ServiceCollectionExtensions
{
    public static void AddSociable(this IServiceCollection services, HostBuilderContext context)
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
        //.RegisterAppServicesAsInterfaceTypes(
        //    app,
        //    webApplicationBuilder,
        //    typeof(IApplicationService)
        //);
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

    ///// <summary>
    ///// Register as scoped, all concrete types (self)
    ///// implementing a generic interface from the WebApplication (the app under test)
    ///// into the test project DI container.
    ///// </summary>
    ///// <param name="services">The ServiceCollection of the test project.</param>
    ///// <param name="webApplication">The WebApplication.</param>
    ///// <param name="webApplicationBuilder">The WebApplicationBuilder.</param>
    ///// <param name="genericInterfaceType">The type of the interface</param>
    ///// <returns>The ServiceCollection of the test project</returns>
    ///// (ex: ServiceA : IGenericInterface; then ServiceA is registered as ServiceA and
    ///// ServiceB : IGenericInterface; then ServiceB is registered as ServiceB.)
    //private static IServiceCollection RegisterAppServicesAsConcreteTypes(
    //    this IServiceCollection services,
    //    WebApplication webApplication,
    //    WebApplicationBuilder webApplicationBuilder,
    //    Type genericInterfaceType
    //)
    //{
    //    var serviceDescriptors = webApplicationBuilder
    //        .Services.Where(s => genericInterfaceType.IsAssignableFrom(s.ServiceType))
    //        .ToList();

    //    foreach (var serviceDescriptor in serviceDescriptors)
    //    {
    //        services.AddScoped(
    //            serviceDescriptor.ServiceType,
    //            x =>
    //            {
    //                // Force scope registration otherwise the same instance is provided
    //                // even if it is registered as scoped in WebApplicaton.
    //                using var scope = webApplication.Services.CreateScope();
    //                return scope.ServiceProvider.GetRequiredService(serviceDescriptor.ServiceType!);
    //            }
    //        );
    //    }

    //    return services;
    //}

    ///// <summary>
    ///// Register as scoped, all types as their inferfaces implementing
    ///// a parent interface from the WebApplication (the app under test)
    ///// into the test project DI container.
    ///// (ex: ServiceA : IServiceA; IServiceA : IParentInterface, then ServiceA is registered as IServiceA).
    ///// </summary>
    ///// <param name="services">The ServiceCollection of the test project.</param>
    ///// <param name="webApplication">The WebApplication.</param>
    ///// <param name="webApplicationBuilder">The WebApplicationBuilder.</param>
    ///// <param name="parentInterfaceType">The type of the interface</param>
    ///// <returns>The ServiceCollection of the test project</returns>
    //private static IServiceCollection RegisterAppServicesAsInterfaceTypes(
    //    this IServiceCollection services,
    //    WebApplication webApplication,
    //    WebApplicationBuilder webApplicationBuilder,
    //    Type parentInterfaceType
    //)
    //{
    //    var serviceDescriptors = webApplicationBuilder
    //        .Services.Where(s => parentInterfaceType.IsAssignableFrom(s.ServiceType))
    //        .ToList();

    //    foreach (var serviceDescriptor in serviceDescriptors)
    //    {
    //        services.AddScoped(
    //            serviceDescriptor.ServiceType,
    //            x =>
    //            {
    //                // Force scope registration
    //                using var scope = webApplication.Services.CreateScope();
    //                return scope.ServiceProvider.GetRequiredService(serviceDescriptor.ServiceType);
    //            }
    //        );
    //    }

    //    return services;
    //}

    private static WebApplicationBuilder ConfigureWebApplicationBuilder(
        this IServiceCollection _, //services
        WebApplicationBuilder webApplicationBuilder
    )
    {
        //var serviceProvider = services.BuildServiceProvider();
        //var serviceDescriptors = services.Where(s =>
        //    typeof(IRepository).IsAssignableFrom(s.ServiceType)
        //);

        // csharpier-ignore
        webApplicationBuilder.Services
            .RegisterWebApiEndpointsInWebApplication();
        //.RegisterTestRepositoriesInWebApplication(serviceProvider, serviceDescriptors);

        return webApplicationBuilder;
    }

    private static IServiceCollection RegisterWebApiEndpointsInWebApplication(
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
