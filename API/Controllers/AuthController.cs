using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManagementApi.Application.Common.Exceptions;
using ProductManagementApi.Application.Common.Models;
using ProductManagementApi.Application.Features.Auth.Commands.LoginUser;
using ProductManagementApi.Application.Features.Auth.Commands.RegisterUser;
using ProductManagementApi.Application.Features.Auth.DTOs;

namespace ProductManagementApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class AuthController(ISender sender) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register(
        [FromBody] RegisterUserDto dto,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new RegisterUserCommand(dto.Email, dto.Password, dto.ConfirmPassword),
            cancellationToken);

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(
        [FromBody] LoginUserDto dto,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new LoginUserCommand(dto.Email, dto.Password),
            cancellationToken);

        if (!result.Succeeded || result.Auth is null)
        {
            return Unauthorized(new ErrorResponse
            {
                Title = "Unauthorized.",
                Status = ApplicationStatusCodes.Status401Unauthorized,
                TraceId = HttpContext.TraceIdentifier,
                Type = "https://tools.ietf.org/html/rfc7235#section-3.1",
                Detail = result.ErrorMessage
            });
        }

        return Ok(new AuthResponseDto(
            result.Auth.UserId,
            result.Auth.Email,
            result.Auth.Token,
            result.Auth.ExpiresAt));
    }
}