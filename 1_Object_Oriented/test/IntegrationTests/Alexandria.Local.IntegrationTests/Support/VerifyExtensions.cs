using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Alexandria.Local.IntegrationTests.Support;

public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Initialize() => VerifierSettings.InitializePlugins();
}

public class VerifyChecksTests
{
    [Fact]
    public Task CheckVerifyConventions() => VerifyChecks.Run();
}

public static class VerifyExtensions
{
    public static VerifySettings GetVerifySettings()
    {
        var verifySettings = new VerifySettings();
        verifySettings.AddScrubber(x => x.Replace("\r\n", "\n"));

        verifySettings.ScrubLinesWithReplace(line =>
        {
            if (line.Contains("http", StringComparison.OrdinalIgnoreCase))
            {
                // Scrub dynamic port from Location url
                var pattern = @":\d+";
                var modifiedUrl = Regex.Replace(line, pattern, ":Port_1");
                return modifiedUrl;
            }

            return line;
        });

        verifySettings.UseStrictJson();
        return verifySettings;
    }

    public static async Task<VerifyResult> VerifyHttpResponseAsync(this object? toVerify)
    {
        var verifySettings = GetVerifySettings();
        DerivePathInfo(
            (sourceFile, projectDirectory, type, method) =>
            {
                // Resolve path based on namespace because default sourcefile is based on
                // VerifySettings constructor emplacement.
                var root = Path.GetFileName(projectDirectory!.TrimEnd(Path.DirectorySeparatorChar));
                var typePath = type
                    .FullName?.Replace(root, "", StringComparison.OrdinalIgnoreCase)
                    ?.Replace(".", "/", StringComparison.OrdinalIgnoreCase)
                    ?.TrimStart('/')!;

                var directory = Path.Combine(projectDirectory, $"{typePath}_Snapshots");

                return new PathInfo(directory, typeName: "_", methodName: method.Name);
            }
        );

        return await Verify(toVerify, verifySettings).IgnoreMembers("Server");
    }
}
