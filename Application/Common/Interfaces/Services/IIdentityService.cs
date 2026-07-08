using ProductManagementApi.Application.Common.Models;

namespace ProductManagementApi.Application.Common.Interfaces.Services;

public interface IIdentityService
{
    Task<AuthResult> RegisterAsync(string email, string password, CancellationToken cancellationToken = default);
    Task<LoginResult> LoginAsync(string email, string password, CancellationToken cancellationToken = default);
}
