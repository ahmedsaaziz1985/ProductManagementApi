namespace ProductManagementApi.Application.Features.Auth.DTOs;

public record RegisterUserDto(string Email, string Password, string ConfirmPassword);

public record LoginUserDto(string Email, string Password);

public record AuthResponseDto(
    string UserId,
    string Email,
    string Token,
    DateTime ExpiresAt);
