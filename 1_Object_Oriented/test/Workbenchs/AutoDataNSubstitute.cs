using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit3;

namespace Alexandria.WebApi.Workbench;

internal sealed class AutoDataNSubstitute : AutoDataAttribute
{
    internal AutoDataNSubstitute()
        : base(
            () =>
                new Fixture().Customize(
                    new AutoNSubstituteCustomization() { ConfigureMembers = true }
                )
        ) { }
}
