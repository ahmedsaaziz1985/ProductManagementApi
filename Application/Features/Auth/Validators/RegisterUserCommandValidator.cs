using FluentValidation;
using ProductManagementApi.Application.Features.Auth.Commands.RegisterUser;

namespace ProductManagementApi.Application.Features.Auth.Validators;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(command => command.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(256);

        RuleFor(command => command.Password)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(100);

        RuleFor(command => command.ConfirmPassword)
            .Equal(command => command.Password)
            .WithMessage("Password and confirmation password do not match.");
    }
}
