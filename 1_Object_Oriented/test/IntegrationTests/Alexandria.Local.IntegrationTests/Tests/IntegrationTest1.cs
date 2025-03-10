using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using WellKnowns.Presentation.AlexandriaWebApi;

namespace Alexandria.Local.IntegrationTests.Tests;

public class IntegrationTest1
{
    private readonly AspireEnvironment _aspireEnvironment;

    public IntegrationTest1(AspireEnvironment aspireEnvironment)
    {
        _aspireEnvironment = aspireEnvironment;
    }

    [Fact]
    public async Task PostAuthorTest()
    {
        // Arrange
        using var aspireEnvironment = await _aspireEnvironment.StartAsync(
            TestContext.Current.CancellationToken
        );

        using var webApiHttpClient = aspireEnvironment.CreateHttpClient(
            WebApiProjectReferences.ProjectName
        );

        using var content = JsonContent.Create(
            new
            {
                FirstName = "Tom",
                MiddleNames = new List<string>() { "The 3rd" },
                LastName = "Challenge",
                BirthDate = DateTime.Parse(
                    "2025-03-10T11:17:38.733Z",
                    CultureInfo.InvariantCulture
                ),
            }
        );

        var baseAddress = webApiHttpClient.BaseAddress ?? throw new InvalidOperationException();
        var uri = new Uri(baseAddress, "/v1/authors");

        // Act
        var sut = await webApiHttpClient.PostAsync(
            uri,
            content,
            TestContext.Current.CancellationToken
        );

        // Assert
        sut.StatusCode.Should().Be(HttpStatusCode.Created);
    }
}
