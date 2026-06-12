using AspPresentation.LoggingRedactors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NSubstitute;

namespace AspPresentationTests;

public class LoggingRedactorsTests
{

    private static readonly string _inputStr = "Random";
    private static ReadOnlySpan<char> Input => _inputStr.ToCharArray();
    private static readonly string _inputEmailStr = "Random@Test.com";
    private static ReadOnlySpan<char> InputEmail => _inputEmailStr.ToCharArray();

    [Test]
    public void FullStar_NotDevelopment()
    {
        // Arrange
        var env = Substitute.For<IWebHostEnvironment>();
        env.EnvironmentName.Returns("");

        // Act
        var sut = new FullStarRedactor(env);
        var len = sut.GetRedactedLength(Input);
        var red=sut.Redact(Input);

        //Assert
         Assert.That(len, Is.EqualTo(4));
         Assert.That(red, Is.EqualTo("****"));
    }

    [Test]
    public void FullStar_Development()
    {
        // Arrange
        var env = Substitute.For<IWebHostEnvironment>();
        env.EnvironmentName.Returns("Development");

        // Act
        var sut = new FullStarRedactor(env);
        var len = sut.GetRedactedLength(Input);
        var red=sut.Redact(Input);

        //Assert
        Assert.That(len, Is.EqualTo(13));
        Assert.That(red, Is.EqualTo($"**** [Random]"));
    }

    [Test]
    public void PartialStar_NotDevelopment()
    {
        // Arrange
        var env = Substitute.For<IWebHostEnvironment>();
        env.EnvironmentName.Returns("");

        // Act
        var sut = new PartialStarRedactor(env);
        var len = sut.GetRedactedLength(Input);
        var red=sut.Redact(Input);

        //Assert
        Assert.That(len, Is.EqualTo(6));
        Assert.That(red, Is.EqualTo("R****m"));
    }

    [Test]
    public void PartialStar_Development()
    {
        // Arrange
        var env = Substitute.For<IWebHostEnvironment>();
        env.EnvironmentName.Returns("Development");

        // Act
        var sut = new PartialStarRedactor(env);
        var len = sut.GetRedactedLength(Input);
        var red=sut.Redact(Input);

        //Assert
        Assert.That(len, Is.EqualTo(15));
        Assert.That(red, Is.EqualTo($"R****m [Random]"));
    }

    [Test]
    public void PartialStar_Email_NotDevelopment()
    {
        // Arrange
        var env = Substitute.For<IWebHostEnvironment>();
        env.EnvironmentName.Returns("");

        // Act
        var sut = new PartialStarRedactor(env);
        var len = sut.GetRedactedLength(InputEmail);
        var red=sut.Redact(InputEmail);

        //Assert
        Assert.That(len, Is.EqualTo(13));
        Assert.That(red, Is.EqualTo("R****m@T****m"));
    }

    [Test]
    public void PartialStar_Email_Development()
    {
        // Arrange
        var env = Substitute.For<IWebHostEnvironment>();
        env.EnvironmentName.Returns("Development");

        // Act
        var sut = new PartialStarRedactor(env);
        var len = sut.GetRedactedLength(InputEmail);
        var red=sut.Redact(InputEmail);

        //Assert
        Assert.That(len, Is.EqualTo(31));
        Assert.That(red, Is.EqualTo($"R****m@T****m [Random@Test.com]"));
    }
}
