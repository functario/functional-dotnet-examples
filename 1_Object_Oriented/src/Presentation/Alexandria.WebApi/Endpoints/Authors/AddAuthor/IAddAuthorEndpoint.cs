using Alexandria.Application.AuthorUseCases.AddAuthor;
using Alexandria.WebApi.Endpoints.Accounts;
using Alexandria.WebApi.Supports.EndpointMapper;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Alexandria.WebApi.Endpoints.Authors.AddAuthor;

internal interface IAddAuthorEndpoint : IGroupedEndpoint<AccountsGroup>
{
    Task<Results<Created<AddAuthorResult>, Conflict<AuthorAlreadyExistsResult>>> HandleAsync(
        IAddAuthorService addAuthorService,
        AddAuthorRequest request,
        CancellationToken cancellationToken
    );
}
