using MediatR;
using ProductManagementApi.Application.Common.Models;

namespace ProductManagementApi.Application.Features.Auth.Commands.LoginUser;

public record LoginUserCommand(string Email, string Password) : IRequest<LoginResult>;