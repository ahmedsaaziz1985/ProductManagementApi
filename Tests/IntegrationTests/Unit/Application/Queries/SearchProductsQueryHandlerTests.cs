using Moq;
using ProductManagementApi.Application.Common.Interfaces.Persistence;
using ProductManagementApi.Application.Features.Products.Queries.SearchProducts;
using ProductManagementApi.Domain.Entities;

namespace ProductManagementApi.IntegrationTests.Unit.Application.Queries;

public class SearchProductsQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnMatchingProducts()
    {
        var products = new List<Product> { Product.Create("Pen", "Red pen", 5m, 100) };
        var repository = new Mock<IProductRepository>();
        repository.Setup(r => r.SearchAsync("pen", 1, 10, It.IsAny<CancellationToken>()))
            .ReturnsAsync((products, 1));

        var handler = new SearchProductsQueryHandler(repository.Object);

        var result = await handler.Handle(new SearchProductsQuery("pen"), CancellationToken.None);

        Assert.Single(result.Items);
        Assert.Equal("Pen", result.Items[0].Name);
        Assert.Equal(1, result.TotalCount);
    }
}
