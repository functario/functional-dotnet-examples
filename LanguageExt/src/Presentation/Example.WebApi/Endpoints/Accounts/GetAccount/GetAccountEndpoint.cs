using Example.WebApi.Supports.EndpointMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Example.WebApi.EndPoints.Accounts.GetAccount;

internal sealed class GetAccountEndpoint : IGroupedEndpoint<AccountsGroup>
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

internal readonly record struct GetAccountResponse(string Account) { }
