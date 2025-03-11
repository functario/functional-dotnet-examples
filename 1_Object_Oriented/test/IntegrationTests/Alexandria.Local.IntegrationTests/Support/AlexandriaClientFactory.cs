using Aspire.Hosting;
using CleanArchitecture.WebAPI.Client;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using WellKnowns.Presentation.AlexandriaWebApi;

namespace Alexandria.Local.IntegrationTests.Support;

public sealed class AlexandriaClientFactory : IDisposable
{
    private HttpClient? HttpClient { get; set; } = null!;
    private HttpClientRequestAdapter? RequestAdapter { get; set; } = null!;

    public AlexandriaClient CreateAlexandriaClient(DistributedApplication app)
    {
        HttpClient = app.CreateHttpClient(WebApiProjectReferences.ProjectName);

        RequestAdapter = new HttpClientRequestAdapter(
            new AnonymousAuthenticationProvider(),
            httpClient: HttpClient
        );

        var alexandriaClient = new AlexandriaClient(RequestAdapter);

        return alexandriaClient;
    }

    public void Dispose()
    {
        HttpClient?.Dispose();
        RequestAdapter?.Dispose();
    }
}

public static class NativeResponseHandlerExtensions
{
    public static HttpResponseMessage? GetHttpResponse(
        this NativeResponseHandler responseHandler
    ) => responseHandler?.Value as HttpResponseMessage;
}
