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
#pragma warning disable CA1822 // Mark members as static
    public async Task<(
        DistributedApplication appHost,
        HttpClient alexandriaHttpClient,
        HttpClientRequestAdapter requestAdapdter,
        AlexandriaClient alexandriaClient
    )> InitializeAsync()
#pragma warning restore CA1822 // Mark members as static
    {
        // Create AppHost with Aspire
        var appHost = await StartAppHostAsync(TestContext.Current.CancellationToken);

        // Reset database between test. The database is persistent.
        await ResetSQLDatabaseAsync(appHost, TestContext.Current.CancellationToken);

        // Configure Alexandria HttpClient
#pragma warning disable CA2000 // Dispose objects before losing scope
        var alexandriaHttpClient = appHost.CreateHttpClient(WebApiProjectReferences.ProjectName);
#pragma warning restore CA2000 // Dispose objects before losing scope
        using var requestAdapter = new HttpClientRequestAdapter(
            new AnonymousAuthenticationProvider(),
            httpClient: alexandriaHttpClient
        );

        var alexandriaClient = new AlexandriaClient(requestAdapter);
        return (appHost, alexandriaHttpClient, requestAdapter, alexandriaClient);
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
