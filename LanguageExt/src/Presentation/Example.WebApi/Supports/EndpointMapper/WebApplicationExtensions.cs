namespace Example.WebApi.Supports.EndpointMapper;

internal static class WebApplicationExtensions
{
    public static WebApplication MapGroupedEndpoints(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app, nameof(app));
        var groupedEndpoints = app.Services.GetRequiredService<GroupedEndpoints>();

        foreach (var keyValuePair in groupedEndpoints.KeyValuePairs)
        {
            var groupType = keyValuePair.Key;
            var endpointBuilder = GetEndpointRouteBuilder(groupType, app);

            foreach (var item in keyValuePair.Value)
            {
                var endpoint =
                    app.Services.GetRequiredService(item) as IEndpoint
                    ?? throw new InvalidCastException(
                        $"Could not cast '{item.FullName}' as {nameof(IEndpoint)}."
                    );

                endpoint.Map(endpointBuilder);
            }
        }

        return app;
    }

    private static IEndpointRouteBuilder GetEndpointRouteBuilder(Type groupType, WebApplication app)
    {
        var endpointBuilder =
            groupType == typeof(GenericEndpointGroup)
                ? app
                : (Activator.CreateInstance(groupType, app) as IGroup)?.Builder;

        return endpointBuilder
            ?? throw new InvalidCastException(
                $"Type '{groupType.FullName}' is not a valid endpoint group."
            );
    }
}
