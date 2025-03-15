using System.Globalization;
using Alexandria.Local.IntegrationTests.Support;
using Alexandria.SQLSeeding;
using Aspire.Hosting;
using CleanArchitecture.WebAPI.Client;
using CleanArchitecture.WebAPI.Client.Models;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using WellKnowns.Aspires;
using WellKnowns.Infrastructure.SQL;
using WellKnowns.Presentation.AlexandriaWebApi;

namespace Alexandria.Local.IntegrationTests.Tests.Authors;

[Trait("Category", "Aspire")]
[Collection(nameof(IntegratedTests))]
public class AddAuthorTests2
{
    private readonly NativeResponseHandler _postAuthorsResponseHandler;

    public AddAuthorTests2()
    {
        _postAuthorsResponseHandler = new NativeResponseHandler();
    }

    [Fact]
    public async Task Create_1_Author()
    {
        var aspire = await DistributedApplicationTestingBuilder.CreateAsync<Projects.AppHost>(
            [AspireContexts.Test.ToString()],
            TestContext.Current.CancellationToken
        );

        var alexandriaWebApi = aspire.CreateResourceBuilder<ProjectResource>(
            WebApiProjectReferences.ProjectName
        );

        alexandriaWebApi.ApplicationBuilder.Services.ConfigureHttpClientDefaults(builder =>
        {
            builder.AddStandardResilienceHandler();
        });

        using var appHost = await aspire.BuildAsync(TestContext.Current.CancellationToken);

        // Configure Alexandria HttpClient
        await appHost.StartAsync(TestContext.Current.CancellationToken);

        // Reset database between test. The database is persistent.
        await ResetSQLDatabaseAsync(appHost, TestContext.Current.CancellationToken);

        using var alexandriaHttpClient = appHost.CreateHttpClient(
            WebApiProjectReferences.ProjectName
        );

        using var requestAdapter = new HttpClientRequestAdapter(
            new AnonymousAuthenticationProvider(),
            httpClient: alexandriaHttpClient
        );

        var alexandriaClient = new AlexandriaClient(requestAdapter);

        // Arrange
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

    private static async Task ResetSQLDatabaseAsync(
        DistributedApplication appHost,
        CancellationToken cancellationToken
    )
    {
        var connectionString =
            await appHost.GetConnectionStringAsync(
                SqlProjectReferences.ProjectName,
                cancellationToken
            )
            ?? throw new InvalidOperationException(
                $"Could not get SQL Connection string from {SqlProjectReferences.ProjectName}"
            );

        await SQLSeeder.ResetAsync(connectionString, cancellationToken);
    }
}
