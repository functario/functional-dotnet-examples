using Alexandria.WebApi.Supports.EndpointMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Alexandria.WebApi.Endpoints.Accounts.GetAccount;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Maintainability",
    "CA1515:Consider making public types internal",
    Justification = "<Pending>"
)]
public interface IGetAccountEndpoint : IGroupedEndpoint<AccountsGroup>
{
    Task<Results<Ok<GetAccountResponse>, NoContent>> HandleAsync(
        [FromQuery] long id,
        CancellationToken cancellationToken
    );
}
