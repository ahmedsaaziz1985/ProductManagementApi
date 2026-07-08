using ProductManagementApi.Application.Features.Products.Commands.CreateProduct;
using ProductManagementApi.Application.Features.Products.Validators;

namespace ProductManagementApi.IntegrationTests.Unit.Application.Validators;

public class CreateProductCommandValidatorTests
{
    private readonly CreateProductCommandValidator _validator = new();

    [Fact]
    public void Validate_ShouldPass_ForValidCommand()
    {
        var command = new CreateProductCommand("Pen", "Red pen", 5m, 10, "USD");

        var result = _validator.Validate(command);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_ShouldFail_WhenNameIsEmpty()
    {
        var command = new CreateProductCommand("", "Red pen", 5m, 10, "USD");

        var result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(command.Name));
    }

    [Fact]
    public void Validate_ShouldFail_WhenCurrencyIsTooLong()
    {
        var command = new CreateProductCommand("Pen", "Red pen", 5m, 10, "dollar");

        var result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(command.Currency));
    }
}
