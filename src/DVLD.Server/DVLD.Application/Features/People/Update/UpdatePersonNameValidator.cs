using FluentValidation;

namespace DVLD.Application.Features.People.Update;

public sealed class UpdatePersonNameValidator : AbstractValidator<UpdatePersonNameCommand>
{
    public UpdatePersonNameValidator()
    {
        RuleFor(x => x.PersonId != Guid.Empty)
            .NotEmpty()
            .WithMessage("Person ID is required.");
        
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(50)
            .WithMessage("First name is required and should be less than 50 characters.");
        
        RuleFor(x => x.SecondName)
            .NotEmpty()
            .MaximumLength(50)
            .WithMessage("Second name is required and should be less than 50 characters.");
        
        RuleFor(x => x.ThirdName)
            .MaximumLength(50)
            .WithMessage("Third name should be less than 50 characters.");
        
        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(50)
            .WithMessage("Last name is required and should be less than 50 characters.");
        
        RuleFor(x => x.MotherName)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("Mother name is required and should be less than 100 characters.");
    }
}