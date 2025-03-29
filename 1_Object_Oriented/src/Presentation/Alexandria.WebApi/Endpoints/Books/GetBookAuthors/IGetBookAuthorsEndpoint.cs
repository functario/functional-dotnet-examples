using Alexandria.Application.BookUseCases.GetBookAuthors;
using Alexandria.WebApi.Supports.EndpointMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Alexandria.WebApi.Endpoints.Books.GetBookAuthors;

internal interface IGetBookAuthorsEndpoint : IGroupedEndpoint<BooksGroup>
{
    Task<Results<Ok<GetBookAuthorsResponse>, NotFound>> HandleAsync(
        [FromServices] IGetBookAuthorsService getAuthorService,
        [FromQuery] long id,
        CancellationToken cancellationToken
    );
}
