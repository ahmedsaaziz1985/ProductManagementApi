using ProductManagementApi.Application.Common.Exceptions;
using ProductManagementApi.API.Common;
using ProductManagementApi.Domain.Exceptions;

namespace ProductManagementApi.IntegrationTests.Unit.API;

public class ExceptionMapperTests
{
    [Fact]
    public void Map_ShouldReturn400_ForValidationException()
    {
        var exception = new ValidationException(new Dictionary<string, string[]>
        {
            ["name"] = ["Name is required."]
        });

        var result = ExceptionMapper.Map(exception, "trace-123", includeDetails: true);

        Assert.Equal(400, result.Status);
        Assert.Equal("Validation failed.", result.Title);
        Assert.Equal("trace-123", result.TraceId);
        Assert.NotNull(result.Errors);
    }

    [Fact]
    public void Map_ShouldReturn404_ForNotFoundException()
    {
        var exception = new NotFoundException("Product", Guid.NewGuid());

        var result = ExceptionMapper.Map(exception, "trace-456", includeDetails: true);

        Assert.Equal(404, result.Status);
        Assert.Equal("Not found.", result.Title);
    }

    [Fact]
    public void Map_ShouldReturn409_ForConflictException()
    {
        var exception = new ConflictException("name", "A product with this name already exists.");

        var result = ExceptionMapper.Map(exception, "trace-789", includeDetails: true);

        Assert.Equal(409, result.Status);
        Assert.Equal("Conflict.", result.Title);
    }

    [Fact]
    public void Map_ShouldReturn400_ForDomainException()
    {
        var exception = new DomainException("Stock cannot be negative.");

        var result = ExceptionMapper.Map(exception, "trace-000", includeDetails: true);

        Assert.Equal(400, result.Status);
        Assert.Equal("Business rule violation.", result.Title);
    }

    [Fact]
    public void Map_ShouldHideDetails_ForUnknownException_InProduction()
    {
        var exception = new InvalidOperationException("Sensitive internal detail");

        var result = ExceptionMapper.Map(exception, "trace-111", includeDetails: false);

        Assert.Equal(500, result.Status);
        Assert.Equal("An unexpected error occurred.", result.Detail);
    }
}
