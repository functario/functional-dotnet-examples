namespace Example.WebApi.Supports.EndpointMapper;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Maintainability",
    "CA1515:Consider making public types internal",
    Justification = "<Pending>"
)]
public interface IEndpoint
{
    public void Map(IEndpointRouteBuilder endpointBuilder);
}
