using Aspire.Hosting;
using WellKnowns.Aspires;
using WellKnowns.Presentation.AlexandriaWebApi;

namespace Alexandria.Local.IntegrationTests;

public class AspireEnvironment
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Performance",
        "CA1822:Mark members as static",
        Justification = "Injected via DI to support futur references"
    )]
    public async Task<DistributedApplication> StartAsync(CancellationToken cancellationToken)
    {
        var aspire = await DistributedApplicationTestingBuilder.CreateAsync<Projects.AppHost>(
            [AspireContexts.Test.ToString()],
            cancellationToken
        );

        var alexandriaWebApi = aspire.CreateResourceBuilder<ProjectResource>(
            WebApiProjectReferences.ProjectName
        );

        alexandriaWebApi.ApplicationBuilder.Services.ConfigureHttpClientDefaults(c =>
        {
            c.AddStandardResilienceHandler();
        });

        var app = await aspire.BuildAsync(cancellationToken);
        await app.StartAsync(cancellationToken);
        return app;
    }
}
