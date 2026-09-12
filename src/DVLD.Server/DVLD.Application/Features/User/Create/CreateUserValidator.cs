using FluentValidation;

namespace DVLD.Application.Features.User.Create;

public sealed class CreateUserValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserValidator()
    {
        
        RuleFor(x => x.PersonId)
            .NotEmpty()
            .WithMessage("PersonId is required.");

        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("Username is required.")
            .MinimumLength(3)
            .WithMessage("Username must be at least 3 characters.")
            .MaximumLength(20)
            .WithMessage("Username must not exceed 20 characters.")
            .Matches("^[a-zA-Z0-9_]+$")
            .WithMessage("Username can only contain letters, numbers, and underscores.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .MinimumLength(8)
            .WithMessage("Password must be at least 8 characters.")
            .MaximumLength(100)
            .WithMessage("Password must not exceed 100 characters.")
            .Matches("[A-Z]")
            .WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]")
            .WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]")
            .WithMessage("Password must contain at least one number.");

        RuleFor(x => x.RoleId)
            .GreaterThan(0)
            .WithMessage("RoleId must be greater than 0.");
    }
}