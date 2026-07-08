using FluentValidation;
using ProductManagementApi.Application.Features.Auth.Commands.LoginUser;

namespace ProductManagementApi.Application.Features.Auth.Validators;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(command => command.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(256);

        RuleFor(command => command.Password)
            .NotEmpty()
            .MaximumLength(100);
    }
}
