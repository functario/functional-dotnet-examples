using Alexandria.Application.AuthorUseCases.AddAuthor;
using Alexandria.WebApi.Supports.EndpointMapper;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Alexandria.WebApi.Endpoints.Authors.AddAuthor;

internal interface IAddAuthorEndpoint : IGroupedEndpoint<AuthorsGroup>
{
    Task<Results<Created<AddAuthorResponse>, Conflict<AuthorAlreadyExistsResponse>>> HandleAsync(
        IAddAuthorService addAuthorService,
        AddAuthorRequest request,
        CancellationToken cancellationToken
    );
}
