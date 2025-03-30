using Alexandria.Application.AuthorUseCases.DeleteAuthor;
using Alexandria.WebApi.Supports.EndpointMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Alexandria.WebApi.Endpoints.Authors.DeleteAuthor;

internal interface IDeleteAuthorEndpoint : IGroupedEndpoint<AuthorsGroup>
{
    Task<Results<NoContent, NotFound>> HandleAsync(
        [FromServices] IDeleteAuthorService deleteAuthorService,
        [FromQuery] long id,
        CancellationToken cancellationToken
    );
}
