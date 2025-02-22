using System.Collections.Frozen;
using System.Reflection;

namespace Example.WebApi.Supports.EndpointMapper;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEndpoints(
        this IServiceCollection services,
        Assembly assembly
    )
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        ArgumentNullException.ThrowIfNull(assembly, nameof(assembly));

        var endpointsGroups = assembly.DefinedTypes.GetGroupedEndpointsAsDictionnary();

        return services.AddEndpoints(endpointsGroups).AddGroups(endpointsGroups);
    }

    private static Dictionary<Type, FrozenSet<Type>> GetGroupedEndpointsAsDictionnary(
        this IEnumerable<TypeInfo> types
    )
    {
        return types
            .Where(t => t.IsAssignableToEndPoint())
            .Select(x => x.GroupEndpoints())
            .GroupBy(x => x.group, x => x.endpointType)
            .ToDictionary(g => g.Key, g => g.ToFrozenSet());
    }

    private static IServiceCollection AddGroups(
        this IServiceCollection services,
        Dictionary<Type, FrozenSet<Type>> endpointsGroups
    )
    {
        return services.AddSingleton(x => new GroupedEndpoints(endpointsGroups));
    }

    private static IServiceCollection AddEndpoints(
        this IServiceCollection services,
        Dictionary<Type, FrozenSet<Type>> endpointsGroups
    )
    {
        foreach (var endpointType in endpointsGroups.Values.SelectMany(x => x.Items))
        {
            services.AddSingleton(endpointType);
        }

        return services;
    }

    private static bool IsAssignableToEndPoint(this Type type)
    {
        return type.GetInterfaces().Any(x => x.IsAssignableTo(typeof(IEndpoint)))
            && !type.IsInterface;
    }

    private static bool IsAssignableToGroup(this TypeInfo typeInfo)
    {
        return typeInfo.ImplementedInterfaces.Any(x =>
            x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IGroupedEndpoint<>)
        );
    }

    private static (Type endpointType, Type group) GroupEndpoints(this TypeInfo typeInfo)
    {
        var group = typeInfo.IsAssignableToGroup()
            ? typeInfo.GetEndpointGroupType()
            : typeof(GenericEndpointGroup);

        return (endpointType: typeInfo, group);
    }

    private static Type GetEndpointGroupType(this TypeInfo typeInfo)
    {
        return typeInfo
            .ImplementedInterfaces.Where(x =>
                x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IGroupedEndpoint<>)
            )
            .Select(x => x.GetGenericArguments().First())
            .Single();
    }
}
