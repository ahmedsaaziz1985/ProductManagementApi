using Moq;
using ProductManagementApi.Application.Common.Exceptions;
using ProductManagementApi.Application.Common.Interfaces.Persistence;
using ProductManagementApi.Application.Features.Products.Queries.GetProductById;
using ProductManagementApi.Domain.Entities;

namespace ProductManagementApi.IntegrationTests.Unit.Application.Queries;

public class GetProductByIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnProduct_WhenProductExists()
    {
        var product = Product.Create("Monitor", "4K monitor", 399m, 8);
        var repository = new Mock<IProductRepository>();
        repository.Setup(r => r.GetByIdAsync(product.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        var handler = new GetProductByIdQueryHandler(repository.Object);

        var result = await handler.Handle(new GetProductByIdQuery(product.Id), CancellationToken.None);

        Assert.Equal(product.Id, result.Id);
        Assert.Equal("Monitor", result.Name);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFound_WhenProductDoesNotExist()
    {
        var repository = new Mock<IProductRepository>();
        repository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        var handler = new GetProductByIdQueryHandler(repository.Object);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.Handle(new GetProductByIdQuery(Guid.NewGuid()), CancellationToken.None));
    }
}
