namespace ProductManagementApi.Application.Common.Models;

public class LoginResult
{
    public bool Succeeded { get; init; }
    public AuthResult? Auth { get; init; }
    public string ErrorMessage { get; init; } = "Invalid email or password.";

    public static LoginResult Success(AuthResult auth) =>
        new() { Succeeded = true, Auth = auth };

    public static LoginResult Failure(string? errorMessage = null) =>
        new()
        {
            Succeeded = false,
            ErrorMessage = string.IsNullOrWhiteSpace(errorMessage)
                ? "Invalid email or password."
                : errorMessage
        };
}
