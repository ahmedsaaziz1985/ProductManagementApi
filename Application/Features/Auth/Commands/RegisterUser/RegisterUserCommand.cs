using MediatR;
using ProductManagementApi.Application.Features.Auth.DTOs;

namespace ProductManagementApi.Application.Features.Auth.Commands.RegisterUser;

public record RegisterUserCommand(
    string Email,
    string Password,
    string ConfirmPassword) : IRequest<AuthResponseDto>;
