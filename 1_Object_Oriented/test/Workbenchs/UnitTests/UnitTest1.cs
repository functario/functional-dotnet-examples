using Alexandria.WebApi.Endpoints.Accounts.GetAccount;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Alexandria.WebApi.Workbench.UnitTests;

public class UnitTest1
{
    [Fact]
    public async Task Test()
    {
        // Arrange
        var getAccountEndpoint = Substitute.For<IGetAccountEndpoint>();
        var response = new GetAccountResponse($"accountId: {1}");
        Results<Ok<GetAccountResponse>, NoContent> result = TypedResults.Ok(response);

        getAccountEndpoint
            .HandleAsync(Arg.Any<long>(), Arg.Any<CancellationToken>())
            .Returns(x => Task.FromResult(result));

        // Act
        var sut = await getAccountEndpoint.HandleAsync(1, CancellationToken.None);

        // Assert
        sut.Should().Be(result);
    }

    [Theory, AutoDataNSubstitute]
    internal async Task Test2(IGetAccountEndpoint getAccountEndpoint)
    {
        // Arrange
        var response = new GetAccountResponse($"accountId: {1}");
        Results<Ok<GetAccountResponse>, NoContent> result = TypedResults.Ok(response);

        getAccountEndpoint
            .HandleAsync(Arg.Any<long>(), Arg.Any<CancellationToken>())
            .Returns(x => Task.FromResult(result));

        // Act
        var sut = await getAccountEndpoint.HandleAsync(1, CancellationToken.None);

        // Assert
        sut.Should().Be(result);
    }
}
