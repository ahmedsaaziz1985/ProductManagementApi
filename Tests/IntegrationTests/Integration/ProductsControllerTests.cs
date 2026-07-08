using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using ProductManagementApi.Application.Features.Products.DTOs;

namespace ProductManagementApi.IntegrationTests.Integration;

public class ProductsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ProductsControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateProduct_ShouldReturnCreatedProduct()
    {
        var dto = new CreateProductDto($"Integration Product {Guid.NewGuid()}", "Created from integration test", 49.99m, 5);

        var response = await _client.PostAsJsonAsync("/api/products", dto);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var product = await response.Content.ReadFromJsonAsync<ProductDto>();
        Assert.NotNull(product);
        Assert.Equal(dto.Name, product.Name);
    }

    [Fact]
    public async Task GetProducts_ShouldReturnOk()
    {
        var response = await _client.GetAsync("/api/products");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task SearchProducts_ShouldReturnOk()
    {
        var response = await _client.GetAsync("/api/products/search?search=USD");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
