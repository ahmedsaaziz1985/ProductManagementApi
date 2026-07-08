using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using ProductManagementApi.Application.Features.Auth.DTOs;
using ProductManagementApi.Application.Features.Products.DTOs;

namespace ProductManagementApi.IntegrationTests.Integration;

public class ProductsControllerTests : IClassFixture<IntegrationTestWebAppFactory>
{
    private readonly HttpClient _client;

    public ProductsControllerTests(IntegrationTestWebAppFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetProducts_ShouldReturnUnauthorized_WhenTokenIsMissing()
    {
        var response = await _client.GetAsync("/api/products");

        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task CreateProduct_ShouldReturnCreatedProduct_WhenAuthenticated()
    {
        var token = await AuthenticateAsync();

        var request = new HttpRequestMessage(HttpMethod.Post, "/api/products");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Content = JsonContent.Create(
            new CreateProductDto($"Integration Product {Guid.NewGuid()}", "Created from integration test", 49.99m, 5));

        var response = await _client.SendAsync(request);

        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);

        var product = await response.Content.ReadFromJsonAsync<ProductDto>();
        Assert.NotNull(product);
    }

    [Fact]
    public async Task GetProducts_ShouldReturnOk_WhenAuthenticated()
    {
        var token = await AuthenticateAsync();

        var request = new HttpRequestMessage(HttpMethod.Get, "/api/products");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(request);

        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
    }

    private async Task<string> AuthenticateAsync()
    {
        var email = $"user_{Guid.NewGuid()}@test.com";
        var password = "Password1";

        var registerResponse = await _client.PostAsJsonAsync(
            "/api/auth/register",
            new RegisterUserDto(email, password, password));

        registerResponse.EnsureSuccessStatusCode();

        var auth = await registerResponse.Content.ReadFromJsonAsync<AuthResponseDto>();
        return auth!.Token;
    }
}
