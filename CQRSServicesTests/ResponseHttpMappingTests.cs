using CQRSServices.ServiceResponses;
using CQRSServicesTests.TestObjects;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQRSServicesTests;

public class ResponseHttpMappingTests
{
    private string[] NoErrors = Array.Empty<string>();

    [Fact]
    public void Response_ReturnOK()
    {
        // Arrange
        var sut = new Response<TestClass1>(NoErrors, false, false, new TestClass1 { FirstName = "Test" });

        // Act
        var res = sut.HttpResult();

        // Assert
        res.Should().NotBeNull();
        res.Should().BeOfType<Ok<TestClass1>>();
    }

    [Fact]
    public void ResponseNoValue_ReturnNoContent()
    {
        // Arrange
        var sut = new Response<TestClass1>(NoErrors, false, false, null);

        // Act
        var res = sut.HttpResult();

        // Assert
        res.Should().NotBeNull();
        res.Should().BeOfType<NoContent>();
    }

    [Fact]
    public void Response_ReturnNoContent()
    {
        // Arrange
        var sut = new Response(NoErrors,false,false);

        // Act

        // Assert

    }
}
