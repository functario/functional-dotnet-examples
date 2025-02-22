using Example.WebApi.Supports.EndpointMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Example.WebApi.EndPoints.Accounts.GetAccount;

internal interface IGetAccountEndpoint : IGroupedEndpoint<AccountsGroup>
{
    Task<Results<Ok<GetAccountResponse>, NoContent>> HandleAsync(
        [FromQuery] ulong id,
        CancellationToken cancellationToken
    );
}
