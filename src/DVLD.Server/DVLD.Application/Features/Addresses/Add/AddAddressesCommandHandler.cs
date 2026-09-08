using DVLD.Application.Abstractions.Persistence;
using DVLD.Domain.Common;
using DVLD.Domain.Entities;
using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.Addresses.Add;

public sealed class AddAddressesCommandHandler(IUnitOfWork uow, AddAddressesValidator validator)
    : IRequestHandler<AddAddressesCommand, ErrorOr<Success>>
{
    private readonly IUnitOfWork _uow = uow;
    private readonly AddAddressesValidator _validator = validator;

    public async Task<ErrorOr<Success>> Handle(AddAddressesCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return Error.Validation("Invalid.Address", 
                validationResult.Errors.First().ErrorMessage);
        
        IEnumerable<Address> addresses = request.Addresses.Select(
            a => Address.Create(
                a.PersonId,
                (AddressType)a.AddressType,
                a.CountryCode,
                a.City,
                a.Governorate,
                a.Street,
                a.BuildingNumber,
                a.ApartmentNumber,
                a.PostalCode,
                a.AdditionalDetails
                )
            );

        try
        {
            await _uow.AddressRepository.AddRangeAsync(addresses, cancellationToken);
            return Result.Success;
        }
        catch (DomainException e)
        {
            Console.WriteLine(e);
            return Error.Validation("Domain.Rule.Violation",e.Message);
        }
        catch (KeyNotFoundException e)
        {
            return Error.NotFound("Person.NotFound",e.Message);
        }
        catch(Exception e)
        {
            return Error.Failure("Error",e.Message);
        }
    }
    
}