using DVLD.Application.Abstractions.Persistence;
using DVLD.Application.DTOs;
using DVLD.Domain.Entities;
using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.Addresses.Get;

public sealed class GetPersonAddressesQueryHandler(IUnitOfWork uow)
    : IRequestHandler<GetPersonAddressesQuery, ErrorOr<IReadOnlyList<AddressDto>>>
{
    private readonly IUnitOfWork _uow = uow;

    public async Task<ErrorOr<IReadOnlyList<AddressDto>>> Handle(GetPersonAddressesQuery request, CancellationToken cancellationToken)
    {
        if (request.PersonId == Guid.Empty)
            return Error.Validation("Person.Validation", "Person ID is required");
        try
        {
            var addressType = request.AddressType.HasValue
                ? (AddressType)request.AddressType
                : (AddressType?)null;
            IReadOnlyList<AddressDto> personAddresses = await _uow.AddressRepository
                .GetPersonAddressesAsync(request.PersonId,
                addressType, request.IsPrimary, request.IsActive
                , cancellationToken);
            if (personAddresses.Count == 0)
                return Error.NotFound("No.Addresses",
                    $"Person With ID : {request.PersonId} does not has any Address");
            return personAddresses.ToErrorOr();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Error.Failure("Error", e.Message);
        }
    }
}