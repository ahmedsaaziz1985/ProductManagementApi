using Moq;
using ProductManagementApi.Application.Common.Interfaces.Services;
using ProductManagementApi.Application.Common.Models;
using ProductManagementApi.Application.Features.Auth.Commands.LoginUser;

namespace ProductManagementApi.IntegrationTests.Unit.Application.Commands;

public class LoginUserCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnAuthResponse_WhenLoginSucceeds()
    {
        var authResult = new AuthResult
        {
            UserId = "user-1",
            Email = "user@test.com",
            Token = "jwt-token",
            ExpiresAt = DateTime.UtcNow.AddHours(1)
        };

        var identityService = new Mock<IIdentityService>();
        identityService.Setup(s => s.LoginAsync("user@test.com", "Password1", It.IsAny<CancellationToken>()))
            .ReturnsAsync(LoginResult.Success(authResult));

        var handler = new LoginUserCommandHandler(identityService.Object);

        var result = await handler.Handle(new LoginUserCommand("user@test.com", "Password1"), CancellationToken.None);

        Assert.True(result.Succeeded);
        Assert.Equal("user@test.com", result.Auth!.Email);
        Assert.Equal("jwt-token", result.Auth.Token);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenLoginFails()
    {
        var identityService = new Mock<IIdentityService>();
        identityService.Setup(s => s.LoginAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(LoginResult.Failure());

        var handler = new LoginUserCommandHandler(identityService.Object);

        var result = await handler.Handle(new LoginUserCommand("user@test.com", "wrong"), CancellationToken.None);

        Assert.False(result.Succeeded);
        Assert.Equal("Invalid email or password.", result.ErrorMessage);
    }
}
