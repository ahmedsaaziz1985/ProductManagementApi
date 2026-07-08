using MediatR;
using ProductManagementApi.Application.Common.Interfaces.Services;
using ProductManagementApi.Application.Common.Models;

namespace ProductManagementApi.Application.Features.Auth.Commands.LoginUser;

public class LoginUserCommandHandler(IIdentityService identityService)
    : IRequestHandler<LoginUserCommand, LoginResult>
{
    public Task<LoginResult> Handle(LoginUserCommand request, CancellationToken cancellationToken) =>
        identityService.LoginAsync(request.Email, request.Password, cancellationToken);
}
