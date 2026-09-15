using FluentValidation;

namespace DVLD.Application.Features.Authentication.Login;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("Username is required.")
            .MinimumLength(3)
            .WithMessage("Invalid Username.")
            .MaximumLength(20)
            .WithMessage("Invalid Username.")
            .Matches("^[a-zA-Z0-9_]+$")
            .WithMessage("Invalid Username.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .MinimumLength(8)
            .WithMessage("Invalid Password.")
            .MaximumLength(100)
            .WithMessage("Invalid Password.")
            .Matches("[A-Z]")
            .WithMessage("Invalid Password.")
            .Matches("[a-z]")
            .WithMessage("Invalid Password.")
            .Matches("[0-9]")
            .WithMessage("Invalid Password.");
    }
}