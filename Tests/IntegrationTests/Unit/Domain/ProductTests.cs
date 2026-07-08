using ProductManagementApi.Domain.Entities;
using ProductManagementApi.Domain.Exceptions;

namespace ProductManagementApi.IntegrationTests.Unit.Domain;

public class ProductTests
{
    [Fact]
    public void Create_ShouldCreateProduct_WithValidData()
    {
        var product = Product.Create("Laptop", "Gaming laptop", 999.99m, 10);

        Assert.NotEqual(Guid.Empty, product.Id);
        Assert.Equal("Laptop", product.Name);
        Assert.Equal(999.99m, product.Price);
        Assert.Equal(10, product.Stock);
        Assert.Single(product.DomainEvents);
    }

    [Fact]
    public void Create_ShouldThrow_WhenNameIsEmpty()
    {
        Assert.Throws<DomainException>(() => Product.Create("", "Description", 10m, 5));
    }

    [Fact]
    public void Update_ShouldUpdateProductProperties()
    {
        var product = Product.Create("Phone", "Smartphone", 499m, 20);

        product.Update("Phone Pro", "Updated smartphone", 599m, 15);

        Assert.Equal("Phone Pro", product.Name);
        Assert.Equal(599m, product.Price);
        Assert.NotNull(product.UpdatedAt);
    }

    [Fact]
    public void ReduceStock_ShouldThrow_WhenInsufficientStock()
    {
        var product = Product.Create("Keyboard", "Mechanical keyboard", 79.99m, 5);

        Assert.Throws<DomainException>(() => product.ReduceStock(10));
    }
}
