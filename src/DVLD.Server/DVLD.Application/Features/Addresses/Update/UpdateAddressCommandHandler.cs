using DVLD.Application.Abstractions.Persistence;
using DVLD.Domain.Common;
using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.Addresses.Update;

public class UpdateAddressCommandHandler(IUnitOfWork uow, UpdateAddressValidator validator)
    : IRequestHandler<UpdateAddressCommand, ErrorOr<Updated>>
{
    private readonly IUnitOfWork _uow = uow;
    private readonly UpdateAddressValidator _validator = validator;

    public async Task<ErrorOr<Updated>> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request,cancellationToken);
        
        if(!validationResult.IsValid)
            return  Error.Validation("Address.Validation",
                validationResult.Errors.First().ErrorMessage);

        try
        {
            await _uow.AddressRepository.UpdateAsync(request.AddressId, request.AddressType,
                request.CountryCode, request.City, request.Governorate, request.Street, request.BuildingNumber,
                request.ApartmentNumber, request.PostalCode, request.AdditionalDetails, cancellationToken);
            await uow.SaveChangesAsync(cancellationToken);
            return Result.Updated;
        }
        catch (DomainException e)
        {
            Console.WriteLine(e);
            return Error.Validation("Domain.Rule.Violation", e.Message);
        }
        catch (KeyNotFoundException e)
        {
            return Error.NotFound("Address.NotFound", e.Message);
        }
        catch (Exception e)
        {
            return Error.Failure("Error", e.Message);
        }
    }
}