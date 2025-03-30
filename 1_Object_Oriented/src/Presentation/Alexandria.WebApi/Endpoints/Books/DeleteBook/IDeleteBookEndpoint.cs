using Alexandria.Application.BookUseCases.DeleteBook;
using Alexandria.WebApi.Supports.EndpointMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Alexandria.WebApi.Endpoints.Books.DeleteBook;

internal interface IDeleteBookEndpoint : IGroupedEndpoint<BooksGroup>
{
    Task<Results<NoContent, NotFound>> HandleAsync(
        [FromServices] IDeleteBookService getAuthorService,
        [FromQuery] long id,
        CancellationToken cancellationToken
    );
}
