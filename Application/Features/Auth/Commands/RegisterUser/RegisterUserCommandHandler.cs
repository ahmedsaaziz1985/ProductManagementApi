using MediatR;
using ProductManagementApi.Application.Common.Interfaces.Services;
using ProductManagementApi.Application.Features.Auth.DTOs;

namespace ProductManagementApi.Application.Features.Auth.Commands.RegisterUser;

public class RegisterUserCommandHandler(IIdentityService identityService)
    : IRequestHandler<RegisterUserCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var result = await identityService.RegisterAsync(request.Email, request.Password, cancellationToken);

        return new AuthResponseDto(
            result.UserId,
            result.Email,
            result.Token,
            result.ExpiresAt);
    }
}
