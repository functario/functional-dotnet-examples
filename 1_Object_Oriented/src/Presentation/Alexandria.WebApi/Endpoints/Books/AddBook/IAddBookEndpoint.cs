using Alexandria.Application.BookUseCases.AddBook;
using Alexandria.WebApi.Supports.EndpointMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Alexandria.WebApi.Endpoints.Books.AddBook;

internal interface IAddBookEndpoint : IGroupedEndpoint<BooksGroup>
{
    Task<
        Results<
            Created<AddBookResponse>,
            NotFound<AuthorNotFoundResponse>,
            Conflict<BookAlreadyExistsResponse>
        >
    > HandleAsync(
        [FromServices] IAddBookService addBookService,
        LinkGenerator linkGenerator,
        HttpContext httpContext,
        [FromBody] AddBookRequest request,
        CancellationToken cancellationToken
    );
}
