using FluentValidation;

namespace DVLD.Application.Features.User.Get;

public sealed class GetUsersWithFilterQueryValidator : AbstractValidator<GetUsersWithFilterQuery>
{
    public GetUsersWithFilterQueryValidator()
    {
        RuleFor(x => x.Filter.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page number must be 1 or greater.");

        RuleFor(x => x.Filter.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("Page size must be between 1 and 100 items.");

        // 2. Optional String Length Safety Constraints
        RuleFor(x => x.Filter.Search)
            .MaximumLength(100).WithMessage("Search query is too long.")
            .MinimumLength(2).When(x => x.Filter.Search != null)
            .WithMessage("Search query must be at least 2 characters.");

        RuleFor(x => x.Filter.NationalId)
            .Matches(@"^\d+$").When(x => !string.IsNullOrEmpty(x.Filter.NationalId))
            .WithMessage("National ID must contain digits only.");

        RuleFor(x => x.Filter.Phone)
            .Matches(@"^\+?[1-9]\d{1,14}$").When(x => !string.IsNullOrEmpty(x.Filter.Phone))
            .WithMessage("Invalid phone number format.");
    }
}