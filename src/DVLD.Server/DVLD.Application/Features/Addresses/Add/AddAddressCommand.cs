using ErrorOr;
using MediatR;

namespace DVLD.Application.Features.Addresses.Add;

public sealed record AddAddressCommand(
    Guid PersonId,
    byte AddressType,
    string CountryCode,
    string City,
    string? Governorate,
    string? Street,
    string? BuildingNumber,
    string? ApartmentNumber,
    string? PostalCode,
    string? AdditionalDetails
    ) : IRequest<ErrorOr<Success>>;