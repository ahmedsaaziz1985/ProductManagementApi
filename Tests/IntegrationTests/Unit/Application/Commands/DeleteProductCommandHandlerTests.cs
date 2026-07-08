using Moq;
using ProductManagementApi.Application.Common.Exceptions;
using ProductManagementApi.Application.Common.Interfaces.Persistence;
using ProductManagementApi.Application.Features.Products.Commands.DeleteProduct;
using ProductManagementApi.Domain.Entities;

namespace ProductManagementApi.IntegrationTests.Unit.Application.Commands;

public class DeleteProductCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldDeleteProduct_WhenProductExists()
    {
        var product = Product.Create("Mouse", "Wireless mouse", 29.99m, 50);
        var repository = new Mock<IProductRepository>();
        repository.Setup(r => r.GetByIdAsync(product.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);
        repository.Setup(r => r.DeleteAsync(product, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = new DeleteProductCommandHandler(repository.Object);

        await handler.Handle(new DeleteProductCommand(product.Id), CancellationToken.None);

        repository.Verify(r => r.DeleteAsync(product, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFound_WhenProductDoesNotExist()
    {
        var repository = new Mock<IProductRepository>();
        repository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        var handler = new DeleteProductCommandHandler(repository.Object);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.Handle(new DeleteProductCommand(Guid.NewGuid()), CancellationToken.None));
    }
}
