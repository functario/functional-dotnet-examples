using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Alexandria.WebApi.Endpoints.Accounts.GetAccount;

internal class GetAccountEndpoint : IGetAccountEndpoint
{
    public void Map(IEndpointRouteBuilder endpointBuilder)
    {
        endpointBuilder
            .MapGet("/", HandleAsync)
            .WithSummary($"Get an Account.")
            .WithName("GetAccount");
    }

    public async Task<Results<Ok<GetAccountResponse>, NoContent>> HandleAsync(
        [FromQuery] ulong id,
        CancellationToken cancellationToken
    )
    {
        //https://localhost:7022/v1/accounts?id=1
        await Task.Delay(10, cancellationToken);
        var response = new GetAccountResponse($"accountId: {id}");
        return TypedResults.Ok(response);
    }
}

[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Maintainability",
    "CA1515:Consider making public types internal",
    Justification = "<Pending>"
)]
public readonly record struct GetAccountResponse(string Account) { }
