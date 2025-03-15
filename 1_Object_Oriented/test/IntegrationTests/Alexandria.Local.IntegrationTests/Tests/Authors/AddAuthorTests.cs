using System.Globalization;
using Alexandria.Local.IntegrationTests.Support;
using CleanArchitecture.WebAPI.Client;
using CleanArchitecture.WebAPI.Client.Models;
using Microsoft.Kiota.Abstractions;

namespace Alexandria.Local.IntegrationTests.Tests.Authors;

[Trait("Category", "Aspire")]
[Collection(nameof(IntegratedTests))]
public class AddAuthorTests : IAsyncLifetime
{
    private readonly NativeResponseHandler _postAuthorsResponseHandler;
    private readonly IntegratedTestFixture _integratedTestFixture;
    private AlexandriaClient _alexandriaClient;
    private HttpClient _alexandriaHttpClient;

    public AddAuthorTests(IntegratedTestFixture integratedTestFixture)
    {
        _postAuthorsResponseHandler = new NativeResponseHandler();
        _integratedTestFixture = integratedTestFixture;
    }

    public async ValueTask InitializeAsync()
    {
        (_alexandriaHttpClient, _alexandriaClient) = await _integratedTestFixture.InitializeAsync();
    }

    public ValueTask DisposeAsync()
    {
        _alexandriaHttpClient?.Dispose();
        return ValueTask.CompletedTask;
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
        var sut = await _alexandriaClient.V1.Authors.PostAsync(
            authorRequest,
            c => SetResponseHandler(c, _postAuthorsResponseHandler),
            cancellationToken: TestContext.Current.CancellationToken
        );

        var response = _postAuthorsResponseHandler.GetHttpResponse();

        // Assert
        await response.VerifyHttpResponseAsync();
    }
}
