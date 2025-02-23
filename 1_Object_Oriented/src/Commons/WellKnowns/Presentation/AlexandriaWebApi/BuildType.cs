using System.Reflection;

namespace WellKnowns.Presentation.AlexandriaWebApi;

public static class BuildType
{
    public static bool IsOpenApiGeneratorBuild()
    {
        // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/openapi/aspnetcore-openapi?view=aspnetcore-9.0&tabs=visual-studio#customizing-run-time-behavior-during-build-time-document-generation
        return Assembly.GetEntryAssembly()?.GetName().Name == "GetDocument.Insider";
    }
}
