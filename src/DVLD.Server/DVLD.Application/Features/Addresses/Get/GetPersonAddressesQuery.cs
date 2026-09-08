using DVLD.Application.DTOs;
using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.Addresses.Get;

public sealed record GetPersonAddressesQuery(
    Guid PersonId,
    byte? AddressType,
    bool? IsPrimary,
    bool? IsActive
    ) : IRequest<ErrorOr<IReadOnlyList<AddressDto>>>;