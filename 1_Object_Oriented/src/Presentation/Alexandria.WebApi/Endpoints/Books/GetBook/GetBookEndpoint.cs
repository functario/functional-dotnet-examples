using Alexandria.Application.BookUseCases.GetBook;
using Alexandria.Domain.BookDomain;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Alexandria.WebApi.Endpoints.Books.GetBook;

internal sealed class GetBookEndpoint : IGetBookEndpoint
{
    public const string GetBookName = "GetBook";

    public void Map(IEndpointRouteBuilder endpointBuilder)
    {
        endpointBuilder.MapGet("/", HandleAsync).WithSummary($"Get a Book.").WithName(GetBookName);
    }

    public async Task<Results<Ok<GetBookResponse>, NotFound>> HandleAsync(
        [FromServices] IGetBookService getAuthorService,
        [FromQuery] long id,
        CancellationToken cancellationToken
    )
    {
        var query = new GetBookQuery(id);
        var response = await getAuthorService.Handle(query, cancellationToken);
        if (response is not null)
        {
            var result = new GetBookResponse(response.Book);
            return TypedResults.Ok(result);
        }

        return TypedResults.NotFound();
    }

    /// <summary>
    /// Returns a query object value to use with <see cref="LinkGenerator"/>
    /// to create the location URI.
    /// </summary>
    /// <param name="book">The author to query.</param>
    /// <returns>The query object value.</returns>
    internal static object QueryObjectValue(Book book) => new { id = book.Id };
}
