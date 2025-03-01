using Alexandria.Application.AuthorUseCases.GetAuthor;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Alexandria.WebApi.Endpoints.Authors.GetAuthor;

internal sealed class GetAuthorEndpoint : IGetAuthorEndpoint
{
    public void Map(IEndpointRouteBuilder endpointBuilder)
    {
        endpointBuilder
            .MapGet("/", HandleAsync)
            .WithSummary($"Get an Author.")
            .WithName("GetAuthor");
    }

    public async Task<Results<Ok<GetAuthorResponse>, NotFound>> HandleAsync(
        [FromServices] IGetAuthorService getAuthorService,
        [FromQuery] long authorId,
        CancellationToken cancellationToken
    )
    {
        var query = new GetAuthorQuery(authorId);
        var response = await getAuthorService.Handle(query, cancellationToken);
        if (response is not null)
        {
            var result = new GetAuthorResponse(response.Author);
            return TypedResults.Ok(result);
        }

        return TypedResults.NotFound();
    }
}
