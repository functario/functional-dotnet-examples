using System.Diagnostics.CodeAnalysis;
using Alexandria.SQLSeeding;
using Aspire.Hosting;
using CleanArchitecture.WebAPI.Client;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using WellKnowns.Aspires;
using WellKnowns.Infrastructure.SQL;
using WellKnowns.Presentation.AlexandriaWebApi;

namespace Alexandria.Local.IntegrationTests.Support;

public class IntegratedTestFixture
{
    [SuppressMessage(
        "Performance",
        "CA1822:Mark members as static",
        Justification = "Allowed for DI injection given this class may extend."
    )]
    public async Task<(
        HttpClient alexandriaHttpClient,
        AlexandriaClient alexandriaClient
    )> InitializeAsync()
    {
        // Create AppHost with Aspire
        var appHost = await StartAppHostAsync(TestContext.Current.CancellationToken);

        // Reset database between test. The database is persistent.
        await ResetSQLDatabaseAsync(appHost, TestContext.Current.CancellationToken);

        // Configure Alexandria HttpClient
        var alexandriaHttpClient = appHost.CreateHttpClient(WebApiProjectReferences.ProjectName);
        using var requestAdapter = new HttpClientRequestAdapter(
            new AnonymousAuthenticationProvider(),
            httpClient: alexandriaHttpClient
        );

        var alexandriaClient = new AlexandriaClient(requestAdapter);
        return (alexandriaHttpClient, alexandriaClient);
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

    private static async Task<DistributedApplication> StartAppHostAsync(
        CancellationToken cancellationToken
    )
    {
        var aspire = await DistributedApplicationTestingBuilder.CreateAsync<Projects.AppHost>(
            [AspireContexts.Test.ToString()],
            cancellationToken
        );

        var alexandriaWebApi = aspire.CreateResourceBuilder<ProjectResource>(
            WebApiProjectReferences.ProjectName
        );

        alexandriaWebApi.ApplicationBuilder.Services.ConfigureHttpClientDefaults(builder =>
        {
            builder.AddStandardResilienceHandler();
        });

        var appHost = await aspire.BuildAsync(cancellationToken);
        await appHost.StartAsync(cancellationToken);
        return appHost;
    }
}
