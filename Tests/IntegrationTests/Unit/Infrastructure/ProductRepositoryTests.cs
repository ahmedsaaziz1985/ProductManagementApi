using Microsoft.EntityFrameworkCore;
using ProductManagementApi.Domain.Entities;
using ProductManagementApi.Infrastructure.Persistence;
using ProductManagementApi.Infrastructure.Repositories;

namespace ProductManagementApi.IntegrationTests.Unit.Infrastructure;

public class ProductRepositoryTests
{
    [Fact]
    public async Task SearchAsync_ShouldFindProduct_ByName()
    {
        await using var context = CreateContext();
        var repository = new ProductRepository(context);
        await repository.AddAsync(Product.Create("Pen", "Red pen", 5m, 100));

        var (items, totalCount) = await repository.SearchAsync("pen", 1, 10);

        Assert.Equal(1, totalCount);
        Assert.Single(items);
        Assert.Equal("Pen", items[0].Name);
    }

    [Fact]
    public async Task SearchAsync_ShouldFindProduct_ByCurrency()
    {
        await using var context = CreateContext();
        var repository = new ProductRepository(context);
        await repository.AddAsync(Product.Create("Book", "Notebook", 10m, 20, "USD"));

        var (items, totalCount) = await repository.SearchAsync("USD", 1, 10);

        Assert.Equal(1, totalCount);
        Assert.Equal("USD", items[0].Currency);
    }

    [Fact]
    public async Task ExistsByNameAsync_ShouldReturnTrue_WhenProductExists()
    {
        await using var context = CreateContext();
        var repository = new ProductRepository(context);
        await repository.AddAsync(Product.Create("Headphones", "Wireless", 199m, 25));

        var exists = await repository.ExistsByNameAsync("Headphones");

        Assert.True(exists);
    }

    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }
}
