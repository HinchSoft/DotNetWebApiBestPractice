using CQRSServices.ApiResults;
using CQRSServices.Results;
using CQRSServicesTests.TestObjects;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CQRSServicesTests;

public class ResultHttpMappingTests
{
    private string[] NoErrors = Array.Empty<string>();

    [Fact]
    public void Result_ReturnOK()
    {
        // Arrange
        var sut = new Result<TestClass1>(NoErrors, false, false, new TestClass1 { FirstName = "Test" });

        // Act
        var res = sut.HttpResult();

        // Assert
        res.Should().NotBeNull();
        res.Should().BeOfType<Ok<TestClass1>>();
    }

    [Fact]
    public void ResultNoValue_ReturnNoContent()
    {
        // Arrange
        var sut = new Result<TestClass1>(NoErrors, false, false, null);

        // Act
        var res = sut.HttpResult();

        // Assert
        res.Should().NotBeNull();
        res.Should().BeOfType<NoContent>();
    }

    [Fact]
    public void Result_ReturnNoContent()
    {
        // Arrange
        var sut = new Result(NoErrors,false,false);

        // Act
        var res = sut.HttpResult();

        // Assert
        res.Should().NotBeNull();
        res.Should().BeOfType<NoContent>();

    }
}
