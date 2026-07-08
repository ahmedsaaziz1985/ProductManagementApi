using System.Net;
using System.Net.Http.Json;
using ProductManagementApi.Application.Common.Models;
using ProductManagementApi.Application.Features.Auth.DTOs;
namespace ProductManagementApi.IntegrationTests.Integration;

public class AuthControllerTests : IClassFixture<IntegrationTestWebAppFactory>
{
    private readonly HttpClient _client;

    public AuthControllerTests(IntegrationTestWebAppFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Register_ShouldReturnToken_WhenDataIsValid()
    {
        var email = $"register_{Guid.NewGuid()}@test.com";

        var response = await _client.PostAsJsonAsync(
            "/api/auth/register",
            new RegisterUserDto(email, "Password1", "Password1"));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var auth = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
        Assert.NotNull(auth);
        Assert.False(string.IsNullOrWhiteSpace(auth.Token));
        Assert.Equal(email, auth.Email);
    }

    [Fact]
    public async Task Login_ShouldReturnToken_WhenCredentialsAreValid()
    {
        var email = $"login_{Guid.NewGuid()}@test.com";
        var password = "Password1";

        await _client.PostAsJsonAsync(
            "/api/auth/register",
            new RegisterUserDto(email, password, password));

        var response = await _client.PostAsJsonAsync(
            "/api/auth/login",
            new LoginUserDto(email, password));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var auth = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
        Assert.NotNull(auth);
        Assert.False(string.IsNullOrWhiteSpace(auth.Token));
    }

    [Fact]
    public async Task Login_ShouldReturnUnauthorized_WhenPasswordIsInvalid()
    {
        var email = $"invalid_{Guid.NewGuid()}@test.com";

        await _client.PostAsJsonAsync(
            "/api/auth/register",
            new RegisterUserDto(email, "Password1", "Password1"));

        var response = await _client.PostAsJsonAsync(
            "/api/auth/login",
            new LoginUserDto(email, "WrongPassword1"));

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        Assert.NotNull(error);
        Assert.Equal(401, error.Status);
        Assert.Equal("Invalid email or password.", error.Detail);
    }}
