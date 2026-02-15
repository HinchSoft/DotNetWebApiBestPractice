using CQRSServices.ServiceResponses;
using CQRSServicesTests.TestObjects;
using FluentAssertions;

namespace CQRSServicesTests;

public class ResponseBuilderTests
{
    [Fact]
    public void ResponseHasMultipleErrors()
    {
        // Arrange
        var sut = new ResponseBuilder();

        // Act
        sut.AddError("Error1");
        sut.AddError("Error2");
        var res = sut.Build();

        // Assert
        res.Errors.Should().HaveCount(2);
    }

    [Fact]
    public void ResponseRemovesDuplicateErrors()
    {
        // Arrange
        var sut = new ResponseBuilder();

        // Act
        sut.AddError("Error");
        sut.AddError("Error");
        var res = sut.Build();

        // Assert
        res.Errors.Should().HaveCount(1);
    }

    [Fact]
    public void GenericResponseTakesResponse_OK()
    {
        // Arrange
        var b2 = new Response(new string[] { }, false, false);
        var sut = new ResponseBuilder<TestClass1>();

        // Act
        sut.AddResponse(b2);
        var res = sut.Build();

        // assert
        sut.HasFault.Should().BeFalse();
        res.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void GenericResponseTakesResponse_Errors()
    {
        // Arrange
        var b2 = new Response(new string[] {"An Error"}, false, false);
        var sut = new ResponseBuilder<TestClass1>();

        // Act 
        sut.AddResponse(b2);
        var res = sut.Build();

        // assert
        sut.HasFault.Should().BeTrue();
        res.Errors.Should().NotBeEmpty();
        res.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void GenericResponseTakesResponse_NotFound()
    {
        // Arrange
        var b2 = new Response(new string[] { "Not Found" }, true, false);
        var sut = new ResponseBuilder<TestClass1>();

        // Act
        sut.AddResponse(b2);
        var res = sut.Build();

        // Assert
        sut.HasFault.Should().BeTrue();
        res.Errors.Should().NotBeEmpty();
        res.IsSuccess.Should().BeFalse();
        res.NotFound.Should().BeTrue();
    }

    [Fact]
    public void GenericResponseTakesResponse_Conflict()
    {
        // Arrange
        var b2 = new Response(new string[] { "Conflict" }, false, true);
        var sut = new ResponseBuilder<TestClass1>();

        // Act
        sut.AddResponse(b2);
        var res = sut.Build();

        // Assert
        sut.HasFault.Should().BeTrue();
        res.Errors.Should().NotBeEmpty();
        res.IsSuccess.Should().BeFalse();
        res.Conflict.Should().BeTrue();
    }

    [Fact]
    public void GenericResponseTakesGenericResponse_ReturnsValue()
    {
        // Arange
        var r2 = new Response<TestClass2>(new string[] { }, false, false, new TestClass2 { Name = "object 2" });
        var sut = new ResponseBuilder<TestClass1>();

        // Act
        var val = sut.AddResponse(r2);
        var res = sut.Build();

        //Assert
        val.Should().NotBeNull();
        val.Name.Should().Be("object 2");
        sut.HasFault.Should().BeFalse();
        res.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void GenericResponseTakesGenericResponse_ReturnsNotFound()
    {
        // Arange
        var r2 = new Response<TestClass2>(new string[] {"Not Found"}, true, false, null);
        var sut = new ResponseBuilder<TestClass1>();

        // Act
        var val = sut.AddResponse(r2);
        var res = sut.Build();

        //Assert
        val.Should().BeNull();
        sut.HasFault.Should().BeTrue();
        res.IsSuccess.Should().BeFalse();
        res.Errors.Should().NotBeEmpty();
        res.NotFound.Should().BeTrue();
    }

}
