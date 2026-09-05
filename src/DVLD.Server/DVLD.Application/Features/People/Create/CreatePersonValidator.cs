using FluentValidation;

namespace DVLD.Application.Features.People.Create;

public sealed class CreatePersonCommandValidator
    : AbstractValidator<CreatePersonCommand>
{
    public CreatePersonCommandValidator()
    {
        RuleFor(x => x.NationalId)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.SecondName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.ThirdName)
            .MaximumLength(50)
            .When(x => x.ThirdName is not null);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.MotherName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.DateOfBirth)
            .LessThan(DateOnly.FromDateTime(DateTime.UtcNow));

        RuleFor(x => x.Phone)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(255);

        RuleFor(x => x.NationalityCountryCode)
            .NotEmpty()
            .Length(2);
    }
}