using Alexandria.Application.Abstractions.Repositories.Exceptions;
using Alexandria.Application.AuthorUseCases.AddAuthor;
using Alexandria.WebApi.Endpoints.Authors.GetAuthor;
using Alexandria.WebApi.Supports;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Alexandria.WebApi.Endpoints.Authors.AddAuthor;

internal sealed class AddAuthorEndpoint : IAddAuthorEndpoint
{
    public const string PostAuthorName = "PostAuthor";

    public void Map(IEndpointRouteBuilder endpointBuilder)
    {
        endpointBuilder
            .MapPost("/", HandleAsync)
            .WithSummary($"Add an Author.")
            .WithName(PostAuthorName);
    }

    public async Task<
        Results<Created<AddAuthorResponse>, Conflict<AuthorAlreadyExistsResponse>>
    > HandleAsync(
        [FromServices] IAddAuthorService addAuthorService,
        LinkGenerator linkGenerator,
        HttpContext httpContext,
        [FromBody] AddAuthorRequest authorRequest,
        CancellationToken cancellationToken
    )
    {
        var command = new AddAuthorCommand(authorRequest.ToAuthor());
        try
        {
            var response = await addAuthorService.Handle(command, cancellationToken);
            var result = new AddAuthorResponse(response.Author);
            var uri = linkGenerator.GetLocationUri(
                httpContext,
                GetAuthorEndpoint.GetAuthorName,
                GetAuthorEndpoint.QueryObjectValue(result.Author)
            )!;

            return TypedResults.Created(uri, result);
        }
        catch (AuthorAlreadyExistsException)
        {
            return TypedResults.Conflict(new AuthorAlreadyExistsResponse(authorRequest.ToAuthor()));
        }
    }
}
