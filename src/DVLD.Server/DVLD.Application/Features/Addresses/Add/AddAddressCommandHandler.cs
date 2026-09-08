using DVLD.Application.Abstractions.Persistence;
using DVLD.Domain.Common;
using DVLD.Domain.Entities;
using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.Addresses.Add;

public sealed class AddAddressCommandHandler(IUnitOfWork uow, AddAddressValidator validator)
    : IRequestHandler<AddAddressCommand, ErrorOr<Success>>
{
    private readonly IUnitOfWork _uow = uow;
    private readonly AddAddressValidator _validator = validator;

    public async Task<ErrorOr<Success>> Handle(AddAddressCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return Error.Validation("Invalid.Address", validationResult.Errors.First().ErrorMessage);

        try
        {
            var address = Address.Create(request.PersonId, (AddressType)request.AddressType,
                request.CountryCode, request.City, request.Governorate, request.Street,
                request.BuildingNumber, request.ApartmentNumber, request.PostalCode, request.AdditionalDetails);
            await _uow.AddressRepository.AddAsync(address, cancellationToken);
            return Result.Success;
        }
        catch (DomainException e)
        {
            Console.WriteLine(e);
            return Error.Validation("Domain.Rule.Violation", e.Message);
        }
        catch (KeyNotFoundException e)
        {
            return Error.NotFound("Person.NotFound", e.Message);
        }
        catch (Exception e)
        {
            return Error.Failure("Error", e.Message);
        }
    }
}