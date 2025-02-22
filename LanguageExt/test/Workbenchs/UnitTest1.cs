using Example.WebApi.EndPoints.Accounts.GetAccount;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Example.WebApi.Workbench;

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
            .HandleAsync(Arg.Any<ulong>(), Arg.Any<CancellationToken>())
            .Returns(x => Task.FromResult(result));

        // Act
        var sut = await getAccountEndpoint.HandleAsync(1, CancellationToken.None);

        // Assert
        sut.Should().Be(result);
    }
}
