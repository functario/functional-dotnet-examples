using Alexandria.WebApi;
using Alexandria.WebApi.Supports.EndpointMapper;

namespace Alexandria.WebApi.Endpoints.Authors;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Maintainability",
    "CA1515:Consider making public types internal",
    Justification = "<Pending>"
)]
public sealed class AuthorsGroup : IGroup
{
    public AuthorsGroup(IEndpointRouteBuilder routeGroupBuilder)
    {
        Builder = routeGroupBuilder.MapGroup("v1/authors").WithOpenApi().WithTags("Authors");
    }

    public IEndpointRouteBuilder Builder { get; }
}
