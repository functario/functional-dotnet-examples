using Microsoft.Kiota.Abstractions;

namespace Alexandria.Local.IntegrationTests.Support;

internal static class KiotaExtensions
{
    public static void SetResponseHandler(
        RequestConfiguration<DefaultQueryParameters> config,
        NativeResponseHandler nativeResponseHandler
    )
    {
        //nativeResponseHandler = new NativeResponseHandler();
        config.Options.Add(new ResponseHandlerOption() { ResponseHandler = nativeResponseHandler });
    }
}
