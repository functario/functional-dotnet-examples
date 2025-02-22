using Example.WebApi.Supports.EndpointMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Example.WebApi.EndPoints.Accounts.GetAccount;

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Maintainability",
    "CA1515:Consider making public types internal",
    Justification = "<Pending>"
)]
public interface IGetAccountEndpoint : IGroupedEndpoint<AccountsGroup>
{
    Task<Results<Ok<GetAccountResponse>, NoContent>> HandleAsync(
        [FromQuery] ulong id,
        CancellationToken cancellationToken
    );
}
