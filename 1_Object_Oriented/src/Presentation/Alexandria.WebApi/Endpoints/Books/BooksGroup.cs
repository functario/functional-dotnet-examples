using Alexandria.WebApi.Supports.EndpointMapper;

namespace Alexandria.WebApi.Endpoints.Books;

internal sealed class BooksGroup : IGroup
{
    public BooksGroup(IEndpointRouteBuilder routeGroupBuilder)
    {
        Builder = routeGroupBuilder.MapGroup("v1/books").WithOpenApi().WithTags("Books");
    }

    public IEndpointRouteBuilder Builder { get; }
}
