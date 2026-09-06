using FluentValidation;

namespace DVLD.Application.Features.People.Update;

public class UpdatePersonalInfoValidator : AbstractValidator<UpdatePersonalInfoCommand>
{
    public UpdatePersonalInfoValidator()
    {
        RuleFor(x => x.DateOfBirth)
            .NotEmpty()
            .WithMessage("Date of birth is required");
        
        RuleFor(x => x.NationalityCountryCode)
            .NotEmpty()
            .WithMessage("Nationality country code is required");
    }
}