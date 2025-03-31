using System.Diagnostics;
using Alexandria.Application.Abstractions.Repositories.Exceptions;
using Alexandria.Application.BookUseCases.DeleteBook;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Alexandria.WebApi.Endpoints.Books.DeleteBook;

internal sealed class DeleteBookEndpoint : IDeleteBookEndpoint
{
    public const string EndpointName = "DeleteBook";

    public void Map(IEndpointRouteBuilder endpointBuilder)
    {
        endpointBuilder
            .MapDelete("/", HandleAsync)
            .WithSummary($"Delete a Book.")
            .WithName(EndpointName);
    }

    public async Task<Results<NoContent, NotFound>> HandleAsync(
        [FromServices] IDeleteBookService deleteBookService,
        [FromQuery] long id,
        CancellationToken cancellationToken
    )
    {
        var query = new DeleteBookCommand(id);
        try
        {
            var response = await deleteBookService.HandleAsync(query, cancellationToken);
            return response.BookId == id
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
