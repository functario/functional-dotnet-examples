using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using NSubstitute;

namespace Alexandria.SociableTests.Extensions;

internal static class LinkGeneratorExtensions
{
    public static LinkGenerator SetGetUriByName(
        this LinkGenerator linkGenerator,
        HttpContext httpContext,
        string gerUri
    )
    {
        linkGenerator.GetUriByName(httpContext, string.Empty, null).ReturnsForAnyArgs(gerUri);
        return linkGenerator;
    }
}
