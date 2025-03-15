using System.Globalization;
using Alexandria.Local.IntegrationTests.Support;
using CleanArchitecture.WebAPI.Client.Models;
using Microsoft.Kiota.Abstractions;

namespace Alexandria.Local.IntegrationTests.Tests.Authors;

[Collection(nameof(IntegratedTestCollection))]
public class AddAuthorTests2 : IntegratedTestFixture
{
    private readonly NativeResponseHandler _postAuthorsResponseHandler;

    public AddAuthorTests2()
    {
        _postAuthorsResponseHandler = new NativeResponseHandler();
    }

    [Fact]
    public async Task Create_1_Author()
    {
        // Arrange
        var authorRequest = new AddAuthorRequest()
        {
            FirstName = "Tom",
            MiddleNames = ["The 3rd"],
            LastName = "Challenge",
            BirthDate = DateTime.Parse("2025-03-10T11:17:38.733Z", CultureInfo.InvariantCulture),
        };

        // Act
        var sut = await AlexandriaClient.V1.Authors.PostAsync(
            authorRequest,
            c => SetResponseHandler(c, _postAuthorsResponseHandler),
            cancellationToken: TestContext.Current.CancellationToken
        );

        var response = _postAuthorsResponseHandler.GetHttpResponse();

        // Assert
        //await response.VerifyHttpResponseAsync();
    }
}
