using Moq;
using ProductManagementApi.Application.Common.Exceptions;
using ProductManagementApi.Application.Common.Interfaces.Persistence;
using ProductManagementApi.Application.Features.Products.Commands.UpdateProduct;
using ProductManagementApi.Domain.Entities;

namespace ProductManagementApi.IntegrationTests.Unit.Application.Commands;

public class UpdateProductCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldUpdateProduct_WhenProductExists()
    {
        var product = Product.Create("Phone", "Smartphone", 499m, 20);
        var repository = new Mock<IProductRepository>();
        repository.Setup(r => r.GetByIdAsync(product.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);
        repository.Setup(r => r.ExistsByNameExceptIdAsync("Phone Pro", product.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        repository.Setup(r => r.UpdateAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = new UpdateProductCommandHandler(repository.Object);
        var command = new UpdateProductCommand(product.Id, "Phone Pro", "Updated", 599m, 15);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal("Phone Pro", result.Name);
        Assert.Equal(599m, result.Price);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFound_WhenProductDoesNotExist()
    {
        var repository = new Mock<IProductRepository>();
        repository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        var handler = new UpdateProductCommandHandler(repository.Object);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.Handle(new UpdateProductCommand(Guid.NewGuid(), "Phone", "Desc", 10m, 1), CancellationToken.None));
    }
}
