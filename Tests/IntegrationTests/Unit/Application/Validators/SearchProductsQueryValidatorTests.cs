using ProductManagementApi.Application.Features.Products.Queries.SearchProducts;
using ProductManagementApi.Application.Features.Products.Validators;

namespace ProductManagementApi.IntegrationTests.Unit.Application.Validators;

public class SearchProductsQueryValidatorTests
{
    private readonly SearchProductsQueryValidator _validator = new();

    [Fact]
    public void Validate_ShouldPass_WhenSearchIsProvided()
    {
        var query = new SearchProductsQuery("pen");

        var result = _validator.Validate(query);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_ShouldFail_WhenSearchIsEmpty()
    {
        var query = new SearchProductsQuery("");

        var result = _validator.Validate(query);

        Assert.False(result.IsValid);
    }
}
