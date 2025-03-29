using Alexandria.Application.BookUseCases.GetBookAuthors;
using Alexandria.Domain.BookDomain;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Alexandria.WebApi.Endpoints.Books.GetBookAuthors;

internal sealed class GetBookAuthorsEndpoint : IGetBookAuthorsEndpoint
{
    public const string EndpointName = "GetBookAuthors";

    public void Map(IEndpointRouteBuilder endpointBuilder)
    {
        endpointBuilder
            .MapGet("/bookAuthors", HandleAsync)
            .WithSummary($"Get a Book and its author(s).")
            .WithName(EndpointName);
    }

    public async Task<Results<Ok<GetBookAuthorsResponse>, NotFound>> HandleAsync(
        [FromServices] IGetBookAuthorsService getBookAuthorsService,
        [FromQuery] long id,
        CancellationToken cancellationToken
    )
    {
        var query = new GetBookAuthorsQuery(id);
        var response = await getBookAuthorsService.Handle(query, cancellationToken);
        if (response is not null)
        {
            var result = new GetBookAuthorsResponse(response.Book, response.Authors);
            return TypedResults.Ok(result);
        }

        return TypedResults.NotFound();
    }

    internal static object QueryObjectValue(Book book) => new { id = book.Id };
}
