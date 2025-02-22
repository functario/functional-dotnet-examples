using System.Text;

namespace Exampe.WebApi.Workbench;

public class UnitTest1
{
    [Fact]
    public void Test()
    {
        // Arrange
        var fakeString = "A fake string";
        var stringBuilder = Substitute.For<StringBuilder>();
        stringBuilder.ToString().Returns(fakeString);

        // Act
        var sut = stringBuilder.ToString();

        // Assert
        sut.Should().Be(fakeString);
    }
}
