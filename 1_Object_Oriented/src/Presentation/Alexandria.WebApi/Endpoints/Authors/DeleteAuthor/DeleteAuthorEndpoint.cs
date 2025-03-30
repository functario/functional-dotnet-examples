using System.Diagnostics;
using Alexandria.Application.Abstractions.Repositories.Exceptions;
using Alexandria.Application.AuthorUseCases.DeleteAuthor;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Alexandria.WebApi.Endpoints.Authors.DeleteAuthor;

internal sealed class DeleteAuthorEndpoint : IDeleteAuthorEndpoint
{
    public const string EndpointName = "DeleteAuthor";

    public void Map(IEndpointRouteBuilder endpointBuilder)
    {
        endpointBuilder
            .MapDelete("/", HandleAsync)
            .WithSummary($"Delete an Author.")
            .WithName(EndpointName);
    }

    public async Task<Results<NoContent, NotFound>> HandleAsync(
        [FromServices] IDeleteAuthorService deleteAuthorService,
        [FromQuery] long id,
        CancellationToken cancellationToken
    )
    {
        var query = new DeleteAuthorQuery(id);
        try
        {
            var response = await deleteAuthorService.HandleAsync(query, cancellationToken);
            return response.AuthorId == id
                ? (Results<NoContent, NotFound>)TypedResults.NoContent()
                : throw new InvalidDataException();
        }
        catch (EntityNotFoundException)
        {
            return TypedResults.NotFound();
        }

        throw new UnreachableException();
    }
}
