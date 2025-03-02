using Alexandria.WebApi;
using Alexandria.WebApi.Supports.EndpointMapper;

namespace Alexandria.WebApi.Endpoints.Authors;

internal sealed class AuthorsGroup : IGroup
{
    public AuthorsGroup(IEndpointRouteBuilder routeGroupBuilder)
    {
        Builder = routeGroupBuilder.MapGroup("v1/authors").WithOpenApi().WithTags("Authors");
    }

    public IEndpointRouteBuilder Builder { get; }
}
