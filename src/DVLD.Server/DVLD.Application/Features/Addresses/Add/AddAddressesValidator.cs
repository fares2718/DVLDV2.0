using FluentValidation;

namespace DVLD.Application.Features.Addresses.Add;

public sealed class AddAddressesValidator
    : AbstractValidator<AddAddressesCommand>
{
    public AddAddressesValidator()
    {
        RuleFor(x => x.Addresses)
            .NotEmpty()
            .WithMessage("At least one address is required.");

        RuleForEach(x => x.Addresses)
            .SetValidator(new AddAddressValidator());
    }
}