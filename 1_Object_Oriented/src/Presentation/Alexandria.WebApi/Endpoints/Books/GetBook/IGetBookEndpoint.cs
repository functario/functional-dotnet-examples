using Alexandria.Application.BookUseCases.GetBook;
using Alexandria.WebApi.Supports.EndpointMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Alexandria.WebApi.Endpoints.Books.GetBook;

internal interface IGetBookEndpoint : IGroupedEndpoint<BooksGroup>
{
    Task<Results<Ok<GetBookResponse>, NotFound>> HandleAsync(
        [FromServices] IGetBookService getAuthorService,
        [FromQuery] long id,
        CancellationToken cancellationToken
    );
}
