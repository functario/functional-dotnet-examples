using Microsoft.Kiota.Abstractions;

namespace Alexandria.Local.IntegrationTests.Support;

public static class NativeResponseHandlerExtensions
{
    public static HttpResponseMessage? GetHttpResponse(
        this NativeResponseHandler responseHandler
    ) => responseHandler?.Value as HttpResponseMessage;
}
