using FluentValidation;

namespace DVLD.Application.Features.Addresses.Add;

public sealed class AddAddressValidator 
    : AbstractValidator<AddAddressCommand>
{
    public AddAddressValidator()
    {
        RuleFor(x => x.PersonId)
            .NotEmpty()
            .WithMessage("Person ID is required.");

        RuleFor(x => x.AddressType)
            .Must(at => at is 1 or 2)
            .WithMessage("Invalid address type.");

        RuleFor(x => x.CountryCode)
            .NotEmpty()
            .WithMessage("Country code is required.")
            .Length(2, 10)
            .WithMessage("Country code must be between 2 and 10 characters.");

        RuleFor(x => x.City)
            .NotEmpty()
            .WithMessage("City is required.")
            .MaximumLength(100)
            .WithMessage("City must not exceed 100 characters.");

        RuleFor(x => x.Governorate)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.Governorate))
            .WithMessage("Governorate must not exceed 100 characters.");

        RuleFor(x => x.Street)
            .MaximumLength(200)
            .When(x => !string.IsNullOrWhiteSpace(x.Street))
            .WithMessage("Street must not exceed 200 characters.");

        RuleFor(x => x.BuildingNumber)
            .MaximumLength(50)
            .When(x => !string.IsNullOrWhiteSpace(x.BuildingNumber))
            .WithMessage("Building number must not exceed 50 characters.");

        RuleFor(x => x.ApartmentNumber)
            .MaximumLength(50)
            .When(x => !string.IsNullOrWhiteSpace(x.ApartmentNumber))
            .WithMessage("Apartment number must not exceed 50 characters.");

        RuleFor(x => x.PostalCode)
            .MaximumLength(20)
            .When(x => !string.IsNullOrWhiteSpace(x.PostalCode))
            .WithMessage("Postal code must not exceed 20 characters.");

        RuleFor(x => x.AdditionalDetails)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.AdditionalDetails))
            .WithMessage("Additional details must not exceed 500 characters.");
    }
}