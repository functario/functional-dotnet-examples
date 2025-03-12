using System.Globalization;
using Alexandria.Local.IntegrationTests.Support;
using CleanArchitecture.WebAPI.Client.Models;
using Microsoft.Kiota.Abstractions;

namespace Alexandria.Local.IntegrationTests.Tests.Authors;

public class AddAuthorTests
{
    private readonly AspireEnvironment _aspireEnvironment;
    private readonly AlexandriaClientFactory _alexandriaClientFactory;
    private readonly NativeResponseHandler _postAuthorsResponseHandler;

    public AddAuthorTests(
        AspireEnvironment aspireEnvironment,
        AlexandriaClientFactory alexandriaClientFactory
    )
    {
        _aspireEnvironment = aspireEnvironment;
        _alexandriaClientFactory = alexandriaClientFactory;
        _postAuthorsResponseHandler = new NativeResponseHandler();
    }

    [Fact]
    public async Task Create_1_Author()
    {
        // Arrange
        using var app = await _aspireEnvironment.StartAsync(TestContext.Current.CancellationToken);

        var alexandriaClient = _alexandriaClientFactory.CreateAlexandriaClient(app);

        var authorRequest = new AddAuthorRequest()
        {
            FirstName = "Tom",
            MiddleNames = ["The 3rd"],
            LastName = "Challenge",
            BirthDate = DateTime.Parse("2025-03-10T11:17:38.733Z", CultureInfo.InvariantCulture),
        };

        // Act
        var sut = await alexandriaClient.V1.Authors.PostAsync(
            authorRequest,
            c => SetResponseHandler(c, _postAuthorsResponseHandler),
            cancellationToken: TestContext.Current.CancellationToken
        );

        var response = _postAuthorsResponseHandler.GetHttpResponse();

        // Assert
        await response.VerifyHttpResponseAsync();
    }
}
