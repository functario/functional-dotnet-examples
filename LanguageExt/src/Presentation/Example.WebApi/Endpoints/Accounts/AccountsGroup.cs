using Example.WebApi.Supports.EndpointMapper;

namespace Example.WebApi.EndPoints.Accounts;

internal sealed class AccountsGroup : IGroup
{
    public AccountsGroup(IEndpointRouteBuilder routeGroupBuilder)
    {
        Builder = routeGroupBuilder.MapGroup("v1/accounts").WithOpenApi().WithTags("Accounts");
    }

    public IEndpointRouteBuilder Builder { get; }
}
