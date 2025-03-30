using Alexandria.Application.BookUseCases.GetBook;
using Alexandria.Domain.BookDomain;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Alexandria.WebApi.Endpoints.Books.GetBook;

internal sealed class GetBookEndpoint : IGetBookEndpoint
{
    public const string EndpointName = "GetBook";

    public void Map(IEndpointRouteBuilder endpointBuilder)
    {
        endpointBuilder.MapGet("/", HandleAsync).WithSummary($"Get a Book.").WithName(EndpointName);
    }

    public async Task<Results<Ok<GetBookResponse>, NotFound>> HandleAsync(
        [FromServices] IGetBookService getAuthorService,
        [FromQuery] long id,
        CancellationToken cancellationToken
    )
    {
        var query = new GetBookQuery(id);
        var response = await getAuthorService.HandleAsync(query, cancellationToken);
        if (response is not null)
        {
            var result = new GetBookResponse(response.Book);
            return TypedResults.Ok(result);
        }

        return TypedResults.NotFound();
    }

    internal static object QueryObjectValue(Book book) => new { id = book.Id };
}
