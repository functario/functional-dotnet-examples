using Alexandria.Application.Abstractions.Repositories.Exceptions;
using Alexandria.Application.AuthorUseCases.AddAuthor;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Alexandria.WebApi.Endpoints.Authors.AddAuthor;

internal sealed class AddAuthorEndpoint : IAddAuthorEndpoint
{
    public void Map(IEndpointRouteBuilder endpointBuilder)
    {
        endpointBuilder
            .MapPost("/", HandleAsync)
            .WithSummary($"Add an Author.")
            .WithName("AddAuthor");
    }

    public async Task<
        Results<Created<AddAuthorResponse>, Conflict<AuthorAlreadyExistsResponse>>
    > HandleAsync(
        [FromServices] IAddAuthorService addAuthorService,
        [FromBody] AddAuthorRequest authorRequest,
        CancellationToken cancellationToken
    )
    {
        var command = new AddAuthorCommand(authorRequest.ToAuthor());
        try
        {
            var response = await addAuthorService.Handle(command, cancellationToken);
            var result = new AddAuthorResponse(response.Author);
            return TypedResults.Created("", result);
        }
        catch (AuthorAlreadyExistsException)
        {
            return TypedResults.Conflict(new AuthorAlreadyExistsResponse(authorRequest.ToAuthor()));
        }
    }
}
