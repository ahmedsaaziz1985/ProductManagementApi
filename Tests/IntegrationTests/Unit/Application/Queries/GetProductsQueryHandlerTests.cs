using Moq;
using ProductManagementApi.Application.Common.Interfaces.Persistence;
using ProductManagementApi.Application.Features.Products.Queries.GetProducts;
using ProductManagementApi.Domain.Entities;

namespace ProductManagementApi.IntegrationTests.Unit.Application.Queries;

public class GetProductsQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnPaginatedProducts()
    {
        var products = new List<Product>
        {
            Product.Create("Pen", "Blue pen", 2m, 100),
            Product.Create("Book", "Notebook", 5m, 50)
        };

        var repository = new Mock<IProductRepository>();
        repository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);

        var handler = new GetProductsQueryHandler(repository.Object);

        var result = await handler.Handle(new GetProductsQuery(1, 10), CancellationToken.None);

        Assert.Equal(2, result.TotalCount);
        Assert.Equal(2, result.Items.Count);
    }
}
