using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using NSubstitute;
using RDH.AspPresentation.LoggingRedactors;
using Xunit;

namespace AspPresentationTests;

public class LoggingRedactorsTests
{
    private static readonly string _inputStr = "Random";
    private static ReadOnlySpan<char> Input => _inputStr.ToCharArray();
    private static readonly string _inputEmailStr = "Random@Test.com";
    private static ReadOnlySpan<char> InputEmail => _inputEmailStr.ToCharArray();

    [Fact]
    public void FullStar_NotDevelopment()
    {
        // Arrange
        var env = Substitute.For<IWebHostEnvironment>();
        env.EnvironmentName.Returns("");

        // Act
        var sut = new FullStarRedactor(env);
        var len = sut.GetRedactedLength(Input);
        var red = sut.Redact(Input);

        //Assert
        len.Should().Be(4);
        red.Should().Be("****");
    }

    [Fact]
    public void FullStar_Development()
    {
        // Arrange
        var env = Substitute.For<IWebHostEnvironment>();
        env.EnvironmentName.Returns("Development");

        // Act
        var sut = new FullStarRedactor(env);
        var len = sut.GetRedactedLength(Input);
        var red = sut.Redact(Input);

        //Assert
        len.Should().Be(13);
        red.Should().Be("**** [Random]");
    }

    [Fact]
    public void PartialStar_NotDevelopment()
    {
        // Arrange
        var env = Substitute.For<IWebHostEnvironment>();
        env.EnvironmentName.Returns("");

        // Act
        var sut = new PartialStarRedactor(env);
        var len = sut.GetRedactedLength(Input);
        var red = sut.Redact(Input);

        //Assert
        len.Should().Be(6);
        red.Should().Be("R****m");
    }

    [Fact]
    public void PartialStar_Development()
    {
        // Arrange
        var env = Substitute.For<IWebHostEnvironment>();
        env.EnvironmentName.Returns("Development");

        // Act
        var sut = new PartialStarRedactor(env);
        var len = sut.GetRedactedLength(Input);
        var red = sut.Redact(Input);

        //Assert
        len.Should().Be(15);
        red.Should().Be("R****m [Random]");
    }

    [Fact]
    public void PartialStar_Email_NotDevelopment()
    {
        // Arrange
        var env = Substitute.For<IWebHostEnvironment>();
        env.EnvironmentName.Returns("");

        // Act
        var sut = new PartialStarRedactor(env);
        var len = sut.GetRedactedLength(InputEmail);
        var red = sut.Redact(InputEmail);

        //Assert
        len.Should().Be(13);
        red.Should().Be("R****m@T****m");
    }

    [Fact]
    public void PartialStar_Email_Development()
    {
        // Arrange
        var env = Substitute.For<IWebHostEnvironment>();
        env.EnvironmentName.Returns("Development");

        // Act
        var sut = new PartialStarRedactor(env);
        var len = sut.GetRedactedLength(InputEmail);
        var red = sut.Redact(InputEmail);

        //Assert
        len.Should().Be(31);
        red.Should().Be("R****m@T****m [Random@Test.com]");
    }
}