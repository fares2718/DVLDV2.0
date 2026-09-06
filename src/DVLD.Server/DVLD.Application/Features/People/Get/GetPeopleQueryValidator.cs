using FluentValidation;

namespace DVLD.Application.Features.People.Get;

public sealed class GetPeopleQueryValidator
    : AbstractValidator<GetPeopleQuery>
{
    private static readonly string[] AllowedSortFields =
    [
        "NationalId",
        "Name",
        "Phone",
        "Email"
    ];

    private static readonly string[] AllowedGenders =
    [
        "Male",
        "Female"
    ];

    public GetPeopleQueryValidator()
    {
        RuleFor(x => x.Search)
            .MaximumLength(100)
            .When(x => x.Search is not null);

        RuleFor(x => x.NationalId)
            .MaximumLength(20)
            .When(x => x.NationalId is not null);

        RuleFor(x => x.Name)
            .MaximumLength(100)
            .When(x => x.Name is not null);

        RuleFor(x => x.Gender)
            .Must(gender =>
                gender is null ||
                AllowedGenders.Contains(
                    gender,
                    StringComparer.OrdinalIgnoreCase))
            .WithMessage("Gender must be Male or Female.");

        RuleFor(x => x.Phone)
            .MaximumLength(20)
            .When(x => x.Phone is not null);
        
        RuleFor(x => x.Email)
            .MaximumLength(100)
            .When(x => x.Email is not null);

        RuleFor(x => x.SortBy)
            .Must(sortBy =>
                sortBy is null ||
                AllowedSortFields.Contains(
                    sortBy,
                    StringComparer.OrdinalIgnoreCase))
            .WithMessage("Invalid sort field.");

        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100);
    }
}