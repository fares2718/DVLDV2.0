using FluentValidation;

namespace DVLD.Application.Features.People.Update;

public sealed class UpdatePersonContactInfoValidator : AbstractValidator<UpdatePersonContactInfoCommand>
{
    public UpdatePersonContactInfoValidator()
    {
        RuleFor(c => c.PersonId)
            .NotEmpty()
            .WithMessage("Person ID is required.");
        
        RuleFor(c => c.Phone)
            .NotEmpty()
            .MaximumLength(20)
            .WithMessage("Phone is required and should be less than 20 characters.");
        
        RuleFor(c => c.AltPhone)
            .MaximumLength(20)
            .WithMessage("Alternative Phone should be less than 20 characters.");
    }
}