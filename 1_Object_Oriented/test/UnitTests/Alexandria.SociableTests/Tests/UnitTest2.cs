using Alexandria.WebApi.Endpoints.Accounts.GetAccount;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Alexandria.SociableTests.Tests;

public class UnitTest2
{
    private readonly IGetAccountEndpoint _getAccountEndpoint;

    public UnitTest2(WebAppServicesFactory webAppServicesFactory)
    {
        _getAccountEndpoint = webAppServicesFactory.CreateService<IGetAccountEndpoint>();
    }

    [Fact]
    public async Task Test1()
    {
        // Arrange
        var response = new GetAccountResponse($"accountId: {1}");
        Results<Ok<GetAccountResponse>, NoContent> result = TypedResults.Ok(response);

        // Act
        var sut = await _getAccountEndpoint.HandleAsync(1, CancellationToken.None);

        // Assert
        sut.Should().BeEquivalentTo(result);
    }

    [Theory]
    [InlineData(uint.MinValue)]
    [InlineData(1)]
    [InlineData(uint.MaxValue)]
    public async Task Test2(uint accountId)
    {
        // Arrange
        var response = new GetAccountResponse($"accountId: {accountId}");
        Results<Ok<GetAccountResponse>, NoContent> result = TypedResults.Ok(response);

        // Act
        var sut = await _getAccountEndpoint.HandleAsync(accountId, CancellationToken.None);

        // Assert
        sut.Should().BeEquivalentTo(result);
    }
}
