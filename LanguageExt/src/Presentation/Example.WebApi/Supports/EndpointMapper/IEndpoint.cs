namespace Example.WebApi.Supports.EndpointMapper;

internal interface IEndpoint
{
    void Map(IEndpointRouteBuilder endpointBuilder);
}
