using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ProductManagementApi.Application.Common.Exceptions;
using ProductManagementApi.Application.Common.Interfaces.Services;
using ProductManagementApi.Application.Common.Models;
using ProductManagementApi.Infrastructure.Identity;

namespace ProductManagementApi.Infrastructure.Services;

public class IdentityService(
    UserManager<ApplicationUser> userManager,
    JwtTokenService jwtTokenService,
    ILogger<IdentityService> logger) : IIdentityService
{
    public async Task<AuthResult> RegisterAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var existingUser = await userManager.FindByEmailAsync(email);
        if (existingUser is not null)
        {
            logger.LogWarning("Registration failed. Email already exists: {Email}", email);
            throw new ConflictException(nameof(email), "Email is already registered.");
        }

        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            var errors = result.Errors
                .GroupBy(error => error.Code, error => error.Description)
                .ToDictionary(group => group.Key, group => group.ToArray());

            logger.LogWarning(
                "Registration failed for {Email}. IdentityErrors: {@Errors}",
                email,
                errors);

            throw new ValidationException(errors);
        }

        logger.LogInformation("User registered successfully: {Email}", email);

        return CreateAuthResult(user);
    }

    public async Task<LoginResult> LoginAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
        {
            logger.LogWarning("Login failed. User not found for email: {Email}", email);
            return LoginResult.Failure();
        }

        var isValidPassword = await userManager.CheckPasswordAsync(user, password);
        if (!isValidPassword)
        {
            logger.LogWarning("Login failed. Invalid password for email: {Email}", email);
            return LoginResult.Failure();
        }

        logger.LogInformation("User logged in successfully: {Email}", email);

        return LoginResult.Success(CreateAuthResult(user));
    }

    private AuthResult CreateAuthResult(ApplicationUser user) =>
        new()
        {
            UserId = user.Id,
            Email = user.Email ?? string.Empty,
            Token = jwtTokenService.GenerateToken(user),
            ExpiresAt = jwtTokenService.GetExpiration()
        };
}
