using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit3;

namespace Alexandria.SociableTests;

internal sealed class AutoDataNSubstitute : AutoDataAttribute
{
    internal AutoDataNSubstitute()
        : base(() =>
        {
            var f = new Fixture();

            var fixture = new Fixture().Customize(
                new AutoNSubstituteCustomization() { ConfigureMembers = true }
            );

            return fixture;
        }) { }
}
