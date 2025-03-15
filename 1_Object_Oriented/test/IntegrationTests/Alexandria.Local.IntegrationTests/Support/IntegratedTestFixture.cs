using Alexandria.SQLSeeding;
using Aspire.Hosting;
using CleanArchitecture.WebAPI.Client;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using WellKnowns.Aspires;
using WellKnowns.Infrastructure.SQL;
using WellKnowns.Presentation.AlexandriaWebApi;

namespace Alexandria.Local.IntegrationTests.Support;

public class IntegratedTestFixture : IAsyncLifetime
{
    public static CancellationToken TestCancellationToken => TestContext.Current.CancellationToken;
    public DistributedApplication AppHost { get; private set; } = null!;

    public AlexandriaClient AlexandriaClient { get; private set; } = null!;

    private HttpClient? HttpClient { get; set; } = null!;

    private HttpClientRequestAdapter? RequestAdapter { get; set; } = null!;

    public async ValueTask DisposeAsync()
    {
        await Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual async ValueTask Dispose(bool disposing)
    {
        if (disposing)
        {
            HttpClient?.Dispose();
            RequestAdapter?.Dispose();

            if (AppHost is not null)
            {
                await AppHost.StopAsync(TestCancellationToken);
                await AppHost.DisposeAsync();
            }
        }
    }

    public async ValueTask InitializeAsync()
    {
        // Create AppHost with Aspire
        AppHost = await StartAppHostAsync(TestContext.Current.CancellationToken);

        // Reset database between test. The database is persistent.
        await ResetSQLDatabaseAsync(AppHost, TestCancellationToken);

        // Configure Alexandria HttpClient
        HttpClient = AppHost.CreateHttpClient(WebApiProjectReferences.ProjectName);
        RequestAdapter = new HttpClientRequestAdapter(
            new AnonymousAuthenticationProvider(),
            httpClient: HttpClient
        );

        AlexandriaClient = new AlexandriaClient(RequestAdapter);
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

#pragma warning restore IDE0059 // Unnecessary assignment of a value
