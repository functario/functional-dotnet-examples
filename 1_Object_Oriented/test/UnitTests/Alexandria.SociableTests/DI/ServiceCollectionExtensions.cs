using Alexandria.Application.Abstractions.Repositories;
using Alexandria.WebApi.Supports.EndpointMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSubstitute;

namespace Alexandria.SociableTests.DI;

internal static class ServiceCollectionExtensions
{
    public static void AddSociableTests(
        this IServiceCollection services,
        HostBuilderContext context
    )
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        services.AddWebAppResolver();
    }

    private static IServiceCollection AddWebAppResolver(this IServiceCollection services)
    {
        services.AddScoped(x =>
        {
            // Each WebAppServicesFactory has a built WebApi.
            // This ensure to isolate each tests.
            var webApplicationBuilder = WebApiStartup.CreateWebHostBuilder([]);
            webApplicationBuilder
                .Services.RegisterRepositoryInWebApplicationAsMock()
                .RegisterConcreteClassesAsInterfacesInWebApplication(
                    typeof(IEndpoint),
                    (implementedInterface, concreteImplementation) =>
                        implementedInterface.Name.Contains(
                            concreteImplementation.Name,
                            StringComparison.OrdinalIgnoreCase
                        )
                );

            var app = WebApiStartup
                .BuildWebAppAsync(webApplicationBuilder)
                .GetAwaiter()
                .GetResult();

            return new WebAppServicesFactory(app);
        });

        return services;
    }

    /// <summary>
    /// Replace IRepository concrete registrations by mock.
    /// </summary>
    /// <param name="webApplicationServiceCollection">The WebApplication service collection.</param>
    /// <returns></returns>
    private static IServiceCollection RegisterRepositoryInWebApplicationAsMock(
        this IServiceCollection webApplicationServiceCollection
    )
    {
        var iRepositories = webApplicationServiceCollection
            .Where(s => typeof(IRepository).IsAssignableFrom(s.ServiceType))
            .ToList();

        foreach (var iRepository in iRepositories)
        {
            webApplicationServiceCollection.Remove(iRepository);
            webApplicationServiceCollection.AddScoped(
                iRepository.ServiceType,
                x => Substitute.For([iRepository.ServiceType], null)
            );
        }

        return webApplicationServiceCollection;
    }

    /// <summary>
    /// Register inside AppService container matching implemented interfaces as scoped, from concret types
    /// implementing a generic interface.
    /// </summary>
    /// <param name="webApplicationServiceCollection">The ServiceCollection of the WebApp.</param>
    /// <param name="genericInterfaceType">The type of the one interface implemented by the concret types.
    /// It does not have to be the matching implemented interface resolved by the predicate.</param>
    /// <param name="predicate">The predicate comparing implemented interface with the implementation type.</param>
    /// <returns>The ServiceCollection of WebApp.</returns>
    /// <example>ServiceA : IServiceA. And IServiceA : IGenericInterface;
    /// then ServiceA is registered as IServiceA given a predicate
    /// (interface, class) => interface.Name.Contains(class.Name). </example>
    private static IServiceCollection RegisterConcreteClassesAsInterfacesInWebApplication(
        this IServiceCollection webApplicationServiceCollection,
        Type genericInterfaceType,
        Func<Type, Type, bool> predicate
    )
    {
        var serviceDescriptors = webApplicationServiceCollection
            .Where(s => genericInterfaceType.IsAssignableFrom(s.ServiceType))
            .ToList();

        foreach (var serviceDescriptor in serviceDescriptors)
        {
            var concreteType = serviceDescriptor.ImplementationType;
            var matchingInterface =
                (
                    concreteType
                        ?.GetInterfaces()
                        .Where(x => predicate(x, concreteType))
                        .FirstOrDefault()
                )
                ?? throw new InvalidCastException(
                    $"No matching interface implementation in '{nameof(serviceDescriptor.ImplementationType)}'."
                );

            webApplicationServiceCollection.AddScoped(
                matchingInterface,
                x =>
                {
                    return x.GetRequiredService(concreteType!);
                }
            );
        }

        return webApplicationServiceCollection;
    }
}
