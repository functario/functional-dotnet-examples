using Alexandria.WebApi.Supports.EndpointMapper;

namespace Alexandria.WebApi.Endpoints.Accounts;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Maintainability",
    "CA1515:Consider making public types internal",
    Justification = "<Pending>"
)]
public sealed class AccountsGroup : IGroup
{
    public AccountsGroup(IEndpointRouteBuilder routeGroupBuilder)
    {
        Builder = routeGroupBuilder.MapGroup("v1/accounts").WithOpenApi().WithTags("Accounts");
    }

    public IEndpointRouteBuilder Builder { get; }
}
