using Moq;
using ProductManagementApi.Application.Common.Exceptions;
using ProductManagementApi.Application.Common.Interfaces.Persistence;
using ProductManagementApi.Application.Features.Products.Commands.CreateProduct;
using ProductManagementApi.Domain.Entities;

namespace ProductManagementApi.IntegrationTests.Unit.Application.Commands;

public class CreateProductCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreateProduct_WhenNameIsUnique()
    {
        var repository = new Mock<IProductRepository>();
        repository.Setup(r => r.ExistsByNameAsync("Laptop", It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        repository.Setup(r => r.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product product, CancellationToken _) => product);

        var handler = new CreateProductCommandHandler(repository.Object);
        var command = new CreateProductCommand("Laptop", "Gaming laptop", 999.99m, 10);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal("Laptop", result.Name);
        Assert.Equal(999.99m, result.Price);
        repository.Verify(r => r.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowConflictException_WhenNameExists()
    {
        var repository = new Mock<IProductRepository>();
        repository.Setup(r => r.ExistsByNameAsync("Laptop", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var handler = new CreateProductCommandHandler(repository.Object);
        var command = new CreateProductCommand("Laptop", "Gaming laptop", 999.99m, 10);

        await Assert.ThrowsAsync<ConflictException>(() =>
            handler.Handle(command, CancellationToken.None));
    }
}
