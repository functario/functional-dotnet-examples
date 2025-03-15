using System.Globalization;
using Alexandria.Local.IntegrationTests.Support;
using CleanArchitecture.WebAPI.Client;
using CleanArchitecture.WebAPI.Client.Models;
using Microsoft.Kiota.Abstractions;

namespace Alexandria.Local.IntegrationTests.Tests.Authors;

[Trait("Category", "Aspire")]
[Collection(nameof(IntegratedTests))]
public class AddAuthorTests3 : IAsyncLifetime
{
#pragma warning disable IDE0044 // Add readonly modifier
    private NativeResponseHandler _postAuthorsResponseHandler;
    private IntegratedTestFixture _integratedTestFixture;
    private AlexandriaClient _alexandriaClient;
    private HttpClient _alexandriaHttpClient;

#pragma warning restore IDE0044 // Add readonly modifier
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public AddAuthorTests3(IntegratedTestFixture integratedTestFixture)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    {
        _postAuthorsResponseHandler = new NativeResponseHandler();
        _integratedTestFixture = integratedTestFixture;
    }

    public async ValueTask InitializeAsync()
    {
        (_, _alexandriaHttpClient, _, _alexandriaClient) =
            await _integratedTestFixture.InitializeAsync();
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
        //await response.VerifyHttpResponseAsync();
    }
}
