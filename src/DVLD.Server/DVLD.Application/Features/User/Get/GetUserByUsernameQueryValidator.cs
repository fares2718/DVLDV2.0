using FluentValidation;

namespace DVLD.Application.Features.User.Get;

public sealed class GetUserByUsernameQueryValidator : AbstractValidator<GetUserByUsernameQuery>
{
    public GetUserByUsernameQueryValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("Username is required.")
            .MinimumLength(3)
            .WithMessage("Username must be at least 3 characters.")
            .MaximumLength(20)
            .WithMessage("Username must not exceed 20 characters.")
            .Matches("^[a-zA-Z0-9_]+$")
            .WithMessage("Username can only contain letters, numbers, and underscores.");
    }
}