using Alexandria.Application.AuthorUseCases.GetAuthor;
using Alexandria.WebApi.Supports.EndpointMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Alexandria.WebApi.Endpoints.Authors.GetAuthor;

internal interface IGetAuthorEndpoint : IGroupedEndpoint<AuthorsGroup>
{
    Task<Results<Ok<GetAuthorResponse>, NotFound>> HandleAsync(
        [FromServices] IGetAuthorService getAuthorService,
        [FromBody] long authorId,
        CancellationToken cancellationToken
    );
}
